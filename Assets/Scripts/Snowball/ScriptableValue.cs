/*****************************************************************************
// File Name : ScriptableValue.cs
// Author : Brandon Koederitz
// Creation Date : 9/13/2025
// Last Modified : 9/13/2025
//
// Brief Description : Controls a value accessible to all objects by reference to this ScriptableObject.
*****************************************************************************/
using System;
using UnityEngine;

namespace Snowmentum
{
    [CreateAssetMenu(fileName = "ScriptableValue", menuName = "ScriptableObjects/ScriptableValue")]
    public class ScriptableValue : ScriptableObject
    {
        [SerializeField] private float val;


        public event Action<float, float> OnValueChanged;
 
        #region Properties
        public float Value
        {
            get { return val; }
            set
            {
                float oldVal = val;
                // Should get to this if both are infinity.
                val = value;
                OnValueChanged?.Invoke(val, oldVal);
            }
        }
        #endregion

        /// <summary>
        /// Reset this value when it is first loaded so that it doesnt save values.
        /// </summary>
        private void OnEnable()
        {
            Value = 0;
        }

        /// <summary>
        /// Call the Value setter when we modify the value of val in the inspector.
        /// </summary>
        private void OnValidate()
        {
            Value = val;
        }
    }
}
