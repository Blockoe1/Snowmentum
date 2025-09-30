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

        private static float size;
        private static float targetSize;

        public static event Action<float, float> OnTargetValueChanged;
        public static event Action<float, float> OnValueChanged;

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

                // Update our size bracket.
                SizeBracket.UpdateBracket(targetSize);
            }
        }

        public override float TargetValue_Local { get => TargetValue; set => TargetValue = value; }
        public override float Value_Local { get => Value; set => Value = value; }
        #endregion

        /// <summary>
        /// Resets values on start (so that things can subscribe to events on awake)
        /// </summary>
        private void Awake()
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
    }
}
