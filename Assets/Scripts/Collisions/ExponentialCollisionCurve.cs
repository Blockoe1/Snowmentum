/*****************************************************************************
// File Name : ExponentialCollisionCurve.cs
// Author : Brandon Koederitz
// Creation Date : 9/16/2025
// Last Modified : 9/18/2025
//
// Brief Description : Calculates the result of a collision on a given value based on the relative size of the two
// objects using an exponential function.  Uses the obstacle size as the baseline.  Used to change size on collision.
*****************************************************************************/
using UnityEngine;

namespace Snowmentum
{
    [CreateAssetMenu(fileName = "ExponentialCollisionCurve", menuName = 
        "ScriptableObjects/Collision Curves/ExponentialCollisionCurve")]
    public class ExponentialCollisionCurve : CollisionResultCurve
    {
        [Header("Curve Settings")]
        [SerializeField, Tooltip("The steepness of the curve.  Higher numbers will result in harsher punishments" +
            " for colliding with objects that are smaller than you."), Min(1.001f)]
        private float curveSteepness = 2;
        [SerializeField, Tooltip("The maximum positive value this curve can return.")]
        private float maxGain;
        [SerializeField, Tooltip("If true, then the maxGain parameter will be overwritten by the size of " +
            "the obstacle.")] 
        private bool useSizeAsMax;

        /// <summary>
        /// Evaluates the values of x and y in the equasion -curveSteepness ^ (-snowballSize + obstacleSize + 
        /// logbase[curveSteepness](snowballSize)) + snowballSize.
        /// </summary>
        /// <param name="snowballSize"></param>
        /// <param name="obstacleSize"></param>
        /// <returns>
        /// The result of the math, which is the amount the value should be changed.  Can never exceed the obstacle's
        /// size.
        /// </returns>
        public override float Equasion(float snowballSize, float obstacleSize)
        {
            float gain = useSizeAsMax ? obstacleSize : maxGain;
            return -Mathf.Pow(curveSteepness, -snowballSize + obstacleSize +
                Mathf.Log(gain, curveSteepness)) + gain;
        }
    }
}
