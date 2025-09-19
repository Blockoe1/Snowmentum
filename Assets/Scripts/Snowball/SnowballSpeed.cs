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

        #region Properties
        public static float Value
        {
            get { return val; }
            private set
            {
                float oldVal = val;
                // Should get to this if both are infinity.
                val = value;

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
        private void Awake()
        {
            TargetValue = startingValue;
            Value = startingValue;
        }


        /// <summary>
        /// Accelerates the snowball towards the target speed.
        /// </summary>
        protected override void MoveToTarget()
        {
            Value = Mathf.MoveTowards(Value, TargetValue, moveToTargetSpeed);
        }
    }
}
