using UnityEngine;
using UnityEngine.Events;

namespace SHMUP.Events
{
	public class OnCollisionEnter2DEvent : MonoBehaviour
    {
        public UnityEvent<Collision2D> onCollisionEnter2D = new UnityEvent<Collision2D>();

		void OnCollisionEnter2D(Collision2D collision)
        {
            if (onCollisionEnter2D == null)
                return;

            onCollisionEnter2D.Invoke(collision);
        }
    }
}