/*****************************************************************************
// File Name : ObjectScaler.cs
// Author : Brandon Koederitz
// Creation Date : 9/13/2025
// Last Modified : 9/16/2025
//
// Brief Description : Scales the object according to the size of the snowball.
*****************************************************************************/
using UnityEngine;

namespace Snowmentum.Size
{
    [RequireComponent(typeof(ObjectMover))]
    public class ObjectScaler : MonoBehaviour, IMovementModifier
    {
        [SerializeField, Tooltip("The in-game size of this obstacle.  Used to determine how large this obstacle is " +
    "in relation to the snowball.")]
        private float obstacleSize;
        [SerializeField, Tooltip("If true, then the obstacle's position in the hill will automatically be adjusted" +
    " to add to the illusion of the snowball getting bigger.")]
        private bool scalePerspective;
        [SerializeField, Tooltip("When set to true, the object will scale it's position based on it's starting size" +
    " when it spawns.  This results in obstacles spawning at varied locations based on their size.")]
        private bool scaleOnSpawn;
        [SerializeField] private bool debugDisable;

        private float oldSize = 1;

        #region Properties
        private Vector2 PivotPoint
        {
            get
            {
                // Magic Numbering in a pivot point for the obstacles right now.  Will replace with a dynamic value
                // that changes based on the player's X position later.
                return new Vector2(SnowballSize.ScalePivotX, 0);
            }
        }

        public float Size => obstacleSize;
        #endregion

        /// <summary>
        /// Subscribe/Unsubscribe to the snowball size's OnValueChanged event.
        /// </summary>
        private void Awake()
        {
            // Store our base size.
            oldSize = scaleOnSpawn ? 1 : 0;
        }

        /// <summary>
        /// Update the scale of the obstacle in the scene.
        /// </summary>
        //private void OnValidate()
        //{
        //    ScaleObstacle(obstacleSize);
        //}

        /// <summary>
        /// Continually update the size of our obstacle based on the size of the snowball.
        /// </summary>
        public Vector2 MoveUpdate(Vector2 targetPos, Vector2 moveVector)
        {
            if (debugDisable) { return targetPos; }
            // Scales the obstacle around a given pivot point.
            void ScaleAround(Vector2 pivot, float oldScale, float newScale)
            {
                if (newScale == Mathf.Infinity || oldScale == Mathf.Infinity || oldScale == 0)
                {
                    // If either scale invalid, then we should skip any scaling because values of infinity or 0 will
                    // make the math break.
                    return;
                }

                // Calculates the vector pointing from the pivot point to the current position.
                Vector2 pivotToCurrent = targetPos - pivot;
                // Calculates the change in scale between the old and new scales.  Used to determine how much to scale
                // the position by.
                float scaleFactor = newScale / oldScale;
                //Calculates the final position of the obstacle by scaling our pivotToCurrent vector by our scaleFactor.
                Vector3 finalPosition = pivot + (pivotToCurrent * scaleFactor);
                // Apply the scaling and translation.
                ScaleObstacle(newScale);
                // update our target position to account for changes in scale.
                targetPos = finalPosition;
            }

            // Dont allow any scale updating if the snowball has a 0 or negative size.
            if(SnowballSize.Value <= 0) { return targetPos; }

            float sizeRatio = obstacleSize / SnowballSize.Value;
            // Save the size ratio of this iteration so that changes in size can be tracked.
            if (scalePerspective)
            {
                // Scale the obstacle's position based on the size ratio so that the perspective scales.  Two obstacles
                // get closer to each other as they are scaled down.
                ScaleAround(PivotPoint, oldSize, sizeRatio);
            }
            else
            {
                // Scales the object based on the ratio of the obstacle and snowball sizes.
                ScaleObstacle(sizeRatio);
            }

            // Store the size ratio used this update so that we can reference it next update.
            oldSize = sizeRatio;
            return targetPos;
        }

        /// <summary>
        /// Scales this obstacle based on it's base size.
        /// </summary>
        /// <param name="scale">The scale to set for this obstacle.</param>
        private void ScaleObstacle(float scale)
        {
            transform.localScale = Vector3.one * scale;
        }
    }
}
