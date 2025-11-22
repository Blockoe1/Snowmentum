/*****************************************************************************
// File Name : ObstacleCollision.cs
// Author : Brandon Koederitz
// Creation Date : 9/16/2025
// Last Modified : 9/18/2025
//
// Brief Description : Controls the result of a snowball's collision with an obstacle.  Goes on the snowball.
*****************************************************************************/
using Snowmentum.Size;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Snowmentum
{
    [RequireComponent(typeof(Collider2D))]
    public class SnowballCollision : MonoBehaviour
    {
        [SerializeField, Tooltip("Controls how long the snowball stays immune to collisions after being hit.")] 
        private float hitImmunity;
        [SerializeField] private float flashDelay;

        [SerializeField] private SizeDamage sizeDamage;
        [SerializeField] private SizeGaining sizeGaining;
        [SerializeField] private SpeedDamage speedDamage;
        [SerializeField] private SpeedKnockback speedKnockback;

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

        #region Collision Formulas
        #region Base
        [System.Serializable]
        private abstract class CollisionFormula
        {
            [SerializeField, Tooltip("The steepness of the curve.  Higher numbers will result in harsher punishments" +
                " for colliding with objects that are smaller than you."), Min(1.001f)]
            protected float curveSteepness = 2;

            internal abstract float Evaluate(float obstacleSize, float snowballSize);
        }

        [System.Serializable]
        private abstract class SizeCollisionFormula : CollisionFormula
        {
            [SerializeField, Tooltip("The maximum positive value this curve can return.")]
            protected float maxGain;
            [SerializeField, Tooltip("If true, then the maxGain parameter will be overwritten by the size of " +
                "the obstacle, and the maxGain value will be used as a multiplier.")]
            protected bool scaleWithObstacleSize;

            /// <summary>
            /// The formula
            /// -curveSteepness ^ (-snowballSize + obstacleSize + logbase[curveSteepness](snowballSize)) + snowballSize
            /// that is used to calculate the change to snowball size on collision.
            /// </summary>
            /// <param name="obstacleSize"></param>
            /// <param name="snowballSize"></param>
            /// <returns></returns>
            internal override float Evaluate(float obstacleSize, float snowballSize)
            {
                float gain = scaleWithObstacleSize ? obstacleSize * maxGain : maxGain;
                return -Mathf.Pow(curveSteepness, -snowballSize + obstacleSize +
                    Mathf.Log(gain, curveSteepness)) + gain;
            }
        }

        [System.Serializable]
        private abstract class SpeedCollisionFormula : CollisionFormula
        {
            [SerializeField, Tooltip("The scale of this curve.  The negative value that is returned when a " +
                "collision happens between two objects of the same size will be equal to this."), Min(0.01f)]
            protected float curveScale = 0.01f;

            /// <summary>
            /// The formula
            /// -(curveSteepness ^ (-snowballSize + obstacleSize)) * curveScale,
            /// which is used for determining changes to speed.
            /// </summary>
            /// <param name="obstacleSize"></param>
            /// <param name="snowballSize"></param>
            /// <returns></returns>
            internal override float Evaluate(float obstacleSize, float snowballSize)
            {
                return -Mathf.Pow(curveSteepness, (-snowballSize + obstacleSize)) * curveScale;
            }
        }
        #endregion
        /// <summary>
        /// Controls damage dealt to the snowball.
        /// </summary>
        [System.Serializable]
        private class SizeDamage : SizeCollisionFormula
        {
            [SerializeField, Tooltip("The maximum proportion of this target value that can be lost from a " +
                "collision, unless it is reduced to less than 0.  Set to 1 to have no max value."), Range(0f, 1f)]
            protected float maxDamageProportion;
            [Header("Death Protection")]
            [SerializeField, Tooltip("The number of times the snowball is prevented from dying.")]
            private int lives;
            [SerializeField, Tooltip("If true, then the snowball \"loses a life\" whenever they take any damage, " +
                "even if it didn't kill them.")]
            private bool consumeOnHit;

            internal override float Evaluate(float obstacleSize, float snowballSize)
            {
                float result = base.Evaluate(obstacleSize, snowballSize);

                // Ensures the player only takes a certain amount of damage if they are not one-shot
                if (Mathf.Abs(result) < SnowballSize.Value)
                {
                    result = Mathf.Max(result, -SnowballSize.Value * maxDamageProportion);
                    if (consumeOnHit)
                    {
                        lives--;
                    }
                }
                // If the player does take fatal damage, use a fatalResist instead and take only the max
                // damage proportion.
                else if (lives > 0)
                {
                    result = Mathf.Max(result, -SnowballSize.Value * maxDamageProportion);
                    lives--;
                }

                return result;
            }
        }

        /// <summary>
        /// Controls gaining size when you destroy an obstacle.
        /// </summary>
        [System.Serializable]
        private class SizeGaining : SizeCollisionFormula
        {
            /// <summary>
            /// Invert and subtract the evaluated curve to make it so that you gain more size the closer you are to
            /// the obstacle's size.
            /// </summary>
            /// <param name="obstacleSize"></param>
            /// <param name="snowballSize"></param>
            /// <returns></returns>
            internal override float Evaluate(float obstacleSize, float snowballSize)
            {
                float gain = scaleWithObstacleSize ? obstacleSize * maxGain : maxGain;
                Debug.Log(-(base.Evaluate(obstacleSize, snowballSize) - gain));
                return -(base.Evaluate(obstacleSize, snowballSize) - gain);
            }
        }

        /// <summary>
        /// Manages losing speed when you hit an obstacle.
        /// </summary>
        [System.Serializable]
        private class SpeedDamage : SpeedCollisionFormula
        {
            [SerializeField, Tooltip("The maximum proportion of this target value that can be lost from a " +
                "collision, unless it is reduced to less than 0.  Set to 1 to have no max value."), Range(0f, 1f)]
            protected float maxDamageProportion;

            internal override float Evaluate(float obstacleSize, float snowballSize)
            {
                // Target speed reduction.  Only reduced if the snowball is smaller.
                float result = base.Evaluate(obstacleSize, snowballSize);
                // Ensures the player only loses up to a certain amount of speed per collision.
                if (result < 0)
                {
                    result = Mathf.Max(result, -SnowballSpeed.TargetValue * maxDamageProportion);
                }
                return result;
            }

        }

        [System.Serializable]
        private class SpeedKnockback : SpeedCollisionFormula
        {
            [SerializeField, Tooltip("The minimum knockback applied to the snowball when the obstacle is not " +
                "destroyed."), Min(0f)]
            private float minDamageKnockback;

            private float storedSpeed;

            public void SetStoredSpeed(float ss)
            {
                storedSpeed = ss;
            }

            internal override float Evaluate(float obstacleSize, float snowballSize)
            {
                // Speed kickback.
                float result = base.Evaluate(obstacleSize, snowballSize);
                result = Mathf.Min(-minDamageKnockback, storedSpeed * result);
                return result;
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
            if (collision.gameObject.TryGetComponent(out ObstacleCollision obstacle) && 
                obstacle.HasCollision)
            {
                // Save the snowball's current size so that any changes to size dont affect any of the other math.
                float snowballSizeVal = SnowballSize.Value;

                //Debug.Log("Collided with " + obstacle.name + " of size " + obstacle.ObstacleSize);

                // Change the player's values based on our result curves defined in the inspector.
                if (!IsInvincible && !isImmune)
                {
                    // Apply damage and negative effects
                    if (snowballSizeVal < obstacle.ObstacleSize)
                    {
                        speedComp.TargetValue_Local += speedDamage.Evaluate(obstacle.ObstacleSize, snowballSizeVal);
                        speedKnockback.SetStoredSpeed(speedComp.Value_Local);
                        speedComp.Value_Local += speedKnockback.Evaluate(obstacle.ObstacleSize, snowballSizeVal);
                        sizeComp.TargetValue_Local += sizeDamage.Evaluate(obstacle.ObstacleSize, snowballSizeVal);
                    }
                    // Apply positive benefits.
                    else
                    {
                        sizeComp.TargetValue_Local += sizeGaining.Evaluate(obstacle.ObstacleSize, snowballSizeVal);
                    }

                    //    speedDamage.OnCollision(obstacle.ObstacleSize, snowballSizeVal, speedComp);
                    //sizeDamage.OnCollision(obstacle.ObstacleSize, snowballSizeVal, sizeComp);
                }

                if (IsInvincible || snowballSizeVal > obstacle.ObstacleSize)
                {
                    obstacle.DestroyObstacle(obstacle.ObstacleSize / snowballSizeVal);
                }
                else if (!isImmune)
                {
                    obstacle.OnDealDamage(obstacle.ObstacleSize / snowballSizeVal);
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
