/*****************************************************************************
// File Name : SnowballSize.cs
// Author : Brandon Koederitz
// Creation Date : 9/19/2025
// Last Modified : 9/19/2025
//
// Brief Description : Holds the snowball's current size.
*****************************************************************************/
using UnityEngine;

namespace Snowmentum
{
    [CreateAssetMenu(fileName = "SnowballSize", menuName = "ScriptableObjects/ScriptableValues/SnowballSize")]
    public class SnowballSize : ScriptableValue
    {
        [SerializeField, Tooltip("The steepness of the curve.  Higher numbers will result in harsher punishments" +
    " for colliding with objects that are smaller than you."), Min(1.001f)]
        private float curveSteepness = 2;
        [SerializeField, Tooltip("The maximum positive value this curve can return.")]
        private float maxGain;
        [SerializeField, Tooltip("If true, then the maxGain parameter will be overwritten by the size of " +
            "the obstacle.")]
        private bool useSizeAsMax;

        /// <summary>
        /// When the snowball gets in a collision, the snowball's size should be decreased.
        /// </summary>
        /// <param name="obstacleSize"></param>
        /// <param name="snowballSize"></param>
        public override void OnCollision(float obstacleSize, float snowballSize)
        {
            float gain = useSizeAsMax ? obstacleSize : maxGain;
            // Calculate the change in size.
            float result = SizeCollisionCurve(obstacleSize, snowballSize, gain, curveSteepness);
            // Ensures the player only takes a certain amount of damage if they are not one-shot.
            if (result < 0 && Mathf.Abs(result) < TargetValue)
            {
                result = Mathf.Max(result, -TargetValue * maxDamageProportion);
            }
            TargetValue += result;
        }

        /// <summary>
        /// The formula
        /// -curveSteepness ^ (-snowballSize + obstacleSize + logbase[curveSteepness](snowballSize)) + snowballSize
        /// that is used to calculate the change to snowball size on collision.
        /// </summary>
        /// <param name="obstacleSize"></param>
        /// <param name="snowballSize"></param>
        /// <param name="gain">The maximum positive value this curve can output.</param>
        /// <param name="curveSteepness"></param>
        /// <returns></returns>
        private static float SizeCollisionCurve(float obstacleSize, float snowballSize, float gain, float curveSteepness)
        {
            return -Mathf.Pow(curveSteepness, -snowballSize + obstacleSize +
                Mathf.Log(gain, curveSteepness)) + gain;
        }

        /// <summary>
        /// Size should lerp towards it's target value so that changes in size arae animated, but very quick.
        /// </summary>
        protected override void MoveToTarget()
        {
            // If our value is close enough to our target value, then we snap it to the value directly
            if (MathHelpers.ApproximatelyWithin(Value, TargetValue))
            {
                Value = TargetValue;
            }
            else
            {
                Value = Mathf.Lerp(Value, TargetValue, moveToTargetSpeed);
            }
        }
    }
}
