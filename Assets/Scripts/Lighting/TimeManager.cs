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
using UnityEngine.Events;

namespace Snowmentum
{
    public class TimeManager : MonoBehaviour
    {
        [SerializeField, Tooltip("The amount of time in seconds that a day full day/night cycle lasts.")]
        private float cycleLength;
        [SerializeField] private CelestialBody[] celestialBodies;
        [SerializeField] private bool startOnAwake;
        [SerializeField] private float startingTime;
        [SerializeField] private UnityEvent OnNewDayEvent;
        [Header("Debug")]
        [SerializeField] private bool updateInEditor;
        [SerializeField, Range(0f, 1f)] private float debugTime;

        private float time;
        private bool isTimeMoving;

        private void OnValidate()
        {
            if (updateInEditor)
            {
                UpdateBodies(debugTime);
            }
        }

        /// <summary>
        /// Debug start the day/night cycle.
        /// </summary>
        private void Start()
        {
            if (startOnAwake)
            {
                StartTime();
            }
        }

        /// <summary>
        /// Starts the passage of time.
        /// </summary>
        public void StartTime()
        {
            isTimeMoving = true;
            time = startingTime;
            StartCoroutine(TimeRoutine());
        }

        /// <summary>
        /// Controls the flow of the day/night cycle and updates each celestial body.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        private IEnumerator TimeRoutine()
        {
            while (isTimeMoving)
            {
                time += Time.deltaTime;
                if (time > cycleLength)
                {
                    time -= cycleLength;
                    OnNewDayEvent?.Invoke();
                }
                
                UpdateBodies(time / cycleLength);

                yield return null;
            }
        }

        /// <summary>
        /// Updates the celestial bodies based on the current time.
        /// </summary>
        /// <param name="normalizedTime"></param>
        private void UpdateBodies(float normalizedTime)
        {
            // Update the celestial bodies.
            foreach (var body in celestialBodies)
            {
                body.TimeUpdate(normalizedTime);
            }
        }
    }

}
