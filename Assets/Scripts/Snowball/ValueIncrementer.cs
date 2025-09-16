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
        private ScriptableValue value;
        [SerializeField] private float startingValue;
        [SerializeField, Tooltip("The amount the ScriptableValue increases each second.")] 
        private float increasePerSecond;
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
                value.Value += increasePerSecond * Time.deltaTime;
            }
        }

        private void FixedUpdate()
        {
            if (useFixedUpdate)
            {
                value.Value += increasePerSecond * Time.fixedDeltaTime;
            }
        }
    }
}
