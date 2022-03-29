using UnityEngine;
using UnityEngine.Events;

namespace SHMUP.Events
{
	public class StartEvent : MonoBehaviour
    {
        public UnityEvent start = new UnityEvent();

        void Start()
        {
            if (start == null)
                return;

            start.Invoke();
        }
    }
}