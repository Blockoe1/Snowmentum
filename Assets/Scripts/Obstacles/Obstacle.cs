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
        [SerializeField, Tooltip("The size required for the snowball to destroy this obstacle.  Does not affect " +
            "obstacle scale.")] 
        protected float obstacleSize;
        [SerializeField, Tooltip("The sprite of the obstacle.")] private Sprite obstacleSprite;
        [SerializeField, Tooltip("Obstacle renderer's order within the obstacle sorting later.")] 
        private int orderInLayer;
        [SerializeField, Tooltip("The base amount of score the player gains when this obstacle is destroyed by a " +
            "snowball of similar size")] 
        private int baseScore;
        [SerializeField, Tooltip("The sound to play when this obstacle is destroyed.")]
        private string destroySound = "Obstacle Destruction";
        [SerializeField, Tooltip("Whether this obstacle should show an outline as the snowball gets close or not.")]
        private bool showColors = true;
        [SerializeField, Tooltip("The tag of the obstacle game object.  Only used for specific special cases where " +
            "a custom identifier is needed, such as puddles.")] 
        private string tag = "Untagged";

        [Header("Hitbox")]
        [SerializeField, Tooltip("Whether the obstacle has normal collision with the snowball.")] 
        private bool hasCollision = true;
        [SerializeField, Tooltip("The direction of the obstacle's capsule collider.")] 
        private CapsuleDirection2D capsuleDirection; 
        [SerializeField, Tooltip("The offset of the obstacle's collider.")] 
        private Vector2 offset;
        [SerializeField, Tooltip("The size of the obstacle's collider.")] 
        private Vector2 size = Vector2.one;

        [Header("Particles")]
        [SerializeField, Tooltip("The sprites used by this obstacle's destruction particles.")]
        private Sprite[] particleSpriteSheet;
        [SerializeField, Tooltip("The number of particles to spawn when the obstacle is destroyed.")] 
        private int particleNumber;
        [SerializeField, Tooltip("The size of the circle that particles spawn from when the obstacle is destroyed.  " +
            "Should approximately correspond to the size of the sprite.")] 
        private float emissionRadius;

        [Header("Lighting")]
        [SerializeField] private float innerRadius;
        [SerializeField] private float outerRadius;

        [Header("Greyboxing Only")]
        [SerializeField] private bool isGreyboxed;
        [SerializeField] private Vector2 spriteSize = Vector2.one;

        [SerializeField, HideInInspector] protected float baseSize;

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
        public bool HasCollision
        {
            get { return hasCollision; }
            set { hasCollision = value; }
        }
        public bool ShowColors
        { 
            get { return showColors; }
            set { showColors = value; }
        }
        #region Particles
        public Sprite[] SpriteSheet
        {
            get { return particleSpriteSheet; }
            set { particleSpriteSheet = value; }
        }
        public int ParticleNumber
        {
            get { return particleNumber; }
            set { particleNumber = value; }
        }
        public float EmissionRadius
        {
            get { return emissionRadius; }
            set { emissionRadius = value; }
        }
        #endregion

        #region Hitbox
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

        #region Lighting
        public float InnerRadius
        { 
            get { return innerRadius; }
            set { innerRadius = value; }
        }
        public float OuterRadius
        {
            get { return outerRadius; }
            set { outerRadius = value; }
        }
        #endregion
        #region Greyboxing
        public Vector2 SpriteSize
        {
            get { return spriteSize; }
            set { spriteSize = value; }
        }
        public bool IsGreyboxed
        {
            get { return isGreyboxed; }
            set { isGreyboxed = value; }
        }
        #endregion
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
