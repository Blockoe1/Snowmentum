/*****************************************************************************
// File Name : SnowballSize.cs
// Author : Brandon Koederitz
// Creation Date : 9/19/2025
// Last Modified : 9/29/2025
//
// Brief Description : Holds the snowball's current size.
*****************************************************************************/
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Snowmentum.Size
{
    public class SnowballSize : SnowballValue
    {
        #region CONSTS
        // The highest possible obstacle / snowball size ratio that can be spawned from the spawner.
        // Used as a baseline when visually scaling obstacles and as an art guideline when creating sprites to ensure
        // we dont get big pixels.
        public const float OBSTACLE_RANGE_SCALE = 2f;
        #endregion

        [Header("Events")]
        [SerializeField] private UnityEvent<float> OnDeathEvent;

        private static float size;
        private static float targetSize;

        public static event Action<float, float> OnTargetValueChanged;
        public static event Action<float, float> OnValueChanged;

        // Not using event here since there should theoretically only be 1 snowball.
        private static Action<float> Internal_OnDeathEvent;

        #region Component References
        [Header("Components")]
        [SerializeReference] protected SnowballSpeed speed;

        /// <summary>
        /// Get components on reset.
        /// </summary>
        [ContextMenu("Get Component References")]
        private void Reset()
        {
            speed = GetComponent<SnowballSpeed>();
        }
        #endregion

        #region Properties
        public static float Value
        {
            get { return size; }
            private set
            {
                float oldVal = size;
                // Should get to this if both are infinity.
                size = value;
                OnValueChanged?.Invoke(size, oldVal);
            }
        }
        public static float TargetValue
        {
            get { return targetSize; }
            private set
            {
                float oldVal = targetSize;
                // Should get to this if both are infinity.
                targetSize = value;
                OnTargetValueChanged?.Invoke(targetSize, oldVal);

                // Check if the snowball was destroyed
                if (value <= 0)
                {
                    // Ntify the snowball object that it died.
                    Internal_OnDeathEvent?.Invoke(oldVal);
                }
                else
                {
                    // Dont update size bracket if the snowball was destroyed so that we dont get visual bugs.
                    SizeBracket.UpdateBracket(targetSize);
                }
            }
        }

        public override float TargetValue_Local { get => TargetValue; set => TargetValue = value; }
        public override float Value_Local { get => Value; set => Value = value; }
        #endregion

        /// <summary>
        /// Setup event references.
        /// </summary>
        protected override void Awake()
        {
            Internal_OnDeathEvent = OnSnowballDeath;
            base.Awake();
        }

        /// <summary>
        /// Resets values
        /// </summary>
        protected override void ResetValues()
        {
            TargetValue = startingValue;
            Value = startingValue;
        }

        /// <summary>
        /// Reset values when the snowball is destroyed.
        /// </summary>
        private void OnDestroy()
        {
            // Modify the values directly instead of through the property since running events on destroy causes
            // erorrs
            targetSize = 0;
            size = 0;
        }

        #region Value Changes
        /// <summary>
        /// Size should lerp towards it's target value so that changes in size arae animated, but very quick.
        /// EnvironmentSize should move linearly towards its target.
        /// </summary>
        /// <param name="timeDelta">The time delta for this update.</param>
        protected override void MoveToTarget(float timeDelta)
        {
            // If our value is close enough to our target value, then we snap it to the value directly
            if (MathHelpers.ApproximatelyWithin(Value, TargetValue))
            {
                Value = TargetValue;
            }
            else
            {
                // Lerp independent of time.
                Value = Mathf.Lerp(Value, TargetValue, 1 - Mathf.Pow(0.5f, Time.deltaTime * moveToTarget));
            }
        }

        /// <summary>
        /// Adds a certain amount to this value's TargetValue.
        /// </summary>
        /// <param name="toAdd"></param>
        public override void AddTargetValue(float toAdd)
        {
            //Debug.Log("Target Value Added");
            TargetValue += toAdd;
        }
        #endregion

        /// <summary>
        /// Called when the snowball's size hits 0.
        /// </summary>
        /// <param name="oldValue"></param>
        private void OnSnowballDeath(float oldValue)
        {
            // Reset speed to 0 so that the screen stops moving 
            speed.TargetValue_Local = 0;
            speed.Value_Local = 0;
            OnDeathEvent?.Invoke(oldValue);
            Destroy(gameObject);
        }

        #region Debug
        [ContextMenu("Half Size")]
        public void HelfSize()
        {
            TargetValue_Local -= 0.5f;
        }
        #endregion
    }
}
