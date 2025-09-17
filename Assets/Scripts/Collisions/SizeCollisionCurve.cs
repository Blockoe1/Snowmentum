/*****************************************************************************
// File Name : SizeCollisionCurve.cs
// Author : Brandon Koederitz
// Creation Date : 9/16/2025
// Last Modified : 9/16/2025
//
// Brief Description : Calculates the result of a collision on a given value based on the relative size of the two
// objects using an exponential function.  Uses the obstacle size as the baseline.  Used to change size on collision.
*****************************************************************************/
using UnityEngine;

namespace Snowmentum
{
    [CreateAssetMenu(fileName = "SizeCollisionCurve", menuName = 
        "ScriptableObjects/Collision Curves/SizeCollisionCurve")]
    public class SizeCollisionCurve : CollisionResultCurve
    {
        [SerializeField, Tooltip("The steepness of the curve.  Higher numbers will result in harsher punishments" +
            " for colliding with objects that are smaller than you."), Min(2f)]
        private float curveSteepness = 2;
        [SerializeField, Tooltip("The maximum proportion of the snowball's size that it can lose from a " +
            "collision, unless it gets instant death.")]
        private float maxDamageProportion;
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
        public override float Evaluate(float snowballSize, float obstacleSize)
        {
            float gain = useSizeAsMax ? obstacleSize : maxGain;
            float returnValue = -Mathf.Pow(curveSteepness, -snowballSize + obstacleSize + 
                Mathf.Log(gain, curveSteepness)) + gain;
            // If the player is taking damage,
            // and the player is not taking enough damage to be one shot,
            // then we need to clamp the return value so that it doesnt exceed maxDamageProportion
            if (returnValue < 0 && Mathf.Abs(returnValue) < obstacleSize)
            {
                returnValue = Mathf.Max(returnValue, -obstacleSize * maxDamageProportion);
            }
            return returnValue;
        }
    }
}
