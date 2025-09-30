/*****************************************************************************
// File Name : GroupObjectMover.cs
// Author : Brandon  Koederitz
// Creation Date : 9/30/2025
// Last Modified : 9/30/2025
//
// Brief Description : Moves a group of objects scroll and loop across the screen.
*****************************************************************************/
using System.Collections.Generic;
using UnityEngine;

namespace Snowmentum
{
    public class GroupScroller : ObjectMoverBase
    {
        [SerializeField] private List<Transform> objects;
        [SerializeField] private float objectWidth;

        private void Awake()
        {
            // Order our objects by X position, from left to right

        }

        /// <summary>
        /// Use the transform of the object to move it if there is no rigidbody attached.
        /// </summary>
        private void LateUpdate()
        {
            foreach (Transform t in objects)
            {
                if (t == null) { continue; }
                Vector2 targetPos = (Vector2)t.localPosition +
                    (SnowballSpeed.Value * speedScale * Time.deltaTime * moveVector);

                // Get modification from our movement modifiers.
                QueryModifiers(ref targetPos);
                t.localPosition = targetPos;
            }
        }

        /// <summary>
        /// Adds an object to the end of this group of objects.
        /// </summary>
        /// <param name="toAppend">The object to add to the back of this group.</param>
        public void AppendObjects(Transform toAppend)
        {

        }
    }
}
