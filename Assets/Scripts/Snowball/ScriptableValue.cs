/*****************************************************************************
// File Name : ScriptableValue.cs
// Author : Brandon Koederitz
// Creation Date : 9/13/2025
// Last Modified : 9/19/2025
//
// Brief Description : Controls a value accessible to all objects by reference to this ScriptableObject.
*****************************************************************************/
using System;
using UnityEngine;

namespace Snowmentum
{
    [CreateAssetMenu(fileName = "ScriptableValue", menuName = "ScriptableObjects/ScriptableValues/Default")]
    public class ScriptableValue : ScriptableObject
    {
        [SerializeField] private float val;
        [SerializeField] private float targetVal;
        [Header("Growth Settings")]
        [SerializeField] private float startingValue;
        [SerializeField, Tooltip("The amount the target value increases each second.")]
        protected float increasePerSecond;
        [SerializeField, Tooltip("The speed at which this value's actual value moves towards the target value.")] 
        protected float moveToTargetSpeed;
        [SerializeField] private bool useFixedUpdate;
        [Header("Collision Curve Settings")]
        [SerializeField, Tooltip("The maximum proportion of this target value that can be lost from a " +
            "collision, unless it is reduced to less than 0.  Set to 1 to have no max value."), Range(0f, 1f)]
        protected float maxDamageProportion;


        public event Action<float, float> OnTargetValueChanged;

        #region Properties
        public bool UseFixedUpdate => useFixedUpdate;
        public virtual float Value
        {
            get { return val; }
            set
            {
                float oldVal = val;
                // Should get to this if both are infinity.
                val = value;
          
            }
        }
        public virtual float TargetValue
        {
            get { return targetVal; }
            set
            {
                float oldVal = targetVal;
                // Should get to this if both are infinity.
                targetVal = value;
                OnTargetValueChanged?.Invoke(targetVal, oldVal);
            }
        }

        #endregion

                /// <summary>
                /// Reset this value when it is first loaded so that it doesnt save values.
                /// </summary>
        private void OnEnable()
        {
            TargetValue = startingValue;
            Value = startingValue;
        }

        /// <summary>
        /// Call the Value setter when we modify the value of val in the inspector.
        /// </summary>
        //private void OnValidate()
        //{
        //    Value = val;

        //}

        /// <summary>
        /// Applies changes made to this value over time.
        /// </summary>
        /// <param name="time">The time elapsed since the last update (ie. Time.deltaTime, Time.fixedDeltaTile)</param>
        public virtual void TimedUpdate(float time)
        {
            TargetValue += increasePerSecond * time;
            MoveToTarget();
        }

        /// <summary>
        /// Moves the current actual value towards the target value.  Called in TimedUpdate
        /// </summary>
        protected virtual void MoveToTarget()
        {
            Value = Mathf.MoveTowards(Value, TargetValue, moveToTargetSpeed);
        }

        /// <summary>
        /// Handles what happens to this value when it is affected by a collision.
        /// </summary>
        /// <param name="obstacleSize"></param>
        /// <param name="snowballSize"></param>
        public virtual void OnCollision(float obstacleSize, float snowballSize)
        {
            // Nothing happens by default.
        }
    }
}
