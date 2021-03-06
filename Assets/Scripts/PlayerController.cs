using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using SHMUP.Util;
using DG.Tweening;
using System;

namespace SHMUP
{
	public class PlayerController : MonoBehaviour
	{
		#region Variables
		[Header("Components")]
		[SerializeField]
		private PlayerInput input;
		[SerializeField]
		private Transform sprite;
		[SerializeField]
		private Transform recticle;
		[SerializeField]
		private Transform bulletSpawn;

		[Header("Control Variables")]
		[SerializeField]
		private bool useAbsJoystickAngle = true;
		[SerializeField]
		private float virtualRecticleDist = 3.2f;
		[SerializeField]
		private float bulletsPerSecond = 8f;
		[SerializeField]
		private float movementSpeed = 10;
		[SerializeField]
		private float speedBoost = 2;
		[SerializeField]
		private float targetSpeed = 15;
		[SerializeField]
		private float rotationSmoothing = 10;
		[SerializeField]
		private float movementSmoothing = 2.5f;
		[SerializeField]
		private float targetSmoothing = 2.5f;
		[SerializeField]
		private float boostDuration = 5;
		[SerializeField]
		private float boostRecharge = 1.5f;

		[Header("Animation Variables")]
		[SerializeField]
		private Vector2 trailEmmisionLimits = new Vector2(0.3f, 30);
		[SerializeField]
		private float trailEmmisionBoostMulti = 2f;
		[SerializeField]
		private float maxRelativeSpeed = 0.2f;
		[SerializeField]
		private Vector2 fireScaleLimits = new Vector2(0.38f, 0.4f);
		[SerializeField]
		private float fireScaleDuration = 0.06f;
		[SerializeField]
		private Vector2 boostScaleLimits = new Vector2(0.32f, 0.4f);
		[SerializeField]
		private float boostScaleDuration = 0.2f;
		[SerializeField]
		private float invincibleBlinkDuration = 0.2f;
		[SerializeField]
		private int invincibleBlinkLoops = 20;

		[Header("Camera Shake")]
		[SerializeField]
		private CameraShakeParams firingShake;
		[SerializeField]
		private CameraShakeParams damageShake;
		[SerializeField]
		private CameraShakeParams deathShake;

		[Header("Rumble")]
		[SerializeField]
		private Vector2 boostRumble;
		[SerializeField]
		private Vector2 hitRumble;
		[SerializeField]
		private float hitRumbleTime = 0.2f;
		[SerializeField]
		private Vector2 deathRumble;
		[SerializeField]
		private float deathRumbleTime;

		[Header("Component References")]
		public SpriteRenderer mainSprite;
		public SpriteRenderer recticleSprite;
		public ParticleSystem trailSystem;
		public ParticleSystem deathSystem;

		[HideInInspector]
		public PlayerUI playerUI;

		// Input variables
		private Vector3 moveInput = Vector3.zero;
		private Vector3 targetInput = Vector3.zero;
		private bool firing = false;
		private bool boosting = false;
		//private float speedMulti = 1;

		// Calculation variables
		private Vector3 prevMove = Vector3.zero;
		private Vector3 prevTarget = Vector3.zero;
		private bool wasFiring = false;
		private bool isBoosting = false;
		private bool wasBoosting = false;

		// State variables
		private bool invincible;

		// Properties
		public float BoostRemaining { get; private set; }
		public float BoostDuration { get { return boostDuration; } }
		public bool BoostDrained { get; private set; } = false;
		public string BulletVariant { get; set; }
		public int LivesRemaining { get; private set; } = 4;


		public AudioSource audioSource;
		public AudioClip audioClip;
		#endregion

		#region Input Callbacks
		public void OnMove(InputAction.CallbackContext context)
		{
			moveInput = context.ReadValue<Vector2>();
		}

		public void OnLook(InputAction.CallbackContext context)
		{
			targetInput = context.ReadValue<Vector2>();
		}

		public void OnFire(InputAction.CallbackContext context)
		{
			firing = context.action.triggered;
		}

		public void OnBoost(InputAction.CallbackContext context)
		{
			boosting = context.action.triggered;
		}
		#endregion

		public void Start()
		{
			BoostRemaining = boostDuration;
		}

		Vector3 moveME;

		public void Update()
		{
			// Reset all inputs when player dies
			if (LivesRemaining < 1)
			{
				moveInput = Vector3.zero;
				targetInput = Vector3.zero;
				firing = false;
				boosting = false;
			}

			// Calculate boost status
			BoostRemaining += Time.deltaTime * (boosting && !BoostDrained && !invincible ? -1 : boostRecharge);
			BoostRemaining = Mathf.Clamp(BoostRemaining, 0, boostDuration);

			if (BoostRemaining == 0)
				BoostDrained = true;
			if (BoostRemaining == boostDuration)
				BoostDrained = false;
			isBoosting = !BoostDrained && BoostRemaining > 0 && boosting && !invincible;

			// Vibrate controller
			// NEEDS FIXING, VIBRATE CORRECT CONTROLLER
			if (isBoosting)
				Gamepad.current.SetMotorSpeeds(boostRumble.x, boostRumble.y);
			else if (!invincible)
				ResetRumble();

			// Scale player when boost start
			if (isBoosting && !wasBoosting)
				sprite.DOScaleY(boostScaleLimits.x, boostScaleDuration);
			if (wasBoosting && !isBoosting)
				sprite.DOScaleY(boostScaleLimits.y, boostScaleDuration);
			wasBoosting = isBoosting;

			// Update player position
			Vector3 velocity = Vector3.Lerp(prevMove, moveInput * (isBoosting ? speedBoost : 1), Time.deltaTime * movementSmoothing);
			prevMove = velocity;
			Vector3 absVelocity = velocity * Time.deltaTime * movementSpeed;
			sprite.position += absVelocity;

			playerUI.CustomUpdate();

			// Update trail emmision rate
			// based on current velocity
			ParticleSystem.EmissionModule emm = trailSystem.emission;
			float relativeSpeed = Mathf.InverseLerp(0, maxRelativeSpeed, absVelocity.magnitude);
			float emmisionRate = Mathf.Lerp(trailEmmisionLimits.x, trailEmmisionLimits.y, relativeSpeed);
			emm.rateOverTime = emmisionRate * (isBoosting ? trailEmmisionBoostMulti : 1);

			// Update recticle position
			if (input.currentControlScheme == "Controller")
			{
				if (useAbsJoystickAngle)
				{
					recticleSprite.enabled = false;
					if (targetInput.sqrMagnitude > 0)
						moveME = targetInput.normalized * virtualRecticleDist;

					recticle.position = Vector3.Lerp(recticle.position, sprite.position + moveME, Time.deltaTime * targetSmoothing);
				}
				else
				{
					targetInput = targetInput.Clamp(GameManager.ScreenLower - recticle.position, GameManager.ScreenUpper - recticle.position);

					Vector3 targetVel = Vector3.Lerp(prevTarget, targetInput, Time.deltaTime * targetSmoothing);
					prevTarget = targetVel;
					Vector3 newPos = recticle.position;
					newPos += (targetVel * Time.deltaTime * targetSpeed);
					recticle.position = newPos.Clamp(GameManager.ScreenLower, GameManager.ScreenUpper);
				}
			}
			else
			{
				Vector2 mousePos = Mouse.current.position.ReadValue();
				Vector3 worldMousePos = GameManager.MainCamera.ScreenToWorldPoint(mousePos).SetZ(0);
				recticle.position = Vector3.Lerp(recticle.position, worldMousePos, Time.deltaTime * targetSmoothing);
			}

			// Update player rotation
			Vector3 diff = recticle.position - sprite.position;
			float angle = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
			Quaternion quat = Quaternion.Euler(new Vector3(0, 0, angle));
			sprite.rotation = Quaternion.Lerp(sprite.rotation, quat, Time.deltaTime * rotationSmoothing);

			// Fire bullets
			if (firing && !wasFiring)
			{
				GameObject bullet = ObjectPool.Get(BulletVariant);
				bullet.SetActive(true);
				bullet.transform.position = bulletSpawn.position;
				bullet.GetComponent<Rigidbody2D>().AddForce(diff.normalized * 1000);
				wasFiring = true;
				audioSource.PlayOneShot(audioClip);
				Invoke("ResetFireState", 1f / bulletsPerSecond);

				ShakeCamera(firingShake);

				// Animate recoil
				sprite.DOScaleX(fireScaleLimits.x, fireScaleDuration).OnComplete(() =>
				{
					sprite.DOScaleX(fireScaleLimits.y, fireScaleDuration);
				});
			}
		}

		public void OnTriggerEnter2D(Collider2D collision)
		{
			if (collision.tag == "Enemy")
			{
				if (!invincible)
				{
					invincible = true;
					LivesRemaining--;
					if (LivesRemaining > 0)
					{
						ShakeCamera(damageShake);
						Gamepad.current.SetMotorSpeeds(hitRumble.x, hitRumble.y);
						Invoke("ResetRumble", hitRumbleTime);
						mainSprite.DOColor(Color.white, invincibleBlinkDuration).SetLoops(invincibleBlinkLoops, LoopType.Yoyo).OnComplete(() => invincible = false);
					}
					else
					{
						ShakeCamera(deathShake);
						Gamepad.current.SetMotorSpeeds(deathRumble.x, deathRumble.y);
						Invoke("ResetRumble", deathRumbleTime);
						deathSystem.Play();
						mainSprite.DOColor(Color.white, invincibleBlinkDuration);
					}
				}
			}
		}

		private void ResetRumble()
		{
			Gamepad.current.SetMotorSpeeds(0, 0);
		}

		private void ResetFireState()
		{
			wasFiring = false;
		}

		private void ShakeCamera(CameraShakeParams shake)
		{
			GameManager.MainCamera.transform.DOShakePosition(shake.duration, shake.strength, shake.vibrato, shake.randomness, shake.snapping, shake.fadeOut);
		}

		[Serializable]
		struct CameraShakeParams
		{
			public float duration;
			public float strength;
			public int vibrato;
			public float randomness;
			public bool snapping;
			public bool fadeOut;
		}
	}
}