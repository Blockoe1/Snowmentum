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
using Snowmentum.Size;
using UnityEngine.Events;

namespace Snowmentum
{
    [RequireComponent(typeof(Collider2D))]
    public class SnowballCollision : MonoBehaviour
    {
        [SerializeField, Tooltip("Controls how long the snowball stays immune to collisions after being hit.")] 
        private float hitImmunity;
        [SerializeField] private float flashDelay;

        [SerializeField] private SizeUpdater effectOnSize;
        [SerializeField] private SpeedUpdater effectOnSpeed;

        [Header("Events")]
        [SerializeField] private UnityEvent<float> OnTakeDamage;

        private Coroutine immunityRoutine;
        private bool isImmune;
        private bool isInvincible;

        #region Component References
        [Header("Components")]
        [SerializeReference] protected SnowballSize sizeComp;
        [SerializeReference] protected SnowballSpeed speedComp;
        [SerializeReference] private SpriteRenderer flashRenderer;

        /// <summary>
        /// Get components on reset.
        /// </summary>
        [ContextMenu("Get Component References")]
        private void Reset()
        {
            sizeComp = GetComponent<SnowballSize>();
            speedComp = GetComponent<SnowballSpeed>();
            flashRenderer = GetComponent<SpriteRenderer>();
        }
        #endregion

        #region Properties
        public bool IsInvincible
        {
            get { return isInvincible; }
            set { isInvincible = value; }
        }
        #endregion

        #region Nested
        private abstract class ValueUpdater<T>
        {
            [SerializeField, Tooltip("The maximum proportion of this target value that can be lost from a " +
                "collision, unless it is reduced to less than 0.  Set to 1 to have no max value."), Range(0f, 1f)]
            protected float maxDamageProportion;
            [SerializeField, Tooltip("The steepness of the curve.  Higher numbers will result in harsher punishments" +
                " for colliding with objects that are smaller than you."), Min(1.001f)]
            protected float curveSteepness = 2;

            internal abstract void OnCollision(float obstacleSize, float snowballSize, T snowballValue);
        }

        /// <summary>
        /// Updates the snowball's size based on collisions.
        /// </summary>
        [System.Serializable]
        private class SizeUpdater : ValueUpdater<SnowballSize>
        {
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
            internal override void OnCollision(float obstacleSize, float snowballSize, SnowballSize sizeSetter)
            {
                float gain = useSizeAsMax ? obstacleSize : maxGain;
                // Calculate the change in size.
                float result = SizeCollisionCurve(obstacleSize, snowballSize, gain, curveSteepness);
                // Ensures the player only takes a certain amount of damage if they are not one-shot.
                if (result < 0 && Mathf.Abs(result) < SnowballSize.Value)
                {
                    result = Mathf.Max(result, -SnowballSize.Value * maxDamageProportion);
                }
                sizeSetter.TargetValue_Local += result;
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
            private static float SizeCollisionCurve(float obstacleSize, float snowballSize, float gain, 
                float curveSteepness)
            {
                return -Mathf.Pow(curveSteepness, -snowballSize + obstacleSize +
                    Mathf.Log(gain, curveSteepness)) + gain;
            }
        }

        /// <summary>
        /// Updates the snowball's speed based on collisions.
        /// </summary>
        [System.Serializable]
        private class SpeedUpdater : ValueUpdater<SnowballSpeed>
        {
            [SerializeField, Tooltip("The scale of this curve.  The negative value that is returned when a " +
                "collision happens between two objects of the same size will be equal to this."), Min(0.01f)]
            private float curveScale = 0.01f;
            [Header("Knockback Settings")]
            [SerializeField, Tooltip("The minimum knockback applied to the snowball when the obstacle is not " +
                "destroyed."), Min(0f)]
            private float minDamageKnockback;
            [SerializeField, Tooltip("The steepness of the curve.  Higher numbers will result in harsher punishments" +
                " for colliding with objects that are smaller than you."), Min(1.001f)]
            private float knockbackCurveSteepness = 2;
            [SerializeField, Tooltip("The scale of the curve used to calculate knockback.  The negative value " +
                "that is returned when a collision happens between two objects of the same size will be equal " +
                "to this."), Min(0.01f)]
            private float knockbackCurveScale = 0.01f;

            /// <summary>
            /// When the snowball gets in a collision, they should get a kickback of speed and their target speed should
            /// be reduced.
            /// </summary>
            /// <param name="obstacleSize"></param>
            /// <param name="snowballSize"></param>
            internal override void OnCollision(float obstacleSize, float snowballSize, SnowballSpeed speedSetter)
            {
                // Only change size if the snowball takes damage.
                if (snowballSize > obstacleSize) { return; }
                float result;
                // Target speed reduction.  Only reduced if the snowball is smaller.
                result = SpeedCollisionCurve(obstacleSize, snowballSize, curveScale, curveSteepness);
                // Ensures the player only loses up to a certain amount of speed per collision.
                if (result < 0)
                {
                    result = Mathf.Max(result, -SnowballSpeed.TargetValue * maxDamageProportion);
                }
                speedSetter.TargetValue_Local += result;

                // Speed kickback.
                result = SpeedCollisionCurve(obstacleSize, snowballSize, knockbackCurveScale, knockbackCurveSteepness);
                if (obstacleSize > snowballSize)
                {
                    // If the obstacle is larger than the snowball, then a fixed kickback is applied so that the player
                    // always has some room to move around the obstacle.
                    result = Mathf.Min(-minDamageKnockback, speedSetter.Value_Local * result);
                }
                else
                {
                    // If the snowball was larger, then the obstacle is destroyed and we can apply a smaller kickback
                    // since the obstacle is no longer in the way.
                    
                    // Scale the effect on the speed based on our current speed.  Done this way so that if a reult of 0
                    // is returned, then no speed is changed, but we can still have an affect on our speed if its high.
                    result = speedSetter.Value_Local * result;
                }
                speedSetter.Value_Local += result;
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
        #endregion

        private void Awake()
        {
            // Subscribe to our size value's OnChagneEvent so that when size is reduced, we can gain immunity.
            SnowballSize.OnTargetValueChanged += OnSizeChanged;
        }

        private void OnDestroy()
        {
            // Unsubscribe events.
            SnowballSize.OnTargetValueChanged -= OnSizeChanged;
        }

        /// <summary>
        /// When the obstacle collides with the snowball, it will affect the snowball's values and evaluate if it gets
        /// destroyed or not.
        /// </summary>
        /// <param name="collision"></param>
        private void OnTriggerEnter2D(Collider2D collision)
        {
            //Debug.Log(isImmune);
            if (!isImmune && collision.gameObject.TryGetComponent(out ObstacleCollision obstacle) && 
                obstacle.HasCollision)
            {
                // Save the snowball's current size so that any changes to size dont affect any of the other math.
                float snowballSizeVal = SnowballSize.Value;

                //Debug.Log("Collided with " + obstacle.name + " of size " + obstacle.ObstacleSize);

                // Change the player's values based on our result curves defined in the inspector.
                if (!IsInvincible)
                {
                    effectOnSpeed.OnCollision(obstacle.ObstacleSize, snowballSizeVal, speedComp);
                    effectOnSize.OnCollision(obstacle.ObstacleSize, snowballSizeVal, sizeComp);
                }

                if (IsInvincible || snowballSizeVal > obstacle.ObstacleSize)
                {
                    obstacle.DestroyObstacle(obstacle.ObstacleSize / snowballSizeVal);
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
                // Only allow for broadcasts of damage that the snowball can actually take.
                OnTakeDamage?.Invoke(Mathf.Min(Mathf.Abs(change), old));
                GiveImmunity(hitImmunity);
            }
        }

        /// <summary>
        /// Gives this snowball immunity to collisions.
        /// </summary>
        /// <param name="duration">The duration of the immunity to give.</param>
        public void GiveImmunity(float duration)
        {
            if (immunityRoutine != null)
            {
                StopCoroutine(immunityRoutine);
                immunityRoutine = null;
            }

            immunityRoutine = StartCoroutine(ImmunityRoutine(duration));
        }

        /// <summary>
        /// Gives the snowball immunity from collisions for a time after it takes damage from something.
        /// </summary>
        /// <param name="duration"></param>
        /// <returns></returns>
        private IEnumerator ImmunityRoutine(float duration)
        {
            isImmune = true;
            float timer = duration;
            float blinkTimer = flashDelay;
            while (timer > 0)
            {
                // Toggle the snowball's sprite renderer so it blinks.
                if (blinkTimer <= 0)
                {
                    flashRenderer.enabled = !flashRenderer.enabled;
                    blinkTimer = flashDelay;
                }

                timer -= Time.deltaTime;
                blinkTimer -= Time.deltaTime;
                yield return null;
            }
            //yield return new WaitForSeconds(duration);
            flashRenderer.enabled = true;
            isImmune = false;
            immunityRoutine = null;
        }
        #endregion

    }
}
