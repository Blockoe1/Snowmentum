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
        /// Allows for setting of the TargetValue and Value of this SnwoballValue through a reference to the snowball.
        /// </summary>
        /// <param name="value"></param>
        public abstract float TargetValue_Local { get; set; }
        public abstract float Value_Local { get; set; }

        /// <summary>
        /// Applies changes made to this value over time.
        /// </summary>
        /// <param name="time">The time elapsed since the last update (ie. Time.deltaTime, Time.fixedDeltaTile)</param>
        public virtual void TimedUpdate(float time)
        {
            if (increasePerSecond > 0)
            {
                TargetValue_Local += increasePerSecond * time;
            }
            MoveToTarget();
        }

        /// <summary>
        /// Moves the current actual value towards the target value.  Called in TimedUpdate
        /// </summary>
        protected abstract void MoveToTarget();

        /// <summary>
        /// Adds a certain amount to the TargetValue.  
        /// </summary>
        /// <remarks>
        /// Useful for adding extra size from UnityEvents.
        /// </remarks>
        /// <param name="toAdd">The value to add.</param>
        public abstract void AddTargetValue(float toAdd);
    }
}
