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
using System;
using System.Collections.Generic;
using static UnityEngine.UI.Image;





#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Snowmentum
{
    public class ObstacleController : MonoBehaviour
    {
        [SerializeField, ShowNestedEditor] private Obstacle obstacleData;

        [SerializeField, Space(20)] private bool autoUpdateObstacleData = true;

        private ObstacleReturnFunction obstacleReturnFunction;

#if UNITY_EDITOR
        [SerializeField, HideInInspector] private Obstacle oldObsData;
#endif

        #region Component References    
        [SerializeReference, ReadOnly] protected SpriteRenderer rend;
        [SerializeReference, ReadOnly] private CapsuleCollider2D obstacleCollider;
        [SerializeReference, ReadOnly] private ScoreIncrementer score;
        [SerializeReference, ReadOnly] private ObjectScaler scaler;
        [SerializeReference, ReadOnly] private AudioRelay relay;
        [SerializeReference, ReadOnly] private ObstacleOutliner outliner;
        [SerializeReference] private ParticleSystem particles;

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
            particles = GetComponentInChildren<ParticleSystem>();
        }
        #endregion

        #region Properties
        public bool AutoUpdateObstacleData => autoUpdateObstacleData;
        public Obstacle ObstacleData => obstacleData;
        public ObstacleReturnFunction ReturnFunction
        {
            set { obstacleReturnFunction = value; }
        }
        public virtual float ObstacleSize => obstacleData.ObstacleSize;
        public bool HasCollision => obstacleData == null ? false : obstacleData.HasCollision;
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
            if (autoUpdateObstacleData && obstacleData != oldObsData)
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
        public virtual void SetObstacle(Obstacle obstacleData)
        {
            if (obstacleData == null) { return; }
            this.obstacleData = obstacleData;

            // Alway toggle the obstacle on when new data is set.
            ToggleObstacle(true);

            ReadObstacleData();
        }

        /// <summary>
        /// Tobbles this obstacle's sprite and collision.
        /// </summary>
        /// <param name="isEnabled"></param>
        public void ToggleObstacle(bool isEnabled)
        {
            if (rend != null)
            {
                rend.enabled = isEnabled;
            }
            if (obstacleCollider != null)
            {
                obstacleCollider.enabled = isEnabled;
            }
        }

        #region ObstacleData Manipulation
#if UNITY_EDITOR
        /// <summary>
        /// Updates the data object that controls this obstacle.
        /// </summary>
        [ContextMenu("Write Obstacle Data")]
        public void WriteObstacleData()
        {
            if (obstacleData == null) { return; }

            bool isDirty = false;

            // Update the component on this GameObject when obstacle data changes.
            T ProcessAssignment<T>(T target, T origin)
            {
                if (!isDirty && !EqualityComparer<T>.Default.Equals(origin, target))
                {
                    //Debug.Log(origin + " doesnt equal " + target);
                    isDirty = true;
                }
                return origin;
            }

            // Update the misc data of the obstacle.
            //obstacleData.Tag = gameObject.tag;
            obstacleData.Tag = ProcessAssignment(obstacleData.Tag, gameObject.tag);
            if (rend != null)
            {
                obstacleData.ObstacleSprite = ProcessAssignment(obstacleData.ObstacleSprite, rend.sprite);
                obstacleData.OrderInLayer = ProcessAssignment(obstacleData.OrderInLayer, rend.sortingOrder);
                //obstacleData.ObstacleSprite = rend.sprite;
                //obstacleData.OrderInLayer = rend.sortingOrder;
            }
            if (score != null)
            {
                obstacleData.BaseScore = ProcessAssignment(obstacleData.BaseScore, score.BaseScore);
                //obstacleData.BaseScore = score.BaseScore;
            }
            if (relay != null)
            {
                obstacleData.DestroySound = ProcessAssignment(obstacleData.DestroySound, relay.SoundName);
                //obstacleData.DestroySound = relay.SoundName;
            }
            if (outliner != null)
            {
                obstacleData.ShowOutline = ProcessAssignment(obstacleData.ShowOutline, outliner.ShowOutline);
                //obstacleData.ShowOutline = outliner.ShowOutline;
            }

            // Update the collision data of the obstacle.
            if (obstacleCollider != null)
            {
                obstacleData.HasCollision = ProcessAssignment(obstacleData.HasCollision, !obstacleCollider.isTrigger);
                obstacleData.HitboxOffset = ProcessAssignment(obstacleData.HitboxOffset, obstacleCollider.offset);
                obstacleData.HitboxSize = ProcessAssignment(obstacleData.HitboxSize, obstacleCollider.size);
                obstacleData.HitboxDirection = ProcessAssignment(obstacleData.HitboxDirection, obstacleCollider.direction);
                //obstacleData.HasCollision = !obstacleCollider.isTrigger;
                //obstacleData.HitboxOffset = obstacleCollider.offset;
                //obstacleData.HitboxSize = obstacleCollider.size;
                //obstacleData.HitboxDirection = obstacleCollider.direction;
            }


            // Particles
            if (particles != null)
            {
                var animModule = particles.textureSheetAnimation;
                // Copies the current sprite sheet data to the sprite sheet array.
                Sprite[] spriteSheet = new Sprite[animModule.spriteCount];
                obstacleData.SpriteSheet.CopyTo(spriteSheet, 0);
                // Update the sprite sheet for the particles.
                for (int i = 0; i < animModule.spriteCount; i++)
                {
                    //Debug.Log(spriteSheet[i]);
                    spriteSheet[i] = ProcessAssignment(spriteSheet[i], animModule.GetSprite(i));
                    //spriteSheet[i] = animModule.GetSprite(i);
                }
                obstacleData.SpriteSheet = spriteSheet;

                // Update the emission shape.
                var shapeModule = particles.shape;
                obstacleData.EmissionRadius = ProcessAssignment(obstacleData.EmissionRadius, shapeModule.radius);
                //obstacleData.EmissionRadius = shapeModule.radius;

                // Update the number of particles emitted by the burst.
                var emissionModue = particles.emission;
                var burst = emissionModue.GetBurst(0);
                //obstacleData.ParticleNumber = (int)burst.count.constant;
                obstacleData.ParticleNumber = ProcessAssignment(obstacleData.ParticleNumber, (int)burst.count.constant);
            }

            //SaveObstacleData();
            // Set our obstacle data as dirty if we've modified this game object.
            if (isDirty)
            {
                Debug.Log("Obstacle data dirty");
                EditorUtility.SetDirty(obstacleData);
            }
        }

        /// <summary>
        /// Saves any changes to the ObstacleData asset.
        /// </summary>
        private void SaveObstacleData()
        {
            if (obstacleData != null)
            {
                // Save changes to the asset.
                EditorUtility.SetDirty(obstacleData);
                AssetDatabase.SaveAssetIfDirty(obstacleData);
            }
        }
#endif

        /// <summary>
        /// Reads the data from this object's obstacleData and updates the components on this GameObject.
        /// </summary>
        [ContextMenu("Read Obstacle Data")]
        public void ReadObstacleData()
        {
            if (obstacleData == null) { return; }

#if UNITY_EDITOR
            // Only care about IsDirty if we're modifying the object in the editor.
            bool isDirty = false;
#endif

            // Update the component on this GameObject when obstacle data changes.
            T ProcessAssignment<T>(T target, T origin)
            {
#if UNITY_EDITOR
                if (!isDirty && !EqualityComparer<T>.Default.Equals(origin, target))
                {
                    //Debug.Log(origin + " doesnt equal " + target);
                    isDirty = true;
                }
#endif
                return origin;
            }

            //Debug.Log(EqualityComparer<float>.Default.Equals(obstacleData.BaseScore, 1));

            //gameObject.tag = obstacleData.Tag;
            gameObject.tag = ProcessAssignment(gameObject.tag, obstacleData.Tag);
            if (rend != null)
            {
                rend.sprite = ProcessAssignment(rend.sprite, obstacleData.ObstacleSprite);
                rend.sortingOrder = ProcessAssignment(rend.sortingOrder, obstacleData.OrderInLayer);
                //rend.sprite = obstacleData.ObstacleSprite;
                //rend.sortingOrder = obstacleData.OrderInLayer;
            }
            if (score != null)
            {
                score.BaseScore = ProcessAssignment(score.BaseScore, obstacleData.BaseScore);
                //score.BaseScore = obstacleData.BaseScore;
            }
            if ( scaler != null)
            {
                scaler.Size = ProcessAssignment(scaler.Size, obstacleData.BaseSize);
                //scaler.Size = obstacleData.BaseSize;
            }
            if (relay != null)
            {
                relay.SoundName = ProcessAssignment(relay.SoundName, obstacleData.DestroySound);
                //relay.SoundName = obstacleData.DestroySound;
            }
            if (outliner != null)
            {
                outliner.ShowOutline = ProcessAssignment(outliner.ShowOutline, obstacleData.ShowOutline);
                //outliner.ShowOutline = obstacleData.ShowOutline;
            }

            // Collider Updates
            if (obstacleCollider != null)
            {
                obstacleCollider.isTrigger = ProcessAssignment(obstacleCollider.isTrigger, !obstacleData.HasCollision);
                obstacleCollider.offset = ProcessAssignment(obstacleCollider.offset, obstacleData.HitboxOffset);
                obstacleCollider.size = ProcessAssignment(obstacleCollider.size, obstacleData.HitboxSize);
                obstacleCollider.direction = ProcessAssignment(obstacleCollider.direction, obstacleData.HitboxDirection);
                //obstacleCollider.isTrigger = !obstacleData.HasCollision;
                //obstacleCollider.offset = obstacleData.HitboxOffset;
                //obstacleCollider.size = obstacleData.HitboxSize;
                //obstacleCollider.direction = obstacleData.HitboxDirection;
            }

            // Particles
            if (particles != null)
            {
                if (obstacleData.SpriteSheet != null)
                {
                    var animModule = particles.textureSheetAnimation;
                    // Update the sprite sheet for the particles.
                    for (int i = 0; i < obstacleData.SpriteSheet.Length; i++)
                    {
                        if (i >= animModule.spriteCount)
                        {
                            //Debug.Log("Added sprite");
                            animModule.AddSprite(obstacleData.SpriteSheet[i]);
                            // Set dirty flag manually.  If we need to add a sprite, then we're dirty.
                            isDirty |= true;
                        }
                        else
                        {
                            // Check for if the sprites are different manually.
                            if (animModule.GetSprite(i) != obstacleData.SpriteSheet[i])
                            {
                                animModule.SetSprite(i, obstacleData.SpriteSheet[i]);
                                isDirty |= true;
                            }
                        }
                    }
                }

                // Update the emission shape.
                var shapeModule = particles.shape;
                //shapeModule.radius = obstacleData.EmissionRadius;
                shapeModule.radius = ProcessAssignment(shapeModule.radius, obstacleData.EmissionRadius);

                // Update the number of particles emitted by the burst.
                var emissionModue = particles.emission;
                var burst = emissionModue.GetBurst(0);
                burst.count = ProcessAssignment((int)burst.count.constant, obstacleData.ParticleNumber);
                //burst.count = obstacleData.ParticleNumber;
                emissionModue.SetBurst(0, burst);
            }

#if UNITY_EDITOR
            // If one of our relevant values was changed, we need to mark the game object as dirty if the game is
            // not playing.
            //Debug.Log(isDirty);
            if (!EditorApplication.isPlaying && isDirty)
            {
                Debug.Log("Game object dirty");
                EditorUtility.SetDirty(gameObject);
            }
#endif
        }
        #endregion

        /// <summary>
        /// Return this obstacle to the obstacle spawner's pool.
        /// </summary>
        public void ReturnObstacle()
        {
            if (obstacleReturnFunction != null)
            {
                obstacleReturnFunction(this);
            }
            // If no return function is specified, the object is simply destroyed.
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
