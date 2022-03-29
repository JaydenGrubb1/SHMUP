using UnityEngine;
using UnityEngine.Events;

namespace SHMUP.Events
{
	public class OnTriggerEnter2DEvent : MonoBehaviour
    {
        public UnityEvent<Collider2D> onTriggerEnter2D = new UnityEvent<Collider2D>();

		void OnTriggerEnter2D(Collider2D collision)
        {
            if (onTriggerEnter2D == null)
                return;

            onTriggerEnter2D.Invoke(collision);
        }
    }
}