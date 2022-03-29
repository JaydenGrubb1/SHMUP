using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SHMUP
{
	public class LevelManager : MonoBehaviour
    {
        private static LevelManager instance;

        [SerializeField]
        private int levelNumber;
        [SerializeField]
        private float loadDelay = 3;

        private AudioSource levelTrack;

        public static float MusicProgress { get { return instance.levelTrack ? instance.levelTrack.time : 0; } }
        public static float MusicLength { get { return instance.levelTrack ? instance.levelTrack.clip.length : 0; } }

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
            Invoke("Test", loadDelay);
        }

        private void Test()
        {
            LoadLevel(1);
        }

        public static void LoadLevel(int number)
        {
			SceneManager.LoadSceneAsync(number, LoadSceneMode.Additive).completed += LoadComplete;
        }

		private static void LoadComplete(AsyncOperation obj)
		{
            foreach(GameObject go in GameObject.FindGameObjectsWithTag("PreviewOnly"))
			{
                go.SetActive(false);
			}

            instance.levelTrack = GameObject.FindGameObjectWithTag("LevelMusic").GetComponent<AudioSource>();
		}
	}
}