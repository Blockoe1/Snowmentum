/*****************************************************************************
// File Name : Obstacle.cs
// Author : Brandon Koederitz
// Creation Date : 10/1/2025
// Last Modified : 10/1/2025
//
// Brief Description : Holds the data relating to a specific obstacle.
*****************************************************************************/
using Snowmentum.Size;
using UnityEngine;

namespace Snowmentum
{
    [CreateAssetMenu(fileName = "Obstacle", menuName = "ScriptableObjects/Obstacle")]
    public class Obstacle : ScriptableObject
    {
        [SerializeField] private float obstacleSize;
        [SerializeField] private Sprite obstacleSprite;
        [SerializeField] private int baseScore;

        [Header("Hitbox")]
        [SerializeField] private CapsuleDirection2D capsuleDirection;
        [SerializeField] private Vector2 offset;
        [SerializeField] private Vector2 size;

        [SerializeField] private float baseSize;

        #region Properties
        public float ObstacleSize => obstacleSize;
        public Sprite ObstacleSprite => obstacleSprite;
        public int BaseScore => baseScore;
        public Vector2 HitboxOffset
        {
            get { return offset; }
            set { offset = value; }
        }
        public Vector2 HitboxSize
        {
            get { return size; }
            set { size = value; }
        }
        public CapsuleDirection2D HitboxDirection
        {
            get { return capsuleDirection; }
            set { capsuleDirection = value; }
        }
        // The base size fot his obstacle that is used for scaling.
        public float BaseSize
        {
            get
            {
                return baseSize;
            }
        }
        #endregion

        /// <summary>
        /// Automatically hidden values.
        /// </summary>
        private void OnValidate()
        {
            // Update base size 
            baseSize = SizeBracket.GetMinSize(SizeBracket.GetBracket(obstacleSize));
        }
    }
}
