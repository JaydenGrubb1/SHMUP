using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SHMUP.Util;

namespace SHMUP
{
    public class PlayerUI : MonoBehaviour
    {
        [SerializeField]
        private Image boostBar;
        [SerializeField]
        private Image boostFlash;

        [HideInInspector]
        public PlayerController player;

        public void Update()
        {
            boostBar.fillAmount = player.BoostRemaining / player.BoostDuration;
            boostFlash.color = boostFlash.color.SetA(player.BoostDrained ? 1 : 0);
        }
    }
}