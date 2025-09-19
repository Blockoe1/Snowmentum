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
            ScaledAdd,
            Set
        }
        // Represents a value that is damaged by collisions with obstacles
        [System.Serializable]
        public struct DamagedValue
        {
            [SerializeField] internal ScriptableValue value;
            [SerializeField, Tooltip("A ScriptableObject that holds the formula for determining the effect on " +
                "this value when the snowball collides with an obstacle larger than it.")]
            internal CollisionResultCurve damageResultCurve;
            [SerializeField, Tooltip("A ScriptableObject that holds the formula for determining the effect on " +
                "this value when the snowball collides with an obstacle smaller than it that it destroys.")]
            internal CollisionResultCurve destroyResultCurve;
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
            // If the snowball isn't large enough to destroy the obstacle, we need to use a different collision curve.
            CollisionResultCurve curve = obstacleSize > snowballSize ? damageValue.damageResultCurve : 
                damageValue.destroyResultCurve;

            // If our curve is null, then we skip applying any changes.
            if (curve != null)
            {
                float result = curve.Equasion(snowballSize, obstacleSize);


                switch (damageValue.damageResultCurve.applyType)
                {
                    case ApplyType.Add:
                        damageValue.value.Value += result;
                        break;
                    case ApplyType.ScaledAdd:
                        damageValue.value.Value += (result * damageValue.value.Value);
                        break;
                    case ApplyType.Set:
                        damageValue.value.Value = result;
                        break;
                    default:
                        break;
                }

                Debug.Log($"Result {result} was calculated and the value {damageValue.value.Value} was assigned to " +
                    $"ScriptableValue " + damageValue.value.name);
            }
        }
    }
}
