/*****************************************************************************
// File Name : SnowballSize.cs
// Author : Brandon Koederitz
// Creation Date : 9/19/2025
// Last Modified : 9/22/2025
//
// Brief Description : Holds the snowball's current size.
*****************************************************************************/
using System;
using UnityEngine;

namespace Snowmentum
{
    public class SnowballSize : SnowballValue
    {
        private static float val;
        private static float targetVal;

        public static event Action<float, float> OnTargetValueChanged;
        public static event Action<float, float> OnValueChanged;

        private static float scalePivotX;

        #region Properties
        public static float Value
        {
            get { return val; }
            private set
            {
                float oldVal = val;
                // Should get to this if both are infinity.
                val = value;
                OnValueChanged?.Invoke(val, oldVal);
            }
        }
        public static float TargetValue
        {
            get { return targetVal; }
            private set
            {
                float oldVal = targetVal;
                // Should get to this if both are infinity.
                targetVal = value;
                OnTargetValueChanged?.Invoke(targetVal, oldVal);
            }
        }
        public static float ScalePivotX => scalePivotX;
        public override float TargetValue_Local { get => TargetValue; set => TargetValue = value; }
        public override float Value_Local { get => Value; set => Value = value; }
        #endregion

        private void OnGUI()
        {
            GUI.TextArea(new Rect(10, 10, 300, 200), TargetValue.ToString() + "\n" + Value.ToString());
        }

        /// <summary>
        /// Resets values on awake
        /// </summary>
        private void Awake()
        {
            // Set the pivot point to the snowball's X position so that obstacles that scale based on perspective
            // scale correctly.
            scalePivotX = transform.position.x;

            TargetValue = startingValue;
            Value = startingValue;
        }

        /// <summary>
        /// Size should lerp towards it's target value so that changes in size arae animated, but very quick.
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
    }
}
