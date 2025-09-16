/*****************************************************************************
// File Name : ObstacleScaler.cs
// Author : Brandon Koederitz
// Creation Date : 9/13/2025
// Last Modified : 9/13/2025
//
// Brief Description : Scales obstacle sizes based on the size of the snowball to give the illusion of the snowball
// getting larger.
*****************************************************************************/
using UnityEngine;

namespace Snowmentum
{
    public class ObstacleScaleMover : ObjectMover
    {
        [Header("Values")]
        [SerializeField, Tooltip("The in-game size of this obstacle.  Used to determine how large this obstacle is " +
    "in relation to the snowball.")]
        private float obstacleSize;

        [Header("SO References")]
        [SerializeField, Tooltip("The ScriptableValue that holds the current size of the snowball.")] 
        private ScriptableValue snowballSize;

        private Vector3 baseSize;
        private float oldSize = 1;

        #region Properties
        private Vector2 PivotPoint
        {
            get
            {
                // Magic Numbering in a pivot point for the obstacles right now.  Will replace with a dynamic value
                // that changes based on the player's X position later.
                return new Vector2(-5, 0);
            }
        }
        #endregion

        /// <summary>
        /// Subscribe/Unsubscribe to the snowball size's OnValueChanged event.
        /// </summary>
        private void Awake()
        {
            // Store our base size.
            oldSize = settings.ScaleOnSpawn ? 1 : 0;
            baseSize = transform.localScale;
            //snowballSize.OnValueChanged += UpdateSize;
        }
        private void OnDestroy()
        {
            //snowballSize.OnValueChanged -= UpdateSize;
        }

        /// <summary>
        /// Updates the scale of this obstacle based on the snowball's size.
        /// </summary>
        /// <param name="snowballSize">The current size of the snowball.</param>
        //private void UpdateSize(float snowballSize, float oldSize)
        //{
        //    // Dont allow any scale updating if the snowball is set to a size of 0.
        //    if (snowballSize == 0) { return; }

        //    float sizeRatio = obstacleSize / snowballSize;
        //    if (settings.ScalePerspective)
        //    {
        //        // Scale the obstacle's position based on the size ratio so that the perspective scales.  Two obstacles
        //        // get closer to each other as they are scaled down.
        //        // Debug.Log(oldSize);
        //        float oldSizeRatio = obstacleSize / oldSize;
        //        ScaleAround(PivotPoint, oldSizeRatio, sizeRatio);
        //    }
        //    else
        //    {
        //        // Scales the object based on the ratio of the obstacle and snowball sizes.
        //        ScaleObstacle(sizeRatio);
        //    }
        //}

        /// <summary>
        /// Continually update the size of our obstacle based on the size of the snowball.
        /// </summary>
        protected override Vector2 MoveUpdate(Vector2 targetPos)
        {
            
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

            // Dont allow any scale updating if the snowball is set to a size of 0.
            if(snowballSize.Value == 0) { return targetPos; }

            float sizeRatio = obstacleSize / snowballSize.Value;
            // Save the size ratio of this iteration so that changes in size can be tracked.
            if (settings.ScalePerspective)
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
            return base.MoveUpdate(targetPos);
        }

        /// <summary>
        /// Scales this obstacle based on it's base size.
        /// </summary>
        /// <param name="scale">The scale to set for this obstacle.</param>
        private void ScaleObstacle(float scale)
        {
            transform.localScale = baseSize * scale;
        }

        ///// <summary>
        ///// Scales this object's position on the hill based on it's size to give the illusion of proper scaling.
        ///// </summary>
        ///// <param name="newScale">The ratio of this obstacle's size to the snowball's size.</param>
        ///// <param name="oldScale">
        ///// The old size ratio, as caluclated by the value of the previous snowball size
        ///// </param>
        //private void ScaleObstaclePosition(float newScale, float oldScale)
        //{
        //    // Calculate the change in the ratio of the obstacles size vs the snowball size.  This value will be used
        //    // to determine how much to scale our position by.
        //    //float ratioChange = newScale / oldScale;
        //    //Debug.Log($"Old Ratio: {oldScale}.  New Ratio: {newScale}");
        //    //Debug.Log(oldSizeRatio);
        //    //// Calculate the percentage change in the snowball's size.
        //    //float oldSize = snowballSize - snowballSizeChange;
        //    //float percentChange = oldSize == 0 ? 0 : (snowballSizeChange / oldSize);
        //    //Debug.Log(percentChange);
        //    // Going to use -5 as a magic number for now.  In the future replace this w/ a value that corresponds
        //    // to the player's X position.
        //    //Vector2 pivotPoint = GetPivotPoint(rb.position, -5, settings.MoveAngle);
        //    Vector2 pivotPoint = new Vector2(-5, 0);

        //    // Calculate a new position to jump to based on if we need to get closer or further away from our target
        //    // position.
        //    //pos = pivotPoint + (-toPointVector * ratioChange);

        //    ScaleAround(pivotPoint, oldScale, newScale);

        //    // Return the player's position to be relative to 0,0
        //    //pos.x += playerXPos;
        //    //rb.MovePosition(pos);
        //}

        /// <summary>
        /// Calculates the scale pivot point of a given obstacle along the player's line of movement
        /// </summary>
        /// <param name="currentPosition">The current position of the obstacle.</param>
        /// <param name="playerXPosition">
        /// The X position of the player.  Used to calculate the pivot point along this obstacle's path of movement.
        /// </param>
        /// <param name="angle">The angle that the obstacle is moving at.</param>
        /// <returns>The pivot point of the obstacle.</returns>
        private static Vector2 GetPivotPoint(Vector2 currentPosition, float playerXPosition, float angle)
        {
            // Offsets the obstacle's position to use the player's X position as it's pivot point.
            currentPosition.x -= playerXPosition;

            // Get the vector that points from the obstacle's current position to the point they'll cross the player's
            // X position.

            // Calculate the inner angle of the triangle used to find the point vector.
            float theta = Mathf.Deg2Rad * (180 - angle);
            Vector2 toPointVector = new Vector2(-currentPosition.x, currentPosition.x * Mathf.Tan(theta));
            
            return currentPosition + toPointVector;
        }

        /// <summary>
        /// Scales an object around a given pivot point using a Rigidbody.
        /// </summary>
        /// <param name="pivot">The pivot point to scale the obstacle around.</param>
        /// <param name="oldScale">The old scale of the obstacle</param>
        /// <param name="newScale">The new scale of the obstacle.</param>
        //private void ScaleAround(Vector2 pivot, float oldScale, float newScale)
        //{
        //    if (newScale == Mathf.Infinity || oldScale == Mathf.Infinity || oldScale == 0)
        //    {
        //        // If either scale invalid, then we should skip any scaling because values of infinity or 0 will
        //        // make the math break.
        //        return;
        //    }

        //    // Gets the current position of the obstacle.s
        //    Vector2 currentPos = rb.position;
        //    // Calculates the vector pointing from the pivot point to the current position.
        //    Vector2 pivotToCurrent = currentPos - pivot;
        //    // Calculates the change in scale between the old and new scales.  Used to determine how much to scale
        //    // the position by.
        //    float scaleFactor = newScale / oldScale;
        //    //Calculates the final position of the obstacle by scaling our pivotToCurrent vector by our scaleFactor.
        //    Vector3 finalPosition = pivot + (pivotToCurrent * scaleFactor);
        //    // Apply the scaling and translation.
        //    ScaleObstacle(newScale);
        //    //Debug.Log("Obstacle scaler happened for " + name);
        //    // Have to use .position instead of movePosition
        //    rb.position = finalPosition;
        //}
    }
}
