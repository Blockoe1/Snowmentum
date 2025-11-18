/*****************************************************************************
// File Name : EnvironmentSize.cs
// Author : Brandon Koederitz
// Creation Date : 9/30/2025
// Last Modified : 9/30/2025
//
// Brief Description : Controls the current size of the environment based on the snowball's size.
*****************************************************************************/
using Snowmentum.Size;
using System;
using UnityEngine;

namespace Snowmentum
{
    public class EnvironmentSize : SnowballValue
    {
        #region CONSTS
        public const float HORIZON_LINE = 1.375f;
        #endregion

        private static float val;
        private static float targetVal;

        public static event Action<float, float> OnTargetValueChanged;
        public static event Action<float, float> OnValueChanged;
        public static event Action OnValueTransitionFinish;

        private static Vector2 scalePivot;
        private float moveSpeed;

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
                Debug.Log("Environment size updated to " + TargetValue);
            }
        }

        public static Vector2 ScalePivot => scalePivot;
        public override float TargetValue_Local { get => TargetValue; set => TargetValue = value; }
        public override float Value_Local { get => Value; set => Value = value; }
        #endregion

        /// <summary>
        /// Reset values on start.
        /// </summary>
        protected override void Awake()
        {
            base.Awake();

            // Set the pivot point of the snowball so that obstacles know where to scale based on.
            // Set this whenever the value changes so it's up to date when obstacles need it.
            scalePivot = new Vector2(transform.position.x, HORIZON_LINE);

            SizeBracket.OnBracketChanged += UpdateEnvironmentSize;
        }

        /// <summary>
        /// Resets the environment size
        /// </summary>
        public override void ResetValues()
        {
            TargetValue = startingValue;
            Value = startingValue;
        }

        private void OnDestroy()
        {
            SizeBracket.OnBracketChanged -= UpdateEnvironmentSize;

            // Reset values
            targetVal = 0;
            val = 0;
        }

        /// <summary>
        /// Adds a certain amount to this value's TargetValue.
        /// </summary>
        /// <param name="toAdd"></param>
        public override void AddTargetValue(float toAdd)
        {
            TargetValue += toAdd;
        }

        /// <summary>
        /// Our environemtn size should move towards our target linearly.
        /// </summary>
        /// <param name="timeDelta">The time delta for this update.</param>
        protected override void MoveToTarget(float timeDelta)
        {
            if (!MathHelpers.ApproximatelyWithin(Value, TargetValue))
            {
                Value = Mathf.MoveTowards(Value, TargetValue, moveSpeed * timeDelta);
                if(MathHelpers.ApproximatelyWithin(Value, TargetValue))
                {
                    OnValueTransitionFinish?.Invoke();
                }
            }
        }

        /// <summary>
        /// Updates the current size of the environment based on the minimum size of the current bracket.
        /// </summary>
        /// <param name="bracket"></param>
        private void UpdateEnvironmentSize(int bracket, int oldBracket)
        {
            float oldTarget = TargetValue;
            // The target value of the environment should always be the minimum size of our current bracket.
            TargetValue = SizeBracket.GetMinSize(bracket);

            // Update our move speed based on our old and new target value so that moveToTarget represents the amount
            // of transition time.
            moveSpeed = Mathf.Abs(TargetValue - oldTarget) / moveToTarget;
            //Debug.Log(moveSpeed);
        }
    }
}
