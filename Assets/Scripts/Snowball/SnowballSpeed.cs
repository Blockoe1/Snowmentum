/*****************************************************************************
// File Name : SnowballSpeed.cs
// Author : Brandon Koederitz
// Creation Date : 9/19/2025
// Last Modified : 9/19/2025
//
// Brief Description : From a gameplay standpoint, it controls the speed that the snowball moves down the hill.
// In actuality, it controls the speed that obstacles move towards the snowball.
*****************************************************************************/
using System;
using UnityEngine;

namespace Snowmentum
{
    public class SnowballSpeed : SnowballValue
    {
        [SerializeField, Tooltip("The minimum target speed that the snowball can be at.")]
        private float minSpeed;

        private static float val;
        private static float targetVal;

        public static event Action<float, float> OnTargetValueChanged;
        public static event Action<float, float> OnValueChanged;

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

        public override float TargetValue_Local
        {
            get => TargetValue;
            set
            {
                TargetValue = Mathf.Max(value, minSpeed);
            }
        }
        public override float Value_Local { get => Value; set => Value = value; }
        #endregion

        /// <summary>
        /// Resets values on awake
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
            targetVal = 0;
            val = 0;
        }


        /// <summary>
        /// Accelerates the snowball towards the target speed.
        /// </summary>
        /// <param name="timeDelta">The time delta of this update.</param>
        protected override void MoveToTarget(float timeDelta)
        {
            if (!MathHelpers.ApproximatelyWithin(Value, TargetValue))
            {
                Value = Mathf.MoveTowards(Value, TargetValue, moveToTarget * timeDelta);
            }
        }

        /// <summary>
        /// Adds a certain amount to this value's target value.
        /// </summary>
        /// <param name="toAdd"></param>
        public override void AddTargetValue(float toAdd)
        {
            TargetValue += toAdd;
        }

        #region Debug
        //private void OnGUI()
        //{
        //    GUI.TextArea(new Rect(10, 110, 100, 100), "Snowball Speed: \n" + TargetValue.ToString() + "\n" + 
        //        Value.ToString());
        //}
        #endregion
    }
}
