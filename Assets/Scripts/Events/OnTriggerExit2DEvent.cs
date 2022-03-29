using UnityEngine;
using UnityEngine.Events;

namespace SHMUP.Events
{
    public class OnTriggerExit2DEvent : MonoBehaviour
    {
        public UnityEvent<Collider2D> onTriggerExit2D = new UnityEvent<Collider2D>();

		void OnTriggerExit2D(Collider2D collision)
        {
            if (onTriggerExit2D == null)
                return;

            onTriggerExit2D.Invoke(collision);
        }
    }
}