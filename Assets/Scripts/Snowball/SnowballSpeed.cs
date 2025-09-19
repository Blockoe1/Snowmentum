/*****************************************************************************
// File Name : SnowballSpeed.cs
// Author : Brandon Koederitz
// Creation Date : 9/19/2025
// Last Modified : 9/19/2025
//
// Brief Description : From a gameplay standpoint, it controls the speed that the snowball moves down the hill.
// In actuality, it controls the speed that obstacles move towards the snowball.
*****************************************************************************/
using UnityEngine;

namespace Snowmentum
{
    [CreateAssetMenu(fileName = "SnowballSpeed", menuName = "ScriptableObjects/ScriptableValues/SnowballSpeed")]
    public class SnowballSpeed : ScriptableValue
    {
        [SerializeField, Tooltip("The steepness of the curve.  Higher numbers will result in harsher punishments" +
            " for colliding with objects that are smaller than you."), Min(1.001f)]
        private float curveSteepness = 2;

        [SerializeField, Tooltip("The scale of this curve.  The negative value that is returned when a collision " +
            "happens between two objects of the same size will be equal to this."), Min(0.01f)]
        private float curveScale = 0.01f;

        [SerializeField, Tooltip("The minimum target speed that the snowball can be at.")]
        private float minSpeed;
        [Header("Knockback Settings")]
        [SerializeField, Tooltip("The knockback applied to the snowball when the obstacle is not destroyed."), Min(0f)] 
        private float baseDamageKnockback;
        [SerializeField, Tooltip("The steepness of the curve.  Higher numbers will result in harsher punishments" +
            " for colliding with objects that are smaller than you."), Min(1.001f)]
        private float knockbackCurveSteepness = 2;
        [SerializeField, Tooltip("The scale of this curve.  The negative value that is returned when a collision " +
    "happens between two objects of the same size will be equal to this."), Min(0.01f)]
        private float knockbackCurveScale = 0.01f;

        public override float Value
        {
            set
            {
                base.Value = Mathf.Max(value, minSpeed);
            }
        }

        /// <summary>
        /// When the snowball gets in a collision, they should get a kickback of speed and their target speed should
        /// be reduced.
        /// </summary>
        /// <param name="obstacleSize"></param>
        /// <param name="snowballSize"></param>
        public override void OnCollision(float obstacleSize, float snowballSize)
        {
            // Target speed reduction.
            float result = SpeedCollisionCurve(obstacleSize, snowballSize, curveScale, curveSteepness);
            // Ensures the player only loses up to a certain amount of speed per collision.
            if (result < 0)
            {
                result = Mathf.Max(result, -TargetValue * maxDamageProportion);
            }
            TargetValue += result;

            // Speed kickback.
            if (obstacleSize > snowballSize)
            {
                // If the obstacle is larger than the snowball, then a fixed kickback is applied so that the player
                // always has some room to move around the obstacle.
                Value = -baseDamageKnockback;
            }
            else
            {
                // If the snowball was larger, then the obstacle is destroyed and we can apply a smaller kickback
                // since the obstacle is no longer in the way.
                result = SpeedCollisionCurve(obstacleSize, snowballSize, knockbackCurveScale, knockbackCurveSteepness);
                // Scale the effect on the speed based on our current speed.  Done this way so that if a reult of 0
                // is returned, then no speed is changed, but we can still have an affect on our speed if its high.
                Value += Value * result;
            }
        }

        /// <summary>
        /// The formula
        /// -(curveSteepness ^ (-snowballSize + obstacleSize)) * curveScale,
        /// which is used for determining changes to speed.
        /// </summary>
        /// <param name="obstacleSize"></param>
        /// <param name="snowballSize"></param>
        /// <param name="curveScale"></param>
        /// <param name="curveSteepness"></param>
        /// <returns></returns>
        private static float SpeedCollisionCurve(float obstacleSize, float snowballSize, float curveScale, 
            float curveSteepness)
        {
            return -Mathf.Pow(curveSteepness, (-snowballSize + obstacleSize)) * curveScale;
        }
    }
}
