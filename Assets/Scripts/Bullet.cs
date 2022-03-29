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
	}
}