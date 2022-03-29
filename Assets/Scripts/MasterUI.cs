using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SHMUP
{
	public class MasterUI : MonoBehaviour
	{
		[SerializeField]
		private Image progressBar;
		[SerializeField]
		private Text progressTime;
		[SerializeField]
		private Text totalTime;

		public void Update()
		{
			progressBar.fillAmount = LevelManager.MusicProgress/LevelManager.MusicLength;
			progressTime.text = FormatTime(LevelManager.MusicProgress);
			totalTime.text = FormatTime(LevelManager.MusicLength);
		}

		private string FormatTime(float time)
		{
			int minute = (int)Mathf.Round(time / 60);
			int second = (int)Mathf.Round(time % 60);

			return minute + ":" + second.ToString("00");
		}
	}
}