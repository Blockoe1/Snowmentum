/*****************************************************************************
// File Name : ValueIncrementer.cs
// Author : Brandon Koederitz
// Creation Date : 9/13/2025
// Last Modified : 9/19/2025
//
// Brief Description : Continually updates a scriptable value  over time
*****************************************************************************/
using UnityEngine;

namespace Snowmentum
{
    public class ValueIncrementer : MonoBehaviour
    {
        [SerializeField, Tooltip("The value that should be incremented each update.")] 
        protected ScriptableValue value;

        /// <summary>
        /// Continually updates the given value over time.
        /// </summary>
        private void Update()
        {
            if (!value.UseFixedUpdate)
            {
                value.TimedUpdate(Time.deltaTime);
            }
        }

        private void FixedUpdate()
        {
            if (value.UseFixedUpdate)
            {
                value.TimedUpdate(Time.fixedDeltaTime);
            }
        }
    }
}
