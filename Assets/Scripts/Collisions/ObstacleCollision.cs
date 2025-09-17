/*****************************************************************************
// File Name : ObstacleCollision.cs
// Author : Brandon Koederitz
// Creation Date : 9/16/2025
// Last Modified : 9/16/2025
//
// Brief Description : Controls the result of a collision with the player.
*****************************************************************************/
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace Snowmentum
{
    [RequireComponent(typeof(Collider2D))]
    public class ObstacleCollision : MonoBehaviour
    {
        #region CONSTS
        private const string PLAYER_TAG = "Player";
        #endregion

        [Header("Size")]
        [SerializeField] private ScriptableValue snowballSize;
        [SerializeField, Tooltip("A ScriptableObject that holds the formula for determiining the effect on player " +
            "size when this obstacle collides with the player.")]
        private CollisionResultCurve effectOnSize;
        [Header("Speed")]
        [SerializeField] private ScriptableValue snowballSpeed;
        [SerializeField, Tooltip("A ScriptableObject that holds the formula for determiining the effect on player " +
            "speed when this obstacle collides with the player.")]
        private CollisionResultCurve effectOnSpeed;

        [Header("Events")]
        [SerializeField] private UnityEvent OnDestroyEvent;

        #region Component References
        [Header("Components")]
        [SerializeReference] protected ObjectScaler scaler;

        /// <summary>
        /// Get components on reset.
        /// </summary>
        [ContextMenu("Get Component References")]
        private void Reset()
        {
            scaler = GetComponent<ObjectScaler>();
        }
        #endregion

        #region Nested
        public float ObstacleSize => scaler.Size;
        #endregion

        /// <summary>
        /// When the obstacle collides with the snowball, it will affect the snowball's values and evaluate if it gets
        /// destroyed or not.
        /// </summary>
        /// <param name="collision"></param>
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag(PLAYER_TAG))
            {
                // Set a bool at the beginning so that no matter the result of the collision on the player's size,
                // this obstacle will get destroyed if the snowball was larger than it.
                bool flagForDestroy = snowballSize.Value > ObstacleSize;

                float sizeRatio = ObstacleSize / snowballSize.Value;
                Debug.Log("Collided with Player");

                // Change the player's values based on our result curves.
                UpdateValue(snowballSize, effectOnSize, ObstacleSize, snowballSize.Value);
                UpdateValue(snowballSpeed, effectOnSpeed, ObstacleSize, snowballSize.Value);

                if (flagForDestroy)
                {
                    DestroyObstacle();
                }
            }
        }

        /// <summary>
        /// Evaluates the result of a collision between an obstacle and the snowball.
        /// </summary>
        /// <param name="value">The value we're going to change.</param>
        /// <param name="resultCurve">The curve that determines the results.</param>
        /// <param name="obstacleSize">The size of the obstacle.</param>
        /// <param name="snowballSize">The size of the snowvball.</param>
        private static void UpdateValue(ScriptableValue value, CollisionResultCurve resultCurve,  
            float obstacleSize, float snowballSize)
        {

        }

        /// <summary>
        /// Destroys this obstacle.
        /// </summary>
        private void DestroyObstacle()
        {
            OnDestroyEvent?.Invoke();
            Destroy(gameObject);
        }
    }
}
