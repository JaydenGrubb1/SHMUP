using UnityEngine;
using UnityEngine.Events;

namespace SHMUP.Events
{
    public class OnCollisionStay2DEvent : MonoBehaviour
    {
        public UnityEvent<Collision2D> onCollisionStay2D = new UnityEvent<Collision2D>();

        void OnCollisionStay2D(Collision2D collision)
        {
            if (onCollisionStay2D == null)
                return;

            onCollisionStay2D.Invoke(collision);
        }
    }
}