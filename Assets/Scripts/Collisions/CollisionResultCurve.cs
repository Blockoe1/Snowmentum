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
        [SerializeField] internal ApplyType applyType;
        [SerializeField, Tooltip("The maximum proportion of this value that can be lost from a " +
    "collision, unless it is reduced to less than 0.  Set to 1 to have no max value."), Range(0f, 1f)]
        internal float maxDamageProportion;

        #region Nested
        internal enum ApplyType
        {
            Add,
            ScaledAdd
        }
        // Represents a value that is damaged by collisions with obstacles
        [System.Serializable]
        public struct DamagedValue
        {
            [SerializeField] internal ScriptableValue value;
            [SerializeField, Tooltip("A ScriptableObject that holds the formula for determining the effect on " +
                "this value when this obstacle collides with the snowball.")]
            internal CollisionResultCurve resultCurve;
        }
        #endregion

        /// <summary>
        /// Evaluates the result of the collision based on the size of the snowball and obstacle.
        /// </summary>
        /// <param name="snowballSize"></param>
        /// <param name="obstacleSize"></param>
        /// <returns>The change in the value.</returns>
        public abstract float Equasion(float snowballSize, float obstacleSize);

        /// <summary>
        /// Evaluates the effect on this value based on it's result curve and defined maximum damage amounts.
        /// </summary>
        /// <param name="obstacleSize"></param>
        /// <param name="snowballSize"></param>
        public static void ApplyValueChange(DamagedValue damageValue, float obstacleSize, float snowballSize)
        {
            float result = damageValue.resultCurve.Equasion(snowballSize, obstacleSize);

            // If the player is taking damage,
            // and the player is not taking enough damage to be one shot,
            // then we need to clamp the return value so that it doesnt exceed maxDamageProportion
            if (result < 0 && Mathf.Abs(result) < obstacleSize)
            {
                result = Mathf.Max(result, -obstacleSize * damageValue.resultCurve.maxDamageProportion);
            }
            switch (damageValue.resultCurve.applyType)
            {
                case ApplyType.Add:
                    damageValue.value.Value += result;
                    break;
                case ApplyType.ScaledAdd:
                    damageValue.value.Value += (result * damageValue.value.Value);
                    break;
                default:
                    break;
            }
        }
    }
}
