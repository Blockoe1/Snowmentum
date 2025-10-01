/*****************************************************************************
// File Name : Obstacle.cs
// Author : Brandon Koederitz
// Creation Date : 10/1/2025
// Last Modified : 10/1/2025
//
// Brief Description : Controls storing data for a specific obstacle.
*****************************************************************************/
using Snowmentum.Score;
using Snowmentum.Size;
using UnityEngine;

namespace Snowmentum
{
    public class ObstacleController : MonoBehaviour
    {
        [SerializeField] private Obstacle obstacleData;
        [SerializeField] private bool autoUpdateHitbox;

        #region Component References    
        [Header("Components")]
        [SerializeReference] protected SpriteRenderer rend;
        [SerializeField] private CapsuleCollider2D obstacleCollider;
        [SerializeField] private ScoreIncrementer score;
        [SerializeField] private ObjectScaler scaler;

        /// <summary>
        /// Get components on reset.
        /// </summary>
        [ContextMenu("Get Component References")]
        private void Reset()
        {
            rend = GetComponent<SpriteRenderer>();
            obstacleCollider = GetComponent<CapsuleCollider2D>();
            score = GetComponent<ScoreIncrementer>();
            scaler = GetComponent<ObjectScaler>();
        }
        #endregion

        #region Properties
        public float ObstacleSize => obstacleData.ObstacleSize;
        #endregion

        /// <summary>
        /// Automatically obstacle values.
        /// </summary>
        private void OnValidate()
        {
            // Automatically updates the hitbox data of the obstacle.
            if (autoUpdateHitbox && obstacleData != null)
            {
                obstacleData.HitboxOffset = obstacleCollider.offset;
                obstacleData.HitboxSize = obstacleCollider.size;
            }
        }

        /// <summary>
        /// Sets the obstacle 
        /// </summary>
        /// <param name="obstacleData">The obstacle ScriptableObject that this obstacle should be based on.</param>
        public void SetObstacle(Obstacle obstacleData)
        {
            if (obstacleData == null) { return; }
            this.obstacleData = obstacleData;

            // Update hte component on this GameObject when obstacle data changes/
            rend.sprite = obstacleData.ObstacleSprite;
            score.BaseScore = obstacleData.BaseScore;
            scaler.Size = obstacleData.BaseSize;
            obstacleCollider.offset = obstacleData.HitboxOffset;
            obstacleCollider.size = obstacleData.HitboxSize;
        }
    }
}
