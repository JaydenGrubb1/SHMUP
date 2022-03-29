using UnityEngine;
using UnityEngine.Events;

namespace SHMUP.Events
{
	public class OnEnableEvent : MonoBehaviour
	{
        public UnityEvent onEnable = new UnityEvent();

        void OnEnable()
        {
            if (onEnable == null)
                return;

            onEnable.Invoke();
        }
    }
}