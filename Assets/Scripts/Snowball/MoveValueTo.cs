/*****************************************************************************
// File Name : MoveValueTo.cs
// Author : Brandon Koederitz
// Creation Date : 9/18/2025
// Last Modified : 9/18/2025
//
// Brief Description : Moves a given ScriptableValue towards another.  Used for accelerating speed up to 
// our targetSpeed
*****************************************************************************/
using UnityEngine;

namespace Snowmentum
{
    public class MoveValueTo : ValueIncrementer
    {
        [SerializeField, Tooltip("The target value that we should move our other value towards.")]
        private ScriptableValue targetValue;

        /// <summary>
        /// Have the value MoveTowards the target value instead of simply incrementing it.
        /// </summary>
        /// <param name="time"></param>
        protected override void ApplyChange(float time)
        {
            value.Value = Mathf.MoveTowards(value.Value, targetValue.Value, increasePerSecond);
        }
    }
}
