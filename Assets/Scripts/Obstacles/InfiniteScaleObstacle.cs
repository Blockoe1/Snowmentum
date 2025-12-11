/*****************************************************************************
// File Name : InfiniteScaleObstacle.cs
// Author : Brandon Koederitz
// Creation Date : 10/25/2025
// Last Modified : 10/25/2025
//
// Brief Description : Special obstacle variant that infintiely scales based on the current size bracket.
*****************************************************************************/
using Snowmentum.Size;
using UnityEditor.Rendering;
using UnityEngine;

namespace Snowmentum
{
    [CreateAssetMenu(fileName = "InfiniteScaleObstacle", menuName = "ScriptableObjects/Infinite Scale Obstacle")]
    public class InfiniteScaleObstacle : Obstacle
    {
        [SerializeField, Range(0f, 1f)] private float minRandRatio;
        [SerializeField, Range(0f, 1f)] private float maxRandRatio;

        /// <summary>
        /// When this SO is enabled/Disabled, subscribe/Unsubscribe to the size bracket's OnBracketChanged event
        /// So we can randomize this object's size based on the new bracket.
        /// </summary>
        private void OnEnable()
        {
            SizeBracket.OnBracketChanged += RandomizeSize;
        }
        private void OnDisable()
        {
            SizeBracket.OnBracketChanged -= RandomizeSize;
            // Reset back to 1 after a play session.
            obstacleSize = 1;
        }

        /// <summary>
        /// Randomizes this obstacle's size when we reach a new bracket.
        /// </summary>
        /// <remarks>
        /// Yes, this will change the size for all obstacles that reference this SO.  Intentional.  We can define a
        /// set number of infinite obstacles, then each time we reach a new bracket those obstacles keep a
        /// consistent size.
        /// </remarks>
        /// <param name="bracket"></param>
        private void RandomizeSize(int bracket, int oldBracket)
        {
            obstacleSize = SizeBracket.GetMinSize(bracket) + Random.Range(SizeBracket.GetMaxSize(bracket) * 
                minRandRatio, SizeBracket.GetMaxSize(bracket) * maxRandRatio);
            //Debug.Log(obstacleSize);

            // Set obstacleSize to base size directly so that the obstacle scales up despite it's sprite.
            // Infinite scale obstacles need sprites that are the max resolution (64 x 64) and have 4x the base PPU
            // (128) so that they scale properly and we dont have big pixels.
            baseSize = obstacleSize;
        }
    }
}
