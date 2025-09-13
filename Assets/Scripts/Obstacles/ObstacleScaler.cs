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
        [SerializeField, Tooltip("The in-game size of this obstacle.  Used to determine how large this obstacle is " +
            "in relation to the snowball.")] 
        private float obstacleSize;
        [SerializeField, Tooltip("The ScriptableValue that holds the current size of the snowball.")] 
        private ScriptableValue snowballSize;

        private Vector3 baseSize;

        #region Component References
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
        private void UpdateSize(float snowballSize)
        {
            float sizeRatio = obstacleSize / snowballSize;
            // Scales the object based on the ratio of the obstacle and snowball sizes.
            transform.localScale = Vector3.one * sizeRatio;

            // Scale the obstacle's position based on the size ratio so that the perspective scales.  Two obstacles
            // get closer to each other as they are scaled down.

            // Going to use -5 as a magic number for now.  In the future replace this w/ a value that corresponds
            // to the player's X position.

            float playerXPos = -5;
            Vector2 pos = rb.position;
            // Scale the obstacle's 
            pos.x = (pos.x - playerXPos) / sizeRatio;
            rb.position = pos;
        }
    }
}
