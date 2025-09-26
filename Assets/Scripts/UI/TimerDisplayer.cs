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

namespace Snowmentum
{
    public class TimerDisplayer : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI timerText;
        private float elapsedTime;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Update()
        {
            elapsedTime += Time.deltaTime;
            int minutes = Mathf.FloorToInt(elapsedTime / 60);
            int seconds = Mathf.FloorToInt(elapsedTime % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }

    }
}
