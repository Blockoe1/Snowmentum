/*****************************************************************************
// File Name : ObstacleCollision.cs
// Author : Brandon Koederitz
// Creation Date : 9/16/2025
// Last Modified : 9/18/2025
//
// Brief Description : Controls the result of a snowball's collision with an obstacle.  Goes on the snowball.
*****************************************************************************/
using UnityEngine;

namespace Snowmentum
{
    [RequireComponent(typeof(Collider2D))]
    public class SnowballCollision : MonoBehaviour
    {
        [SerializeField] internal ScriptableValue snowballSize;
        [SerializeField] private DamagedValue[] affectedValues;

        //[Header("Speed")]
        //[SerializeField] private ScriptableValue snowballSpeed;
        //[SerializeField, Tooltip("A ScriptableObject that holds the formula for determining the effect on the " +
        //    "actual speed of the snowball due to a collision.  Used to create the knockback effect.")]
        //private CollisionResultCurve effectOnSpeed;

        #region Nested
        internal enum ApplyType
        { 
            Add,
            Set
        }
        // Represents a value that is damaged by collisions with obstacles
        [System.Serializable]
        private struct DamagedValue
        {
            [SerializeField] internal ScriptableValue value;
            [SerializeField, Tooltip("A ScriptableObject that holds the formula for determining the effect on " +
                "this value when this obstacle collides with the snowball.")]
            internal CollisionResultCurve effectOnValue;

            [SerializeField, Tooltip("The maximum proportion of this value that can be lost from a " +
        "collision, unless it is reduced to less than 0.  Set to 1 to have no max value."), Range(0f, 1f)]
            internal float maxDamageProportion;
            [SerializeField] internal ApplyType applyType;
        }

        #endregion

        /// <summary>
        /// When the obstacle collides with the snowball, it will affect the snowball's values and evaluate if it gets
        /// destroyed or not.
        /// </summary>
        /// <param name="collision"></param>
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.TryGetComponent(out ObstacleCollision obstacle))
            {
                // Save the snowball's current size so that any changes to size dont affect any of the other math.
                float snowballSizeVal = snowballSize.Value;

                // Set a bool at the beginning so that no matter the result of the collision on the player's size,
                // the obstacle will get destroyed if the snowball was larger than it.
                bool flagForDestroy = snowballSize.Value > obstacle.ObstacleSize;

                Debug.Log("Collided with " + collision.gameObject.name);

                // Change the player's values based on our result curves defined in the inspector.
                foreach(DamagedValue val in  affectedValues)
                {
                    ApplyValueChange(val, obstacle.ObstacleSize, snowballSizeVal);
                }

                if (flagForDestroy)
                {
                    obstacle.DestroyObstacle();
                }
            }
        }

        /// <summary>
        /// Evaluates the effect on this value based on it's result curve and defined maximum damage amounts.
        /// </summary>
        /// <param name="obstacleSize"></param>
        /// <param name="snowballSize"></param>
        private static void ApplyValueChange(DamagedValue damageValue, float obstacleSize, float snowballSize)
        {
            float result = damageValue.effectOnValue.Evaluate(snowballSize, obstacleSize);

            // If the player is taking damage,
            // and the player is not taking enough damage to be one shot,
            // then we need to clamp the return value so that it doesnt exceed maxDamageProportion
            if (result < 0 && Mathf.Abs(result) < obstacleSize)
            {
                result = Mathf.Max(result, -obstacleSize * damageValue.maxDamageProportion);
            }
            switch (damageValue.applyType)
            {
                case ApplyType.Add:
                    damageValue.value.Value += result;
                    break;
                case ApplyType.Set:
                    damageValue.value.Value = result;
                    break;
                default:
                    break;
            }
        }
    }
}
