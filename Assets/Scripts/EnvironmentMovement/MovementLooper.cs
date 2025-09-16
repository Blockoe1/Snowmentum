/*****************************************************************************
// File Name : MovementLooper.cs
// Author : Brandon Koederitz
// Creation Date : 9/16/2025
// Last Modified : 9/16/2025
//
// Brief Description : Causes an object to loop back to the other side of the screen when it goes off screen.
*****************************************************************************/
using UnityEngine;

namespace Snowmentum
{
    [RequireComponent(typeof(ObjectMover))]
    public class MovementLooper : MonoBehaviour, IMovementModifier
    {
        [SerializeField, Tooltip("The maximum X position that this object can be at before it loops around.")] 
        private float maxX;

        /// <summary>
        /// Loops the object to the other side of the screen when it exceeds maxX.
        /// </summary>
        /// <param name="targetPos">The target position the object is trying to move to.</param>
        /// <returns>The position after looping has been considered.</returns>
        public Vector2 MoveUpdate(Vector2 targetPos, float moveAngle)
        {
            // If we exceed maxX, then we should loop back to the other side of the screen.
            if (Mathf.Abs(targetPos.x) > maxX)
            {
                // Ensure targetPos is set to be +- maxX
                targetPos.x = Mathf.Clamp(targetPos.x, -maxX, maxX);
                targetPos = GetOppositePos(targetPos, moveAngle);
            }
            return targetPos;
        }

        /// <summary>
        /// Calculates the position that this object should be at when it loops to the other side of the screen, taking
        /// into account the move angle.
        /// </summary>
        /// <param name="currentPos">The current position of our object.</param>
        /// <param name="moveAngle">The angle the object os moving at, relative to 0 degrees = right.</param>
        /// <returns>The position that our object should be at  when it loops.</returns>
        private static Vector2 GetOppositePos(Vector2 currentPos, float moveAngle)
        {

            // Calculate the inner angle of the right triangle whose hypotnuse corresponds to our MoveVector.
            float theta = Mathf.Deg2Rad * (180 - moveAngle);
            Debug.Log(Mathf.Tan(theta));
            float toOppositeX = currentPos.x * 2;
            // Using our move angle, calculate the vector that points from our current position to our looped position.
            Vector2 toLoopedPos = new Vector2(-toOppositeX, -Mathf.Abs(toOppositeX) * Mathf.Tan(theta));

            return currentPos + toLoopedPos;
        }
    }
}
