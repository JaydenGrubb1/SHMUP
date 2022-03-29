using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SHMUP
{
	public class RadialBulletSpawner : MonoBehaviour
	{
		[SerializeField]
		private string bulletType = "enemy-bullet";
		[SerializeField]
		private float bulletsPerTurn = 8;
		[SerializeField]
		private float bulletsPerSecond = 4;
		[SerializeField]
		private float velocity = 600;
		[SerializeField]
		private bool reverseRotation = false;
		[SerializeField]
		private float offsetAngle = 0;

		private float lastAngle = 0;

		public void OnEnable()
		{
			InvokeRepeating("FireBullet", 0, 1 / bulletsPerSecond);
		}

		public void OnDisable()
		{
			CancelInvoke("FireBullet");
		}

		public void FireBullet()
		{
			float angle = lastAngle + (360f / bulletsPerTurn) + offsetAngle;
			angle = angle % 360;
			lastAngle = angle;
			Vector3 direction = Quaternion.AngleAxis(angle, Vector3.back * (reverseRotation ? -1 : 1)) * Vector3.up;

			GameObject bullet = ObjectPool.Get(bulletType);
			bullet.SetActive(true);
			bullet.transform.position = transform.position + (direction * transform.localScale.x / 2f * 1.2f);
			bullet.GetComponent<Rigidbody2D>().AddForce(direction * velocity);
		}
	}
}