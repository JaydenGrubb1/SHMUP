using UnityEngine;
using UnityEngine.Events;

namespace SHMUP.Events
{
    public class OnCollisionExit2DEvent : MonoBehaviour
    {
        public UnityEvent<Collision2D> onCollisionExit2D = new UnityEvent<Collision2D>();

        void OnCollisionExit2D(Collision2D collision)
        {
            if (onCollisionExit2D == null)
                return;

            onCollisionExit2D.Invoke(collision);
        }
    }
}