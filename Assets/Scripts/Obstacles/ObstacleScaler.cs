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
    public class ObstacleScaler : MonoBehaviour
    {
        [Header("Values")]
        [SerializeField, Tooltip("The in-game size of this obstacle.  Used to determine how large this obstacle is " +
    "in relation to the snowball.")]
        private float obstacleSize;

        [Header("SO References")]
        [SerializeField] ObstacleSettings settings;
        [SerializeField, Tooltip("The ScriptableValue that holds the current size of the snowball.")] 
        private ScriptableValue snowballSize;

        private Vector3 baseSize;

        #region Component References
        [Header("Components")]
        [SerializeReference] private Rigidbody2D rb;

        /// <summary>
        /// Get components on reset.
        /// </summary>
        [ContextMenu("Get Component References")]
        private void Reset()
        {
            rb = GetComponent<Rigidbody2D>();
        }
        #endregion

        /// <summary>
        /// Subscribe/Unsubscribe to the snowball size's OnValueChanged event.
        /// </summary>
        private void Awake()
        {
            Application.targetFrameRate = 0;
            // Store our base size.
            baseSize = transform.localScale;
            snowballSize.OnValueChanged += UpdateSize;
        }
        private void OnDestroy()
        {
            snowballSize.OnValueChanged -= UpdateSize;
        }

        /// <summary>
        /// Updates the scale of this obstacle based on the snowball's size.
        /// </summary>
        /// <param name="snowballSize">The current size of the snowball.</param>
        private void UpdateSize(float snowballSize, float oldSize)
        {
            // Dont allow any scale updating if the snowball is set to a size of 0.
            if (snowballSize == 0) { return; }
            float sizeRatio = obstacleSize / snowballSize;
            // Scales the object based on the ratio of the obstacle and snowball sizes.
            transform.localScale = Vector3.one * sizeRatio;

            // Scale the obstacle's position based on the size ratio so that the perspective scales.  Two obstacles
            // get closer to each other as they are scaled down.

            if (settings.ScalePerspective && oldSize != 0)
            {
                float oldSizeRatio = obstacleSize / oldSize;
                ScaleObstaclePosition(sizeRatio, oldSizeRatio);
            }
        }

        /// <summary>
        /// Scales this object's position on the hill based on it's size to give the illusion of proper scaling.
        /// </summary>
        /// <param name="sizeRatio">The ratio of this obstacle's size to the snowball's size.</param>
        /// <param name="oldSizeRatio">
        /// The old size ratio, as caluclated by the value of the previous snowball size
        /// </param>
        private void ScaleObstaclePosition(float sizeRatio, float oldSizeRatio)
        {
            if (sizeRatio == Mathf.Infinity || oldSizeRatio == Mathf.Infinity)
            {
                // If either ratio was invalid, then we should skip any scaling because values of infinity will
                // make the math break.
                return;
            }
            // Calculate the change in the ratio of the obstacles size vs the snowball size.  This value will be used
            // to determine how much to scale our position by.
            float ratioChange = sizeRatio / oldSizeRatio;
            Debug.Log($"Old Ratio: {oldSizeRatio}.  New Ratio: {sizeRatio}");
            //Debug.Log(oldSizeRatio);
            //// Calculate the percentage change in the snowball's size.
            //float oldSize = snowballSize - snowballSizeChange;
            //float percentChange = oldSize == 0 ? 0 : (snowballSizeChange / oldSize);
            //Debug.Log(percentChange);
            // Going to use -5 as a magic number for now.  In the future replace this w/ a value that corresponds
            // to the player's X position.
            float playerXPos = -5;
            Vector2 pos = rb.position;

            // Offsets the obstacle's position to use the player's X position as it's pivot point.
            pos.x -= playerXPos;

            // Get the vector that points from the obstacle's current position to the point they'll cross the player's
            // X position.

            // Calculate the inner angle of the triangle used to find the point vector.
            float theta = Mathf.Deg2Rad * (180 - settings.MoveAngle);
            Vector2 toPointVector = new Vector2(-pos.x, pos.x * Mathf.Tan(theta));
            Debug.DrawLine(rb.position, rb.position + toPointVector, Color.red);
            Vector2 targetPos = pos + toPointVector;

            // Calculate a new position to jump to based on if we need to get closer or further away from our target
            // position.
            pos = targetPos + (-toPointVector * ratioChange);

            // Return the player's position to be relative to 0,0
            pos.x += playerXPos;
            rb.MovePosition(pos);
        }
    }
}
