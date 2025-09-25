/*****************************************************************************
// File Name : PackingMinigame.cs
// Author : Brandon Koederitz
// Creation Date : 9/24/2025
// Last Modified : 9/24/2025
//
// Brief Description : Controls the progression of the packing minigame.
*****************************************************************************/
using System.Collections;
using UnityEngine;

namespace Snowmentum
{
    public class PackingMinigame : MonoBehaviour
    {
        [SerializeField] private float packingTime;

        private MinigameState minigameState;

        #region States
        private abstract class MinigameState
        {
            internal abstract void OnTimeUpdate(PackingMinigame minigameController, float deltaTime);
            internal abstract void OnMouseInput(PackingMinigame minigameController, Vector2 mouseDelta);
        }

        private class PackingState : MinigameState
        {
            private Vector2 lastDelta;
            private float packingQuality;
            private float timer;

            private bool hasStarted;

            internal PackingState(float timer)
            {
                this.timer = timer;
            }

            /// <summary>
            /// When the player inputs with the trackball, track it and progress the minigame
            /// </summary>
            /// <param name="mouseDelta"></param>
            internal override void OnMouseInput(PackingMinigame minigameController, Vector2 mouseDelta)
            {
                // Increase the quality of the snowball based on how much the mouse delta changed since the
                // last update.
                packingQuality += Vector2.Distance(lastDelta, mouseDelta);

                lastDelta = mouseDelta;

                // Only allow the timer to update once the player has done at least one input.
                hasStarted |= true;

            }

            /// <summary>
            /// Limits the player's time during the packing stage of the minigame once the player starts inputting
            /// with the mouse.
            /// </summary>
            /// <param name="deltaTime"></param>
            internal override void OnTimeUpdate(PackingMinigame minigameController, float deltaTime)
            {
                if (hasStarted)
                {
                    timer -= Time.deltaTime;
                    // Once the packing time is up, we move to the throw portion of the minigame.
                    if (timer <= 0)
                    {
                        minigameController.minigameState = new ThrowState();
                    }
                }
            }
        }

        private class ThrowState : MinigameState
        {
            internal override void OnMouseInput(PackingMinigame minigameController, Vector2 mouseDelta)
            {
                throw new System.NotImplementedException();
            }

            internal override void OnTimeUpdate(PackingMinigame minigameController, float deltaTime)
            {
                throw new System.NotImplementedException();
            }
        }
        #endregion

        /// <summary>
        /// When the game begins, we begin with the packing state
        /// </summary>
        private void Awake()
        {
            minigameState = new PackingState(packingTime);
        }

        /// <summary>
        /// Continually calls time update o the current minigame state.
        /// </summary>
        /// <returns></returns>
        private IEnumerator TimeUpdateRoutine()
        {
            while (true)
            {
                minigameState.OnTimeUpdate(this, Time.deltaTime);
                yield return null;
            }
        }
    }
}
