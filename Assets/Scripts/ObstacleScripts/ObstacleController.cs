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
        [SerializeField] private bool autoUpdate;

#if UNITY_EDITOR
        [SerializeField, HideInInspector] private Obstacle oldObsData;
#endif

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


#if UNITY_EDITOR
        /// <summary>
        /// Automatically obstacle data values.
        /// </summary>
        private void OnValidate()
        {
            // Automatically updates the hitbox data of the obstacle.
            if (autoUpdate && obstacleData != null)
            {
                obstacleData.HitboxOffset = obstacleCollider.offset;
                obstacleData.HitboxSize = obstacleCollider.size;
                obstacleData.HitboxDirection = obstacleCollider.direction;
            }

            // Update our prefab's components when the obstacle data changes.
            if (obstacleData != oldObsData)
            {
                Debug.Log("Updated");
                // Run SetObstacle so other values are updated.
                SetObstacle(obstacleData);
                oldObsData = obstacleData;
            }
        }
#endif

        /// <summary>
        /// Sets the obstacle 
        /// </summary>
        /// <param name="obstacleData">The obstacle ScriptableObject that this obstacle should be based on.</param>
        public void SetObstacle(Obstacle obstacleData)
        {
            if (obstacleData == null) { return; }
            this.obstacleData = obstacleData;

            // Update hte component on this GameObject when obstacle data changes/
            UpdateObstacleComponents(obstacleData);
            obstacleCollider.offset = obstacleData.HitboxOffset;
            obstacleCollider.size = obstacleData.HitboxSize;
            obstacleCollider.direction = obstacleData.HitboxDirection;
        }

        /// <summary>
        /// Updates all of the non-hitbox components of this obstacle.
        /// </summary>
        /// <param name="obstacleData"></param>
        public void UpdateObstacleComponents(Obstacle obstacleData)
        {
            rend.sprite = obstacleData.ObstacleSprite;
            score.BaseScore = obstacleData.BaseScore;
            scaler.Size = obstacleData.BaseSize;
        }

        /// <summary>
        /// Return this obstacle to the obstacle spawner's pool.
        /// </summary>
        public void ReturnObstacle()
        {
            ObstacleSpawner.ReturnObstacle(this);
        }
    }
}
