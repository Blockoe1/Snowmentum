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
        [SerializeField] private int orderInLayer;
        [SerializeField] private int baseScore;
        [SerializeField] private string tag = "Untagged";
        [SerializeField] private string destroySound;
        [SerializeField] private bool showOutline;

        [Header("Hitbox")]
        [SerializeField] private bool isTrigger;
        [SerializeField] private CapsuleDirection2D capsuleDirection; 
        [SerializeField] private Vector2 offset;
        [SerializeField] private Vector2 size = Vector2.one;

        [SerializeField, HideInInspector] private float baseSize;

        #region Properties
        public float ObstacleSize
        {
            get { return obstacleSize; }
            set 
            {
                obstacleSize = value;
                baseSize = SizeBracket.GetMinSize(SizeBracket.GetBracket(obstacleSize));
            }
        }
        public Sprite ObstacleSprite
        {
            get { return obstacleSprite; }
            set { obstacleSprite = value; }
        }
        public int OrderInLayer
        {
            get { return orderInLayer; }
            set { orderInLayer = value; }
        }
        public int BaseScore
        {
            get { return baseScore; }
            set { baseScore = value; }
        }
        public string Tag
        {
            get { return tag; }
            set { tag = value; }
        }
        public string DestroySound
        {
            get { return destroySound; }
            set { destroySound = value; }
        }
        public bool IsTrigger
        {
            get { return isTrigger; }
            set { isTrigger = value; }
        }
        public bool ShowOutline
        { 
            get { return showOutline; }
            set { showOutline = value; }
        }
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
            ObstacleSize = obstacleSize;
        }
    }
}
