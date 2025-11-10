/*****************************************************************************
// File Name : ObstacleDropper.cs
// Author : Brandon Koederitz
// Creation Date : 11/4/2025
// Last Modified : 11/4/2025
//
// Brief Description : Causes an obstacle to move downwards towards the bottom kill zone when the snowball goes down a
// a bracket to despawn the obstacle.
*****************************************************************************/
using Snowmentum.Size;
using UnityEngine;

namespace Snowmentum
{
    public class ObstacleDropper : MonoBehaviour, IMovementModifier
    {
        [SerializeField] private float fallSpeed;

        private bool isFalling;

        #region Properties
        public bool IsFalling
        {  
            get { return isFalling; } 
            set { isFalling = value; }
        }
        #endregion

        /// <summary>
        /// Subscribe/unsubscribe to size bracket events based on if this obstacle is enabled or not.
        /// </summary>
        private void OnEnable()
        {
            SizeBracket.OnBracketChanged += CheckBracketChange;
        }

        private void OnDisable()
        {
            SizeBracket.OnBracketChanged -= CheckBracketChange;
            isFalling = false;
        }

        /// <summary>
        /// On move update, moves the object down based on fall speed if the obstacle is falling.
        /// </summary>
        /// <param name="movedObject"></param>
        /// <param name="targetPos"></param>
        /// <param name="moveVector"></param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public Vector2 MoveUpdate(Transform movedObject, Vector2 targetPos, Vector2 moveVector)
        {
            if (isFalling)
            {
                targetPos += (Vector2.down * fallSpeed * Time.fixedDeltaTime);
            }
            return targetPos;
        }

        /// <summary>
        /// When the bracket goes down, we need to set this obstacle to fall.
        /// </summary>
        /// <param name="newSize"></param>
        /// <param name="oldSize"></param>
        private void CheckBracketChange(int newSize, int oldSize)
        {
            if (newSize < oldSize)
            {
                IsFalling = true;
            }    
        }
    }
}
