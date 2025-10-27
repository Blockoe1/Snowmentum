/*****************************************************************************
// File Name : ObstacleOutliner.cs
// Author :
// Creation Date : 
// Last Modified : 
//
// Brief Description : 
*****************************************************************************/
using Snowmentum.Size;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

namespace Snowmentum
{
    [RequireComponent(typeof(ObstacleController))]
    public class ObstacleOutliner : MonoBehaviour
    {
        [SerializeField] private Color deadlyColor = Color.red;
        [SerializeField] private Color destroyableColor = Color.green;
        [SerializeField, Tooltip("The maximum alpha value that the outline can have."), Range(0, 1)] 
        private float maxOutlineAlpha;
        [SerializeField, Tooltip("The maximum amount the object's sprite gets tinted."), Range(0, 1)]
        private float maxTintValue;
        [SerializeField, Tooltip("The maximum distance away that the outline will be visible at.")] 
        private float maxDistance;
        [SerializeField] private float outlineBlinkDelay = 0.1f;
        [SerializeField] private bool showOutline;

        private Color baseColor;
        private bool isBlink;

        #region Component References
        [Header("Components")]
        [SerializeReference] private SpriteRenderer rend;
        [SerializeReference] private ObstacleController controller;

        /// <summary>
        /// Get components on reset.
        /// </summary>
        [ContextMenu("Get Component References")]
        private void Reset()
        {
            rend = GetComponent<SpriteRenderer>();
            controller = GetComponent<ObstacleController>();
        }
        #endregion

        #region Properties
        public bool ShowOutline
        {
            get { return showOutline; }
            set 
            {
                showOutline = value;
            }
        }
        #endregion

        /// <summary>
        /// Subscribe/unsubscribe to functions.
        /// </summary>
        private void Awake()
        {
            baseColor = rend.color;
            if (showOutline)
            {
                SnowballPosition.OnPositionChanged += UpdateOutline;
            }
            else
            {
                ResetColor();
            }
        }
        private void OnDestroy()
        {
            SnowballPosition.OnPositionChanged -= UpdateOutline;
        }

        /// <summary>
        /// Toggles the outline with any required event subscriptions and updates.
        /// </summary>
        /// <param name="value"></param>
        public void ToggleOutline(bool value)
        {
            // Prevents redundant assignment.
            if (value == showOutline) { return; }
            showOutline = value;

            // Disable the outline if ShowOutline is set to false.
            if (showOutline)
            {
                SnowballPosition.OnPositionChanged += UpdateOutline;
            }
            else
            {
                SnowballPosition.OnPositionChanged -= UpdateOutline;
                ResetColor();
            }
        }

        /// <summary>
        /// Gets the correct outline color based on the size of the snowball compared to the obstacle.
        /// </summary>
        /// <returns></returns>
        private Color GetOutlineColor()
        {
            // Update the color based on the size difference between this obstacle and the snowball.
            if (SnowballSize.Value > controller.ObstacleSize || SnowballFreezing.IsFrozen)
            {
                return destroyableColor;
            }
            else
            {
                return deadlyColor;
            }
        }

        /// <summary>
        /// Updates the outline based on this object's distance from the snowball.
        /// </summary>
        /// <param name="snowballPos">The position of the snwoball.</param>
        private void UpdateOutline(Vector2 snowballPos)
        {
            // If ShowOutline is disabled, then we should never update our outline.
            if (!showOutline) { return; }
            // Update the alpha value based on the distance from the snowball and obstacle.
            float distance = Vector2.Distance(snowballPos, transform.position);
            // Calculate the normalized distance value based on the max distance that is used to LERP between the
            // alpha values.
            float normalizedDistance = distance / maxDistance;
            // Lerp between the max alpha and 0.  maxAlpha should be reached at distance 0.
            float alpha  = Mathf.Lerp(maxOutlineAlpha, 0, normalizedDistance);

            // Tint the object's sprite
            float tintStrength = Mathf.Lerp(maxTintValue, 0, normalizedDistance);
            Color tintColor = Color.Lerp(baseColor, GetOutlineColor(), tintStrength);

            // Apply the color changes.
            rend.color = tintColor;
            SetOutlineAlpha(alpha);
        }

        /// <summary>
        /// Sets the current alpha and color of the outline.
        /// </summary>
        /// <param name="alpha"></param>
        private void SetOutlineAlpha(float alpha)
        {
            Color col = GetOutlineColor();
            col.a = alpha;
            rend.material.color = col;
        }

        /// <summary>
        /// Resets this obstacle's colors back to 0.
        /// </summary>
        private void ResetColor()
        {
            // Set the outline to completely transparent on awake.
            rend.material.color = Color.clear;
            rend.color = baseColor;
        }

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
                ToggleOutline(false);
                SetOutlineAlpha(1);
                yield return new WaitForSeconds(outlineBlinkDelay);

                ToggleOutline(true);
                yield return new WaitForSeconds(outlineBlinkDelay);
            }
        }
    }
}
