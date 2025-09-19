/*****************************************************************************
// File Name : ObstacleCollision.cs
// Author : Brandon Koederitz
// Creation Date : 9/16/2025
// Last Modified : 9/18/2025
//
// Brief Description : Controls the result of a snowball's collision with an obstacle.  Goes on the snowball.
*****************************************************************************/
using System.Collections;
using UnityEngine;

namespace Snowmentum
{
    [RequireComponent(typeof(Collider2D))]
    public class SnowballCollision : MonoBehaviour
    {
        [SerializeField, Tooltip("Controls how long the snowball stays immune to collisions after being hit.")] 
        private float hitImmunity;
        [SerializeField] internal SnowballSize snowballSize;
        [SerializeField] private SnowballValue[] affectedValues;

        //[Header("Speed")]
        //[SerializeField] private ScriptableValue snowballSpeed;
        //[SerializeField, Tooltip("A ScriptableObject that holds the formula for determining the effect on the " +
        //    "actual speed of the snowball due to a collision.  Used to create the knockback effect.")]
        //private CollisionResultCurve effectOnSpeed;

        private bool isImmune;

        private void Awake()
        {
            // Subscribe to our size value's OnChagneEvent so that when size is reduced, we can gain immunity.
            snowballSize.OnTargetValueChanged += OnSizeChanged;
        }

        private void OnDestroy()
        {
            // Unsubscribe events.
            snowballSize.OnTargetValueChanged -= OnSizeChanged;
        }

        /// <summary>
        /// When the obstacle collides with the snowball, it will affect the snowball's values and evaluate if it gets
        /// destroyed or not.
        /// </summary>
        /// <param name="collision"></param>
        private void OnCollisionEnter2D(Collision2D collision)
        {
            Debug.Log(isImmune);
            if (!isImmune && collision.gameObject.TryGetComponent(out ObstacleCollision obstacle))
            {
                // Save the snowball's current size so that any changes to size dont affect any of the other math.
                float snowballSizeVal = snowballSize.Value;

                // Set a bool at the beginning so that no matter the result of the collision on the player's size,
                // the obstacle will get destroyed if the snowball was larger than it.
                bool flagForDestroy = snowballSize.Value > obstacle.ObstacleSize;

                Debug.Log("Collided with " + collision.gameObject.name);

                // Change the player's values based on our result curves defined in the inspector.
                foreach(SnowballValue val in  affectedValues)
                {
                    val.OnCollision(obstacle.ObstacleSize, snowballSizeVal);
                }

                if (flagForDestroy)
                {
                    obstacle.DestroyObstacle();
                }
            }
        }

        #region Immunity
        /// <summary>
        /// Handles the size of the snowball changing.
        /// </summary>
        /// <param name="current"></param>
        /// <param name="old"></param>
        private void OnSizeChanged(float current, float old)
        {
            float change = current - old;
            // If we experienced a negative change in size, then we should gain immunity for a bit.
            if (!isImmune && change < 0)
            {
                StartCoroutine(ImmunityRoutine(hitImmunity));
            }
        }

        /// <summary>
        /// Gives the snowball immunity from collisions for a time after it takes damage from something.
        /// </summary>
        /// <param name="duration"></param>
        /// <returns></returns>
        private IEnumerator ImmunityRoutine(float duration)
        {
            isImmune = true;
            yield return new WaitForSeconds(duration);
            isImmune = false;
        }
        #endregion
    }
}
