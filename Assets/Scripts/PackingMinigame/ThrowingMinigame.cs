/*****************************************************************************
// File Name : ThrowingMinigame.cs
// Author : Brandon Koederitz
// Creation Date : 11/17/2025
// Last Modified : 11/17/2025
//
// Brief Description : Controls the throwing portion of the beginning minigame.
*****************************************************************************/
using System.Collections;
using UnityEngine;

namespace Snowmentum
{
    public class ThrowingMinigame : MinigameBase
    {
        #region CONSTS
        private const string THROW_ANIM_TRIGGER = "Throw";
        #endregion

        [Header("Throwing Settings")]
        [SerializeField, Tooltip("The amount of time to delay after the the throw starts.")] 
        private float throwDelay;
        [SerializeField] private float requiredThrowForce;

        private float totalThrowForce;
        private bool isSampling;
        private float sampleTimer;

        /// <summary>
        /// Instead of immediately starting the minigame, wait for a delay.
        /// </summary>

        public override void StartMinigame()
        {
            StartCoroutine(Timer());
        }

        protected override void MouseUpdate(Vector2 mouseDelta)
        {
            // First, check if the trackball is moving in the desired direction.
            // Only count rightwards input.
            if (mouseDelta.x > 0)
            {
                totalThrowForce += mouseDelta.x;
            }

            if (totalThrowForce > requiredThrowForce && !isSampling)
            {
                // Once we've started sampling, play the throw animation
                anim.SetTrigger(THROW_ANIM_TRIGGER);
                isSampling = true;
            }

            // Then, sample mouse delta values for a small amount of time to ensure we capture the fastest the 
            // player moved the ball.
            if (isSampling)
            {
                // Cheating a bit here since I know that InputManager sends input updates on update only.
                sampleTimer += Time.deltaTime;

                if (sampleTimer >= sampleTime)
                {
                    CompleteMinigame(totalThrowForce);
                    //minigameController.CompleteMinigame(storedPackingQuality, totalThrowForce);
                }
            }
        }

        /// <summary>
        /// Delays the throw minigame.
        /// </summary>
        /// <returns></returns>
        protected override IEnumerator Timer()
        {
            yield return new WaitForSeconds(throwDelay);
            base.StartMinigame();
        }
    }
}
