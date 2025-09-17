/*****************************************************************************
// File Name : CollisionResultCurve.cs
// Author : Brandon Koederitz
// Creation Date : 9/16/2025
// Last Modified : 9/16/2025
//
// Brief Description : Calculates the result of a collision on a given value based on the relative size of the two
// objects.
*****************************************************************************/
using UnityEngine;

namespace Snowmentum
{
    public abstract class CollisionResultCurve : ScriptableObject
    {
        /// <summary>
        /// Evaluates the result of the collision based on the size of the snowball and obstacle.
        /// </summary>
        /// <param name="snowballSize"></param>
        /// <param name="obstacleSize"></param>
        /// <returns>The change in the value.</returns>
        public abstract float Evaluate(float snowballSize, float obstacleSize);
    }
}
