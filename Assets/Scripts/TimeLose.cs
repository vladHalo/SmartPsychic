using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class TimeLose : MonoBehaviour
    {
        public Text timer;
        public Image mask;

        public float timeStart;
        public float timeChange;
        
        public bool Timer()
        {
            timer.text = Math.Round(timeChange, 1).ToString();
            if (timeChange < 0)
            {
                timer.text = "0,0";
                return false;
            }
            timeChange -= Time.deltaTime;
            mask.fillAmount = timeChange / timeStart;
            return true;
        }
    }
}