/*****************************************************************************
// File Name : ValueIncrementer.cs
// Author : Brandon Koederitz
// Creation Date : 9/13/2025
// Last Modified : 9/13/2025
//
// Brief Description : Continually updates a scriptable value each frame.
*****************************************************************************/
using UnityEngine;

namespace Snowmentum
{
    public class ValueIncrementer : MonoBehaviour
    {
        [SerializeField, Tooltip("The value that should be incremented each update.")] 
        protected ScriptableValue value;
        [SerializeField] private float startingValue;
        [SerializeField, Tooltip("The amount the ScriptableValue increases each second.")] 
        protected float increasePerSecond;
        [SerializeField] private bool useFixedUpdate;

        /// <summary>
        /// Resets the scriptable value to it's starting value
        /// </summary>
        /// <remarks>
        /// Should run after OnEnable for the ScriptableObject, so it's not in Awake
        /// </remarks>
        private void Start()
        {
            value.Value = startingValue;
        }

        /// <summary>
        /// Continually updates the given value over time.
        /// </summary>
        private void Update()
        {
            if (!useFixedUpdate)
            {
                ApplyChange(Time.deltaTime);
            }
        }

        private void FixedUpdate()
        {
            if (useFixedUpdate)
            {
                ApplyChange(Time.fixedDeltaTime);
            }
        }

        /// <summary>
        /// Applies the change to the value.
        /// </summary>
        /// <param name="time">The time value (ie. Time.deltaTime, Time.fixedDeltaTime) since out last update.</param>
        protected virtual void ApplyChange(float time)
        {
            value.Value += increasePerSecond * time;
        }
    }
}
