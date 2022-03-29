using UnityEngine;
using UnityEngine.Events;

namespace SHMUP.Events
{
    public class OnTriggerStay2DEvent : MonoBehaviour
    {
        public UnityEvent<Collider2D> onTriggerStay2D = new UnityEvent<Collider2D>();

		void OnTriggerStay2D(Collider2D collision)
        {
            if (onTriggerStay2D == null)
                return;

            onTriggerStay2D.Invoke(collision);
        }
    }
}