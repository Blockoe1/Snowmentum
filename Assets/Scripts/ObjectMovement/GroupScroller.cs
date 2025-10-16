/*****************************************************************************
// File Name : GroupObjectMover.cs
// Author : Brandon  Koederitz
// Creation Date : 9/30/2025
// Last Modified : 10/1/2025
//
// Brief Description : Moves a group of objects scroll and loop across the screen.  Makes the assumption that the
// objects are scrolling left.
*****************************************************************************/
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.Events;

namespace Snowmentum
{
    public class GroupScroller : ObjectMoverBase
    {
        [SerializeField] private List<GroupScrolledObject> objects;
        [SerializeField] private float xLimit;

        protected override void Awake()
        {
            base.Awake();
            // Order our objects by X position, from left to right
            objects = objects.OrderBy(item => item.transform.localPosition.x).ToList();
        }

        /// <summary>
        /// Use the transform of the object to move it if there is no rigidbody attached.
        /// </summary>
        private void LateUpdate()
        {
            for (int i = 0; i < objects.Count; i++)
            {

                if (objects[i] == null) { continue; }
                Vector2 targetPos;
                if (i == 0)
                {
                    // Move the leading object up normally.
                    targetPos = (Vector2)objects[i].transform.localPosition +
                        (SnowballSpeed.Value * speedScale * Time.deltaTime * moveVector);

                    // Get modification from our movement modifiers.
                    QueryModifiers(objects[i].transform, ref targetPos);

                    // Loops the leading object if it exceeds the xLimit
                    if (targetPos.x < xLimit)
                    {
                        GroupScrolledObject obj = objects[i];
                        obj.CallObjectLooped();
                        //targetPos = targetPos + ((loopLength) * Math.Sign(targetPos.x) * moveVector);
                        // Moves this object to the end of the objects list.
                        objects.RemoveAt(i);
                        objects.Add(obj);
                        // Decrement i if we loop an object so we dont skip any objects.
                        i--;
                        continue;
                    }
                }
                else
                {
                    // All other obstacles shoudl follow after the one in front of them
                    targetPos = Vector2.zero;

                    // Get modification from our movement modifiers.
                    QueryModifiers(objects[i].transform, ref targetPos);

                    targetPos = GetPreceedingPosition(objects[i - 1], objects[i]);
                }

                objects[i].transform.localPosition = targetPos;
            }
        }

        /// <summary>
        /// Gets this object's position such that it lines up with the end of a preceeding object.
        /// </summary>
        /// <param name="preceedingObj">The object that this object should be positioned behind.</param>
        /// <param name="thisObj">The object to move.</param>
        /// <returns>The position this object should be at.</returns>
        private Vector2 GetPreceedingPosition(GroupScrolledObject preceedingObj, GroupScrolledObject thisObj)
        {
            return preceedingObj.transform.localPosition +
                ((preceedingObj.Width + thisObj.Width) / 2) * Vector3.right;
        }

        ///// <summary>
        ///// Gets the width of a moved sprite based on the sprite's width and the object's scale.
        ///// </summary>
        ///// <param name="sprite">The sprite to get the width of.</param>
        ///// <returns>The width of the sprite.</returns>
        //private float GetObjectWidth(SpriteRenderer sprite)
        //{
        //    return sprite.size.x * sprite.transform.localScale.x;
        //}
    }
}
