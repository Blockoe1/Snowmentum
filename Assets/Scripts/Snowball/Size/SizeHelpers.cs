/*****************************************************************************
// File Name : SizeHelpers.cs
// Author :
// Creation Date : 
// Last Modified : 
//
// Brief Description : 
*****************************************************************************/
using UnityEngine;

namespace Snowmentum
{
    public static class SizeHelpers
    {
        /// <summary>
        /// Calculates the position of an object when scaled about a given pivot point.
        /// </summary>
        /// <param name="pivot">The pivot point the obstacle should be scaled based on.</param>
        /// <param name="oldScale">The old scale of the obstacle/</param>
        /// <param name="newScale">The new scale of the obstacle.</param>
        /// <returns>The new position of the object.</returns>
        public static Vector3 CalculateScaledPosition(Vector2 pivot, Vector2 currentPos, float oldScale, float newScale)
        {
            if (newScale == Mathf.Infinity || oldScale == Mathf.Infinity || oldScale == 0)
            {
                // If either scale invalid, then we should skip any scaling because values of infinity or 0 will
                // make the math break.
                return currentPos;
            }

            // Calculates the vector pointing from the pivot point to the current position.
            Vector2 pivotToCurrent = currentPos - pivot;
            // Calculates the change in scale between the old and new scales.  Used to determine how much to scale
            // the position by.
            float scaleFactor = newScale / oldScale;
            //Calculates the final position of the obstacle by scaling our pivotToCurrent vector by our scaleFactor.
            Vector3 finalPosition = pivot + (pivotToCurrent * scaleFactor);
            // update our target position to account for changes in scale.
            return finalPosition;
        }
    }
}
