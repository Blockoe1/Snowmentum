/*****************************************************************************
// File Name : EditableSO.cs
// Author : Brandon Koederitz
// Creation Date : 10/25/2025
// Last Modified : 10/25/2025
//
// Brief Description : Allows modiftying a scriptable object reference's values from an object reference field.
*****************************************************************************/
using System;
using UnityEngine;

namespace Snowmentum
{
    [AttributeUsage(AttributeTargets.Field)]
    public class ShowNestedEditor : PropertyAttribute
    {
        
    }
}
