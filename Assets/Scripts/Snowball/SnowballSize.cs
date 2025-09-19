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
    [CreateAssetMenu(fileName = "SnowballSize", menuName = "ScriptableObjects/SnowballSize")]
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
        /// When the snowball gets in a collision, the formula 
        /// -curveSteepness ^ (-snowballSize + obstacleSize + logbase[curveSteepness](snowballSize)) + snowballSize
        /// is used to  determine the decrease in snowball size.
        /// </summary>
        /// <param name="obstacleSize"></param>
        /// <param name="snowballSize"></param>
        public override void OnCollision(float obstacleSize, float snowballSize)
        {
            float gain = useSizeAsMax ? obstacleSize : maxGain;
            float result =  -Mathf.Pow(curveSteepness, -snowballSize + obstacleSize +
                Mathf.Log(gain, curveSteepness)) + gain;
            TargetValue = result;
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
