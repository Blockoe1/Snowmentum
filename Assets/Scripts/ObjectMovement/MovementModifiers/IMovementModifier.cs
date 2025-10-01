/*****************************************************************************
// File Name : IMovementModifier.cs
// Author : Brandon Koederitz
// Creation Date : 9/16/2025
// Last Modified : 9/22/2025
//
// Brief Description : Interface for components that modify an object's movement, such as scaling with perspective.
*****************************************************************************/

using UnityEngine;

namespace Snowmentum
{
    public interface IMovementModifier
    {
        Vector2 MoveUpdate(Transform movedObject, Vector2 targetPos, Vector2 moveVector);
    }
}
