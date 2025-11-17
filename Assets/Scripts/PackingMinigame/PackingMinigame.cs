/*****************************************************************************
// File Name : PackingMinigame.cs
// Author : Brandon Koederitz
// Creation Date : 11/17/2025
// Last Modified : 11/17/2025
//
// Brief Description : Controls the packing stage of the beginning minigame.
*****************************************************************************/
using System.Collections;
using UnityEditor;
using UnityEngine;

namespace Snowmentum
{
    public class PackingMinigame : MinigameBase
    {
        #region CONSTS
        private const string DIRECTION_ANIM_INT = "PackingDirection";
        private const string VERTICAL_ANIM_BOOL = "IsVertical";
        #endregion

        private Vector2 lastDelta;
        private float packingQuality;
        private bool hasStarted;

        protected override void MouseUpdate(Vector2 mouseDelta)
        {
            // Skip any updates where our mouse didnt move.
            if (mouseDelta == Vector2.zero)
            {
                // Reset the animator.
                anim.SetInteger(DIRECTION_ANIM_INT, 0);
                anim.SetBool(VERTICAL_ANIM_BOOL, false);
                return;
            }

            // Update the animator.
            anim.SetBool(VERTICAL_ANIM_BOOL, mouseDelta.y > mouseDelta.x);
            anim.SetInteger(DIRECTION_ANIM_INT, mouseDelta.x > 0 ? 1 : -1); 

            // Increase the quality of the snowball based on how much the mouse delta changed since the
            // last update.
            packingQuality += Vector2.Distance(lastDelta, mouseDelta);
            lastDelta = mouseDelta;

            // Only allow the timer to update once the player has done at least one input.
            if (!hasStarted)
            {
                StartCoroutine(Timer());
                hasStarted = true;
            }
        }

        /// <summary>
        /// After the player starts inputting, they can only pack for a limited time.
        /// </summary>
        /// <returns></returns>
        protected override IEnumerator Timer()
        {
            float timer = sampleTime;
            while (timer > 0)
            {
                timer -= Time.deltaTime;
                yield return null;
            }
            CompleteMinigame(packingQuality);
        }
    }
}
