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
using System;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Snowmentum
{
    public class ObstacleController : MonoBehaviour
    {
        [SerializeField] private Obstacle obstacleData;

        private ObstacleReturnFunction obstacleReturnFunction;

#if UNITY_EDITOR
        [SerializeField, HideInInspector] private Obstacle oldObsData;
#endif

        #region Component References    
        [Header("Components")]
        [SerializeReference] protected SpriteRenderer rend;
        [SerializeField] private CapsuleCollider2D obstacleCollider;
        [SerializeField] private ScoreIncrementer score;
        [SerializeField] private ObjectScaler scaler;
        [SerializeField] private AudioRelay relay;
        [SerializeField] private ObstacleOutliner outliner;

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
            relay = GetComponent<AudioRelay>();
            outliner = GetComponent<ObstacleOutliner>();
        }
        #endregion

        #region Properties
        public ObstacleReturnFunction ReturnFunction
        {
            set { obstacleReturnFunction = value; }
        }
        public float ObstacleSize => obstacleData.ObstacleSize;
        #endregion


#if UNITY_EDITOR
        /// <summary>
        /// Automatically obstacle data values.
        /// </summary>
        private void OnValidate()
        {
            //// Automatically updates the hitbox data of the obstacle.
            //if (autoUpdate && obstacleData != null)
            //{
            //    obstacleData.IsTrigger = obstacleCollider.isTrigger;
            //    obstacleData.HitboxOffset = obstacleCollider.offset;
            //    obstacleData.HitboxSize = obstacleCollider.size;
            //    obstacleData.HitboxDirection = obstacleCollider.direction;
            //}

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

            ReadObstacleData();
        }

        #region ObstacleData Manipulation
#if UNITY_EDITOR
        /// <summary>
        /// Updates the data object that controls this obstacle.
        /// </summary>
        [ContextMenu("Write Obstacle Data")]
        private void WriteObstacleData()
        {
            if (obstacleData == null) { return; }

            // Update the misc data of the obstacle.
            obstacleData.Tag = gameObject.tag;
            if (rend != null)
            {
                obstacleData.ObstacleSprite = rend.sprite;
                obstacleData.OrderInLayer = rend.sortingOrder;
            }
            if (score != null)
            {
                obstacleData.BaseScore = score.BaseScore;
            }
            if (relay != null)
            {
                obstacleData.DestroySound = relay.SoundName;
            }
            if (outliner != null)
            {
                obstacleData.ShowOutline = outliner.ShowOutline;
            }

            // Update the collision data of the obstacle.
            if (obstacleCollider != null)
            {
                obstacleData.IsTrigger = obstacleCollider.isTrigger;
                obstacleData.HitboxOffset = obstacleCollider.offset;
                obstacleData.HitboxSize = obstacleCollider.size;
                obstacleData.HitboxDirection = obstacleCollider.direction;
            }

            // Save changes to the asset.
            EditorUtility.SetDirty(obstacleData);
            AssetDatabase.SaveAssetIfDirty(obstacleData);
        }
#endif

        /// <summary>
        /// Reads the data from this object's obstacleData and updates the components on this GameObject.
        /// </summary>
        private void ReadObstacleData()
        {
            if (obstacleData == null) { return; }

            // Update the component on this GameObject when obstacle data changes.
            gameObject.tag = obstacleData.Tag;
            if (rend != null)
            {
                rend.sprite = obstacleData.ObstacleSprite;
                rend.sortingOrder = obstacleData.OrderInLayer;
            }
            if (score != null)
            {
                score.BaseScore = obstacleData.BaseScore;
            }
            if ( scaler != null)
            {
                scaler.Size = obstacleData.BaseSize;
            }
            if (relay != null)
            {
                relay.SoundName = obstacleData.DestroySound;
            }
            if (outliner != null)
            {
                outliner.ShowOutline = obstacleData.ShowOutline;
            }

            // Collider Updates
            if (obstacleCollider != null)
            {
                obstacleCollider.isTrigger = obstacleData.IsTrigger;
                obstacleCollider.offset = obstacleData.HitboxOffset;
                obstacleCollider.size = obstacleData.HitboxSize;
                obstacleCollider.direction = obstacleData.HitboxDirection;
            }
        }
        #endregion

        /// <summary>
        /// Return this obstacle to the obstacle spawner's pool.
        /// </summary>
        public void ReturnObstacle()
        {
            obstacleReturnFunction(this);
        }
    }
}
