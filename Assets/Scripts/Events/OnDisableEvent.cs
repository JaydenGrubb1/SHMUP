using UnityEngine;
using UnityEngine.Events;

namespace SHMUP.Events
{
	public class OnDisableEvent : MonoBehaviour
	{
        public UnityEvent onDisable = new UnityEvent();

        void OnDisable()
        {
            if (onDisable == null)
                return;

            onDisable.Invoke();
        }
    }
}