/*****************************************************************************
// File Name : LoopingMovement.cs
// Author : Brandon Koederitz
// Creation Date : 9/22/2025
// Last Modified : 10/1/2025
//
// Brief Description : Continually loops objects across the screen.
*****************************************************************************/
using System;
using UnityEngine;

namespace Snowmentum
{
    public class LoopingMovement : MonoBehaviour, IMovementModifier
    {
        [SerializeField, Tooltip("How far back the object should go once it exceeds the xLimit")]
        private float loopLength;


        /// <summary>
        /// If the object's target position would exceed it's xLimit, then we loop it around to the other side of the
        /// screen.
        /// </summary>
        /// <param name="targetPos"></param>
        /// <returns></returns>
        public Vector2 MoveUpdate(Transform movedObject, Vector2 targetPos, Vector2 moveVector)
        {
            if (Mathf.Abs(targetPos.x) > loopLength / 2)
            {
                targetPos = targetPos + ((loopLength) * Math.Sign(targetPos.x) * moveVector);
            }
            return targetPos;
        }
    }
}
