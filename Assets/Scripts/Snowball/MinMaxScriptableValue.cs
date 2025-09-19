/*****************************************************************************
// File Name : MinScriptableValue.cs
// Author : Brandon Koederitz
// Creation Date : 9/18/2025
// Last Modified : 9/18/2025
//
// Brief Description : A ScriptableValue with a minimum value.
*****************************************************************************/
using UnityEngine;

namespace Snowmentum
{
    [CreateAssetMenu(fileName = "MinScriptableValue", menuName = "ScriptableObjects/ScriptableValues/MinScriptableValue")]
    public class MinScriptableValue : ScriptableValue
    {
        [SerializeField] private float min;
        public override float Value
        {
            set
            {
                base.Value = Mathf.Max(value, min);
            }
        }
    }
}
