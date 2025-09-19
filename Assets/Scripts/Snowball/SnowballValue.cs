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
    public abstract class SnowballValue : MonoBehaviour
    {
        [Header("Growth Settings")]
        [SerializeField] protected float startingValue;
        [SerializeField, Tooltip("The amount the target value increases each second.")]
        protected float increasePerSecond;
        [SerializeField, Tooltip("The speed at which this value's actual value moves towards the target value.")] 
        protected float moveToTargetSpeed;
        [SerializeField] private bool useFixedUpdate;
        [Header("Collision Curve Settings")]
        [SerializeField, Tooltip("The maximum proportion of this target value that can be lost from a " +
            "collision, unless it is reduced to less than 0.  Set to 1 to have no max value."), Range(0f, 1f)]
        protected float maxDamageProportion;

        #region Properties

        #endregion

        /// <summary>
        /// Continually updates the given value over time.
        /// </summary>
        private void Update()
        {
            if (!useFixedUpdate)
            {
                TimedUpdate(Time.deltaTime);
            }
        }

        private void FixedUpdate()
        {
            if (useFixedUpdate)
            {
                TimedUpdate(Time.fixedDeltaTime);
            }
        }

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
    }
}
