/*****************************************************************************
// File Name : TimeManager.cs
// Author : Brandon Koederitz
// Creation Date : 11/3/2025
// Last Modified : 11/3/2025
//
// Brief Description : Controls the progression of the day/night cycle.
*****************************************************************************/
using System.Collections;
using UnityEngine;

namespace Snowmentum
{
    public class TimeManager : MonoBehaviour
    {
        [SerializeField] private CelestialBody[] celestialBodies;
        [SerializeField, Tooltip("The amount of time in seconds that a day full day/night cycle lasts.")] private float dayTime;

        private float time;
        private bool isTimeMoving;

        /// <summary>
        /// Starts the passage of time.
        /// </summary>
        public void StartTime()
        {
            isTimeMoving = true;
            StartCoroutine(TimeRoutine());
        }

        private IEnumerator TimeRoutine()
        {

        }
    }
}
