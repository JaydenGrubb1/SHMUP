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

		[Header("Camera Shake")]
		[SerializeField]
		private CameraShakeParams firingShake;
		[SerializeField]
		private CameraShakeParams damageShake;
		[SerializeField]
		private CameraShakeParams deathShake;

		[Header("Component References")]
		public SpriteRenderer mainSprite;
		public SpriteRenderer recticleSprite;
		public ParticleSystem trailSystem;

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

		// Properties
		public float BoostRemaining { get; private set; }
		public float BoostDuration { get { return boostDuration; } }
		public bool BoostDrained { get; private set; } = false;
		public string BulletVariant { get; set; }


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
			//speedMulti = context.action.triggered ? speedBoost : 1;
			//context.action.
			boosting = context.action.triggered;
		}
		#endregion

		public void Start()
		{
			//Cursor.lockState = CursorLockMode.Confined;
			//Cursor.visible = false;
			//screenLower = Camera.main.ScreenToWorldPoint(Vector2.zero).SetZ(0);
			//screenUpper = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height)).SetZ(0);

			BoostRemaining = boostDuration;
		}

		public void Update()
		{
			// Calculate boost status
			BoostRemaining += Time.deltaTime * (boosting && !BoostDrained ? -1 : boostRecharge);
			BoostRemaining = Mathf.Clamp(BoostRemaining, 0, boostDuration);

			if (BoostRemaining == 0)
				BoostDrained = true;
			if (BoostRemaining == boostDuration)
				BoostDrained = false;
			isBoosting = !BoostDrained && BoostRemaining > 0 && boosting;

			// Vibrate controller
			// NEEDS FIXING, VIBRATE CORRECT CONTROLLER
			if (isBoosting)
				Gamepad.current.SetMotorSpeeds(0.25f, 0.75f);
			else
				Gamepad.current.SetMotorSpeeds(0, 0);

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

			// Update trail emmision rate
			// based on current velocity
			ParticleSystem.EmissionModule emm = trailSystem.emission;
			float relativeSpeed = Mathf.InverseLerp(0, maxRelativeSpeed, absVelocity.magnitude);
			float emmisionRate = Mathf.Lerp(trailEmmisionLimits.x, trailEmmisionLimits.y, relativeSpeed);
			emm.rateOverTime = emmisionRate * (isBoosting ? trailEmmisionBoostMulti : 1);

			// Update recticle position
			if (input.currentControlScheme == "Controller")
			{
				targetInput = targetInput.Clamp(GameManager.ScreenLower - recticle.position, GameManager.ScreenUpper - recticle.position);

				Vector3 targetVel = Vector3.Lerp(prevTarget, targetInput, Time.deltaTime * targetSmoothing);
				prevTarget = targetVel;
				Vector3 newPos = recticle.position;
				newPos += (targetVel * Time.deltaTime * targetSpeed);
				recticle.position = newPos.Clamp(GameManager.ScreenLower, GameManager.ScreenUpper);
			}
			else
			{
				Vector2 mousePos = Mouse.current.position.ReadValue();
				Vector3 targetPos = GameManager.MainCamera.ScreenToWorldPoint(mousePos);
				targetPos.z = 0;
				recticle.position = Vector3.Lerp(recticle.position, targetPos, Time.deltaTime * targetSmoothing);
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