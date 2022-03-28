using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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

		[Header("Variables")]
		[SerializeField]
		private float movementSpeed = 10;
		[SerializeField]
		private float targetSpeed = 15;
		[SerializeField]
		private float rotationSmoothing = 10;
		[SerializeField]
		private float movementSmoothing = 2.5f;
		[SerializeField]
		private float targetSmoothing = 2.5f;

		// Input variables
		private Vector3 moveInput = Vector3.zero;
		private Vector3 targetInput = Vector3.zero;
		private bool firing = false;

		// Calculation variables
		private Vector3 prevMove = Vector3.zero;
		private Vector3 prevTarget = Vector3.zero;
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
		#endregion

		public void Start()
		{
			Cursor.lockState = CursorLockMode.Confined;
			Cursor.visible = false;
		}

		bool wasFiring = false;

		public void Update()
		{
			// Update player position
			Vector3 velocity = Vector3.Lerp(prevMove, moveInput, Time.deltaTime * movementSmoothing);
			prevMove = velocity;
			sprite.position += velocity * Time.deltaTime * movementSpeed;

			// Update recticle position
			if (input.currentControlScheme == "Controller")
			{
				Vector3 targetVel = Vector3.Lerp(prevTarget, targetInput, Time.deltaTime * targetSmoothing);
				prevTarget = targetVel;
				recticle.position += targetVel * Time.deltaTime * targetSpeed;
			}
			else
			{
				Vector2 mousePos = Mouse.current.position.ReadValue();
				Vector3 targetPos = Camera.main.ScreenToWorldPoint(mousePos);
				targetPos.z = 0;
				recticle.position = Vector3.Lerp(recticle.position, targetPos, Time.deltaTime * targetSmoothing);
			}

			// Update player rotation
			Vector3 diff = recticle.position - sprite.position;
			float angle = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
			Quaternion quat = Quaternion.Euler(new Vector3(0, 0, angle));
			sprite.rotation = Quaternion.Lerp(sprite.rotation, quat, Time.deltaTime * rotationSmoothing);

			// Temp
			if (firing && !wasFiring)
			{
				GameObject go = ObjectPool.Get("player-bullet");
				go.SetActive(true);
				go.transform.position = bulletSpawn.position;
				go.GetComponent<Rigidbody2D>().AddForce(diff.normalized * 1000);
				wasFiring = true;
			}

			if (!firing)
				wasFiring = false;
		}
	}
}