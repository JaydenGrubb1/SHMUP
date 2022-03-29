using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SHMUP
{
	public class Bullet : MonoBehaviour
	{
		public void OnBecameInvisible()
		{
			gameObject.SetActive(false);
		}

		public void OnTriggerEnter2D(Collider2D collision)
		{
			gameObject.SetActive(false);
		}
	}
}