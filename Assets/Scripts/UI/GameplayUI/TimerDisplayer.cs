/*****************************************************************************
// File Name : TimerDisplayer.cs
// Author : Jack Fisher
// Creation Date : September 24, 2025
// Last Modified : September 24, 2025
//
// Brief Description : This script displays a timer in the UI of the screen. 
*****************************************************************************/
using UnityEngine;
using TMPro;
using System.Collections;

namespace Snowmentum
{
    public class TimerDisplayer : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI timerText;
        private float elapsedTime;

        private bool isTimer;

        /// <summary>
        /// Starts the timer.
        /// </summary>
        public void StartStopwatch()
        {
            if (!isTimer)
            {
                isTimer = true;
                StartCoroutine(TimerRoutine());
            }
        }

        /// <summary>
        /// Stops the timer.
        /// </summary>
        public void StopStopwatch()
        {
            isTimer = false;
        }

        /// <summary>
        /// Continually upticks the timer
        /// </summary>
        /// <returns></returns>
        private IEnumerator TimerRoutine()
        {
            while(isTimer)
            {
                elapsedTime += Time.deltaTime;
                int minutes = Mathf.FloorToInt(elapsedTime / 60);
                int seconds = Mathf.FloorToInt(elapsedTime % 60);
                timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
                yield return null;
            }
        }

    }
}
