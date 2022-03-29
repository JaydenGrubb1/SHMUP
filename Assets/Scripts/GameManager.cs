using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using SHMUP.Util;

namespace SHMUP
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager instance;

        [SerializeField]
        private Camera mainCamera;

        public static Camera MainCamera { get { return instance.mainCamera; } }

        public static Vector3 ScreenLower { get; private set; }
        public static Vector3 ScreenUpper { get; private set; }

        public void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(this);
            }
        }

		public void Start()
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = false;
            ScreenLower = MainCamera.ScreenToWorldPoint(Vector2.zero).SetZ(0);
            ScreenUpper = MainCamera.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height)).SetZ(0);
        }

  //      public void Exit()
		//{
  //          Quit();
		//}

        public static void Quit()
        {
            Application.Quit();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
    }
}