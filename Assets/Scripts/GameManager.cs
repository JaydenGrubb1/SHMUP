using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SHMUP.Util;

namespace SHMUP
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager instance;

        public static Vector3 screenLower { get; private set; }
        public static Vector3 screenUpper { get; private set; }

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
            screenLower = Camera.main.ScreenToWorldPoint(Vector2.zero).SetZ(0);
            screenUpper = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height)).SetZ(0);
        }
    }
}