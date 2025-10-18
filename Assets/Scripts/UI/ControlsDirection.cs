/*****************************************************************************
// File Name : ControlsDirection.cs
// Author :
// Creation Date : 
// Last Modified : 
//
// Brief Description : 
*****************************************************************************/
using System;
using UnityEngine;

namespace Snowmentum
{
    [Flags]
    public enum ControlsDirection
    {
        None = 0,
        Left = 1, // 1
        Right = 2, // 2
        Up = 4, // 4
        Down = 8 // 8
    }
}
