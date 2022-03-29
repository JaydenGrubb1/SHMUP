using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SHMUP.Util;

namespace SHMUP
{
    public class PlayerUI : MonoBehaviour
    {
        public Image boostBackground;
        public Image boostBar;
        public Image boostFlash;

        [SerializeField]
        private Transform healthBarPos;
        [SerializeField]
        private Image healthBar;

        [HideInInspector]
        public PlayerController player;

        public void Update()
        {
            if(player == null)
                return;

            boostBar.fillAmount = player.BoostRemaining / player.BoostDuration;
            boostFlash.color = boostFlash.color.SetA(player.BoostDrained ? 1 : 0);
            if (player.LivesRemaining == 4)
                healthBarPos.gameObject.SetActive(false);
            else
            {
                healthBarPos.gameObject.SetActive(true);
                healthBar.fillAmount = player.LivesRemaining / 4f;
            }
        }

		public void CustomUpdate()
        {
            healthBarPos.position = RectTransformUtility.WorldToScreenPoint(GameManager.MainCamera, player.mainSprite.transform.position);
            healthBarPos.rotation = player.mainSprite.transform.rotation;
        }
	}
}