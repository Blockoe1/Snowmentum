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

        /// <summary>
        /// Sets the obstacle 
        /// </summary>
        /// <param name="obstacleData">The obstacle ScriptableObject that this obstacle should be based on.</param>
        public virtual void SetObstacle(Obstacle obstacleData)
        {
            if (obstacleData == null) { return; }
            this.obstacleData = obstacleData;

            // Always toggle the obstacle on when new data is set.
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

        #region Editor Only
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
            if (autoUpdateObstacleData && obstacleData != null && obstacleData != oldObsData)
            {
                Debug.Log("Updated");
                // Run SetObstacle so other values are updated.
                //SetObstacle(obstacleData);
                Editor_ReadObstacleData();
                oldObsData = obstacleData;
            }
        }

        /// <summary>
        /// Updates the data object that controls this obstacle.
        /// </summary>
        [ContextMenu("Write Obstacle Data")]
        public void Editor_WriteObstacleData()
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

        /// <summary>
        /// Reads the data from this object's obstacleData and updates the components on this GameObject.
        /// </summary>
        [ContextMenu("Read Obstacle Data")]
        public void Editor_ReadObstacleData()
        {
            if (obstacleData == null) { return; }

            // Only care about IsDirty if we're modifying the object in the editor.
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

            #region GameObject
            //gameObject.tag = obstacleData.Tag;
            gameObject.tag = ProcessAssignment(gameObject.tag, obstacleData.Tag);
            // Each component needs to be set dirty individually for changes to be properly saved.
            if (isDirty)
            {
                EditorUtility.SetDirty(gameObject);
                isDirty = false;
            }
            #endregion

            #region Sprite
            if (rend != null)
            {
                rend.sprite = ProcessAssignment(rend.sprite, obstacleData.ObstacleSprite);
                rend.sortingOrder = ProcessAssignment(rend.sortingOrder, obstacleData.OrderInLayer);
                //rend.sprite = obstacleData.ObstacleSprite;
                //rend.sortingOrder = obstacleData.OrderInLayer;

                // Each component needs to be set dirty individually for changes to be properly saved.
                if (isDirty)
                {
                    EditorUtility.SetDirty(rend);
                    isDirty = false;
                }
            }
            #endregion

            #region Score
            if (score != null)
            {
                score.BaseScore = ProcessAssignment(score.BaseScore, obstacleData.BaseScore);
                //score.BaseScore = obstacleData.BaseScore;

                // Each component needs to be set dirty individually for changes to be properly saved.
                if (isDirty)
                {
                    EditorUtility.SetDirty(score);
                    isDirty = false;
                }
            }
            #endregion

            #region Scaler
            if ( scaler != null)
            {
                scaler.Size = ProcessAssignment(scaler.Size, obstacleData.BaseSize);
                //scaler.Size = obstacleData.BaseSize;

                // Each component needs to be set dirty individually for changes to be properly saved.
                if (isDirty)
                {
                    EditorUtility.SetDirty(scaler);
                    isDirty = false;
                }
            }
            #endregion

            #region Relay
            if (relay != null)
            {
                relay.SoundName = ProcessAssignment(relay.SoundName, obstacleData.DestroySound);
                //relay.SoundName = obstacleData.DestroySound;

                // Each component needs to be set dirty individually for changes to be properly saved.
                if (isDirty)
                {
                    EditorUtility.SetDirty(relay);
                    isDirty = false;
                }
            }
            #endregion

            #region Outline
            if (outliner != null)
            {
                outliner.ShowOutline = ProcessAssignment(outliner.ShowOutline, obstacleData.ShowOutline);
                //outliner.ShowOutline = obstacleData.ShowOutline;

                // Each component needs to be set dirty individually for changes to be properly saved.
                if (isDirty)
                {
                    EditorUtility.SetDirty(outliner);
                    isDirty = false;
                }
            }
            #endregion

            #region Collider
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

                // Each component needs to be set dirty individually for changes to be properly saved.
                if (isDirty)
                {
                    EditorUtility.SetDirty(obstacleCollider);
                    isDirty = false;
                }
            }
            #endregion

            #region Particles
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

                // Each component needs to be set dirty individually for changes to be properly saved.
                if (isDirty)
                {
                    EditorUtility.SetDirty(particles);
                    isDirty = false;
                }
            }
            #endregion
        }
#endif
#endregion

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
            if (scaler != null)
            {
                scaler.Size = obstacleData.BaseSize;
            }
            if (relay != null)
            {
                relay.SoundName = obstacleData.DestroySound;
            }
            if (outliner != null)
            {
                //outliner.ShowOutline = obstacleData.ShowOutline;
                // run ToggleOutline so that we update the material at runtime.
                outliner.ToggleOutline(obstacleData.ShowOutline);
            }

            // Collider Updates
            if (obstacleCollider != null)
            {
                obstacleCollider.isTrigger = !obstacleData.HasCollision;
                obstacleCollider.offset = obstacleData.HitboxOffset;
                obstacleCollider.size = obstacleData.HitboxSize;
                obstacleCollider.direction = obstacleData.HitboxDirection;
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
                            animModule.AddSprite(obstacleData.SpriteSheet[i]);
                        }
                        else
                        {
                            animModule.SetSprite(i, obstacleData.SpriteSheet[i]);
                        }
                    }
                }

                // Update the emission shape.
                var shapeModule = particles.shape;
                shapeModule.radius = obstacleData.EmissionRadius;

                // Update the number of particles emitted by the burst.
                var emissionModue = particles.emission;
                var burst = emissionModue.GetBurst(0);
                burst.count = obstacleData.ParticleNumber;
                emissionModue.SetBurst(0, burst);
            }
        }

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
