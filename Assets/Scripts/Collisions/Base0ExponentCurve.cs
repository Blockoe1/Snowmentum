/*****************************************************************************
// File Name : Base0ExponentCurve.cs
// Author : Brandon Koederitz
// Creation Date : 9/16/2025
// Last Modified : 9/18/2025
//
// Brief Description : Exponential collision curve that maxes out at a value of 0 and can scale based on an anchor
// position.  Used for changes to speed on collision.
*****************************************************************************/
using UnityEngine;

namespace Snowmentum
{
    [CreateAssetMenu(fileName = "Base0ExponentCurve", menuName =
        "ScriptableObjects/Collision Curves/Base0ExponentCurve")]
    public class Base0ExponentCurve : CollisionResultCurve
    {
        [Header("Curve Settings")]
        [SerializeField, Tooltip("The steepness of the curve.  Higher numbers will result in harsher punishments" +
" for colliding with objects that are smaller than you."), Min(1.001f)]
        private float curveSteepness = 2;

        [SerializeField, Tooltip("The scale of this curve.  The negative value that is returned when a collision " +
            "happens between two objects of the same size will be equal to this."), Min(0.01f)]
        private float curveScale = 0.01f;

        [SerializeField, Tooltip("The minimum value evaluated from this curve.  Automatically converted to a " +
            "negative number, because no values greater than 0 can be output by this curve."), Min(0)]
        private float minValue;

        /// <summary>
        /// Evaluates the values of x and y in the equasion -(curveSteepness ^ (-snowballSize + obstacleSize)) 
        /// * curveScale.
        /// </summary>
        /// <param name="snowballSize"></param>
        /// <param name="obstacleSize"></param>
        /// <returns>
        /// The result of the math, which is the amount the value should be changed.  Can never exceed the obstacle's
        /// size.
        /// </returns>
        public override float Equasion(float snowballSize, float obstacleSize)
        {
            float result = -Mathf.Pow(curveSteepness, (-snowballSize + obstacleSize)) * curveScale;
            result = Mathf.Min(result, -minValue);
            return result;
        }
    }
}
