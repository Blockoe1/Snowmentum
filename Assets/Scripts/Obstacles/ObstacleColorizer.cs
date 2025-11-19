/*****************************************************************************
// File Name : ObstacleColorizer.cs
// Author : Brandon Koederitz
// Creation Date : 11/19/2025
// Last Modified : 11/19/2025
//
// Brief Description : Controls the color of the obstacle's lights, outline, color, etc.
*****************************************************************************/
using Snowmentum.Size;
using UnityEngine;
using UnityEngine.Events;

namespace Snowmentum
{
    public class ObstacleColorizer : MonoBehaviour
    {
        [SerializeField, ColorUsage(true, true)] private Color deadlyColor = Color.red;
        [SerializeField, ColorUsage(true, true)] private Color destroyableColor = Color.blue;
        [SerializeField, Tooltip("The maximum alpha value thatthis obstacle's colors can be."), Range(0, 1)]
        private float maxAlpha;
        [SerializeField, Tooltip("The maximum distance away that the outline will be visible at.")]
        private float maxDistance;
        [SerializeField] private bool showColors;
        [SerializeField] private UnityEvent<Color> OnColorUpdateEvent;

        #region Component References
        [Header("Components")]
        [SerializeReference] private ObstacleController controller;

        /// <summary>
        /// Get components on reset.
        /// </summary>
        [ContextMenu("Get Component References")]
        private void Reset()
        {
            controller = GetComponent<ObstacleController>();
        }
        #endregion

        #region Properties
        public bool ShowColors
        {
            get { return showColors; }
            set
            {
                showColors = value;
            }
        }
        #endregion

        /// <summary>
        /// When this object is enabled/disabled, make sure to set up it's subscriptions or reset the outline.
        /// </summary>
        private void OnEnable()
        {
            if (ShowColors)
            {
                SnowballPosition.OnPositionChanged += UpdateOutline;
            }
            else
            {
                ResetColor();
            }
        }
        private void OnDisable()
        {
            SnowballPosition.OnPositionChanged -= UpdateOutline;
        }

        /// <summary>
        /// Gets the correct color based on the distance from the snowball.
        /// </summary>
        /// <returns></returns>
        private Color GetColor()
        {
            // Update the color based on the size difference between this obstacle and the snowball.
            if (SnowballSize.Value > controller.ObstacleSize || SnowballFreezing.ShowVisuals)
            {
                return destroyableColor;
            }
            else
            {
                return deadlyColor;
            }
        }

        /// <summary>
        /// Updates the color based on the obstacle's distance from the snowball.
        /// </summary>
        /// <param name="snowballPos">The position of the snwoball.</param>
        private void UpdateOutline(Vector2 snowballPos)
        {
            // If ShowOutline is disabled, then we should never update our outline.
            if (!showColors) { return; }
            // Update the alpha value based on the distance from the snowball and obstacle.
            float distance = Vector2.Distance(snowballPos, transform.position);
            // Calculate the normalized distance value based on the max distance that is used to LERP between the
            // alpha values.
            float normalizedDistance = distance / maxDistance;
            // Lerp between the max alpha and 0.  maxAlpha should be reached at distance 0.
            float alpha = Mathf.Lerp(maxAlpha, 0, normalizedDistance);

            Color outputColor = GetColor();
            outputColor.a = alpha;

            OnColorUpdateEvent?.Invoke(outputColor);

            //// Tint the object's sprite
            //float tintStrength = Mathf.Lerp(maxTintValue, 0, normalizedDistance);
            //Color tintColor = Color.Lerp(baseColor, GetColor(), tintStrength);

            //// Apply the color changes.
            //rend.color = tintColor;
            //SetOutlineAlpha(alpha);
        }

        /// <summary>
        /// Resets this obstacle's colors back to clear.
        /// </summary>
        private void ResetColor()
        {
            OnColorUpdateEvent?.Invoke(Color.clear);
        }
    }
}
