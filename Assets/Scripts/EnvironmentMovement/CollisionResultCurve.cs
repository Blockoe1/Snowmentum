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
    [CreateAssetMenu(fileName = "CollisionResultCurve", menuName = "ScriptableObjects/CollisionResultCurve")]
    public class CollisionResultCurve : ScriptableObject
    {
        [SerializeField, Tooltip("The steepness of the curve.  Higher numbers will result in harsher punishments" +
            " for colliding with objects that are smaller than you."), Min(2f)]
        private float curveSteepness = 2;
        [SerializeField, Tooltip("The maximum amount that the value can increase when a collision happens.")]
        private float maxGain;

        /// <summary>
        /// Evaluates the values of x and y in the equasion -curveSteepness ^ (-x + y+ logbase[curveSteepness]
        /// (maxGain)) + maxGain.
        /// </summary>
        /// <param name="x">The value X in the equasion.  Should correspond to the snowball's size.</param>
        /// <param name="y">The value Y in the equasion.  Should correspond to the obstacle's size.</param>
        /// <returns>The result of the math, which is the amount the value should be changed.</returns>
        public float Evaluate(float x, float y)
        {
            return Mathf.Pow(-curveSteepness, -x + y + Mathf.Log(maxGain, curveSteepness)) + maxGain;
        }
    }
}
