/*****************************************************************************
// File Name : ObstacleOutliner.cs
// Author : Brandon Koederitz
// Creation Date : 11/19/2025
//
// Brief Description : Updates the color of the obstacles outline.
*****************************************************************************/
using System.Collections;
using UnityEngine;

namespace Snowmentum
{
    [RequireComponent(typeof(ObstacleController))]
    public class ObstacleOutliner : MonoBehaviour
    {
        [SerializeField] private float outlineBlinkDelay = 0.1f;

        private bool blockColorUpdates;
        //private Color baseColor;

        #region Component References
        [Header("Components")]
        [SerializeReference] private SpriteRenderer rend;

        /// <summary>
        /// Get components on reset.
        /// </summary>
        [ContextMenu("Get Component References")]
        private void Reset()
        {
            rend = GetComponent<SpriteRenderer>();
        }
        #endregion

        ///// <summary>
        ///// Set the obstacle's base color.
        ///// </summary>
        //private void Awake()
        //{
        //    baseColor = rend.color;
        //}

        /// <summary>
        /// Sets the color of this obstacle's outline.
        /// </summary>
        /// <param name="color"></param>
        public void SetOutlineColor(Color color)
        {
            if (blockColorUpdates) { return; }
            rend.material.color = color;
        }

        /// <summary>
        /// Sets the current alpha and color of the outline.
        /// </summary>
        /// <param name="alpha"></param>
        private void SetOutlineAlpha(float alpha)
        {
            Color col = rend.material.color;
            col.a = alpha;
            rend.material.color = col;
        }

        ///// <summary>
        ///// Resets this obstacle's colors back to 0.
        ///// </summary>
        //private void ResetColor()
        //{
        //    // Set the outline to completely transparent on awake.
        //    rend.material.color = Color.clear;
        //    rend.color = baseColor;
        //}

        /// <summary>
        /// Causes the outline to blink and then show in full, overriding any changes caused by distance.
        /// </summary>
        public void BlinkOutline(int blinkAmount)
        {
            StartCoroutine(BlinkRoutine(blinkAmount));
        }

        private IEnumerator BlinkRoutine(int blinkAmount)
        {
            for (int i = 0; i < blinkAmount; i++)
            {
                // Temporarily disable the outline and set it manually.
                blockColorUpdates = true;
                SetOutlineAlpha(1);
                yield return new WaitForSeconds(outlineBlinkDelay);

                blockColorUpdates = false;
                yield return new WaitForSeconds(outlineBlinkDelay);
            }
        }
    }
}
