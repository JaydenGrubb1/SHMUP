using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SHMUP
{
    public class PlayerController : MonoBehaviour
    {
		[SerializeField]
		private float speed = 10;

		private SpriteRenderer sprite;

		private Vector3 movement = Vector2.zero;
		private bool firing = false;

		public void OnMove(InputAction.CallbackContext context)
		{
			Vector2 input = context.ReadValue<Vector2>();
			movement = input;
		}

		public void OnFire(InputAction.CallbackContext context)
		{
			firing = context.action.triggered;
		}

		public void Start()
		{
			sprite = GetComponent<SpriteRenderer>();
		}

		public void Update()
		{
			transform.position += movement * Time.deltaTime * speed;

			if (firing)
			{
				sprite.color = Color.yellow;
			}
			else
			{
				sprite.color = Color.red;
			}
		}
	}
}