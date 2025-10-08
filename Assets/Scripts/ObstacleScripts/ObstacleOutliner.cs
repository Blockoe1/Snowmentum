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
        [SerializeField, Tooltip("The maximum alpha value that the outline can have.")] 
        private float maxAlpha;
        [SerializeField, Tooltip("The maximum distance away that the outline will be visible at.")] 
        private float maxDistance;

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

        /// <summary>
        /// Subscribe/unsubscribe to functions.
        /// </summary>
        private void Awake()
        {
            SnowballPosition.OnPositionChanged += UpdateOutline;
        }
        private void OnDestroy()
        {
            SnowballPosition.OnPositionChanged -= UpdateOutline;
        }

        /// <summary>
        /// Updates the outline based on this object's distance from the snowball.
        /// </summary>
        /// <param name="snowballPos">The position of the snwoball.</param>
        private void UpdateOutline(Vector2 snowballPos)
        {
            Color col;
            // Update the color based on the size difference between this obstacle and the snowball.
            if (SnowballSize.Value > controller.ObstacleSize)
            {
                col = destroyableColor;
            }
            else
            {
                col = deadlyColor;
            }


            // Update the alpha value based on the distance from the snowball and obstacle.
            float distance = Vector2.Distance(snowballPos, transform.position);
            // Calculate the normalized distance value based on the max distance that is used to LERP between the
            // alpha values.
            float normalizedDistance = distance / maxDistance;
            // Lerp between the max alpha and 0.  maxAlpha should be reached at distance 0.
            float alpha  = Mathf.Lerp(maxAlpha, 0, normalizedDistance);

            col.a = alpha;

            rend.material.color = col;
        }
    }
}
