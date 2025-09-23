/*****************************************************************************
// File Name : LoopingMovement.cs
// Author : Brandon Koederitz
// Creation Date : 9/22/2025
// Last Modified : 9/22/2025
//
// Brief Description : Continually loops objects across the screen.
*****************************************************************************/
using UnityEngine;

namespace Snowmentum
{
    public class LoopingMovement : MonoBehaviour, IMovementModifier
    {
        [SerializeField] private float xLimit;
        [SerializeField, Tooltip("How far back the object should go once it exceeds the xLimit")]
        private float loopLength;

        /// <summary>
        /// If the object's target position would exceed it's xLimit, then we loop it around to the other side of the
        /// screen.
        /// </summary>
        /// <param name="targetPos"></param>
        /// <returns></returns>
        public Vector2 MoveUpdate(Vector2 targetPos, Vector2 moveVector)
        {
            if (targetPos.x < xLimit)
            {
                targetPos = targetPos - (loopLength * moveVector);
            }
            return targetPos;
        }
    }
}
