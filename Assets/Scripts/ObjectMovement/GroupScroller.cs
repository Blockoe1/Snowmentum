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
using System.Linq;
using UnityEngine;

namespace Snowmentum
{
    public class GroupScroller : MonoBehaviour
    {

        [SerializeField, Tooltip("The angle that the snowball moves at, based on the approximate angle of " +
"the hillside.  Should be based on 0 degrees being to the right.")]
        private float moveAngle = 180;
        [SerializeField, Tooltip("How quickly this object should move in comparison to the snowball's speed.  " +
            "Obstacles should have this value set to 1.")]
        protected float speedScale = 1;
        [SerializeField] private List<GroupScrolledObject> objects;
        [SerializeField] private float xLimit;
        //[SerializeField, Tooltip("The x position where the front object has torn from the left side of the screen " +
        //    "and a new front object should be pulled.")] 
        //private float xPull;

        [SerializeField, HideInInspector] protected Vector2 moveVector = Vector2.left;

        /// <summary>
        /// When the value of MoveAngle is changed, we should update our value of MoveVector automatically.
        /// </summary>
        private void OnValidate()
        {
            moveVector = MathHelpers.DegAngleToUnitVector(moveAngle);
        }

        private void Awake()
        {
            // Order our objects by X position, from left to right
            objects = objects.OrderBy(item => item.transform.localPosition.x).ToList();
        }

        /// <summary>
        /// Use the transform of the object to move it if there is no rigidbody attached.
        /// </summary>
        private void LateUpdate()
        {
            int loopCount = 0;
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
                    objects[i].QueryModifiers(ref targetPos, moveVector);

                    // This causes a crash when the player goes down a size bracket.

                    // If the right edge of the leading object is beyond the limit of the screen, it should loop to
                    // the back
                    if (GetEdge(objects[i], Vector3.right, targetPos).x < xLimit)
                    {
                        GroupScrolledObject obj = objects[i];
                        obj.CallObjectLooped();
                        //targetPos = targetPos + ((loopLength) * Math.Sign(targetPos.x) * moveVector);
                        // Moves this object to the end of the objects list.
                        objects.RemoveAt(i);
                        objects.Add(obj);

                        Debug.Log("Push");
                        loopCount++;
                        if (loopCount == 100)
                        {
                            Debug.LogError("Send to back Loop Limit Hit for object " + name);
                            enabled = false;
                            break;
                        }


                        // Decrement i if we loop an object so we dont skip any objects.
                        i--;
                        continue;
                    }
                    // If the left edge of the leading object leaves distance between it and the limit of the screen,
                    // we need to pull the last object to loop in front.
                    else if (GetEdge(objects[i], Vector2.left, targetPos).x > xLimit)
                    {
                        // Pulls the last object in our array and loops it in front of the current leading object.
                        GroupScrolledObject obj = objects[objects.Count - 1];
                        
                        // Need to make sure we immediatley snap the new leading object.
                        obj.transform.localPosition = GetRelativePosition(obj, objects[i], Vector3.left);

                        obj.CallObjectLooped();
                        objects.RemoveAt(objects.Count - 1);
                        objects.Insert(0, obj);

                        Debug.Log("Pull");
                        loopCount++;
                        if (loopCount == 100)
                        {
                            Debug.LogError("Pull Loop Limit Hit for object " + name);
                            enabled = false;
                            break;
                        }

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
                    objects[i].QueryModifiers(ref targetPos, moveVector);

                    targetPos = GetRelativePosition(objects[i - 1], objects[i], Vector3.right);
                }

                objects[i].transform.localPosition = targetPos;
            }
        }

        /// <summary>
        /// Gets the left or right edge of a given GroupScrolledObject.
        /// </summary>
        /// <param name="obj">The object to get the edge of.</param>
        /// <param name="direction">The direction of the edge to get.</param>
        /// <returns>The position of the edge in local space.</returns>
        private static Vector2 GetEdge(GroupScrolledObject obj, Vector3 direction)
        {
            return GetEdge(obj, direction, obj.transform.localPosition);
        }
        private static Vector2 GetEdge(GroupScrolledObject obj, Vector3 direction, Vector3 targetPosition)
        {
            return targetPosition + (obj.Width / 2) * direction;
        }

        /// <summary>
        /// Gets this object's position such that it lines up with the end of a preceeding object.
        /// </summary>
        /// <param name="preceedingObj">The object that this object should be positioned behind.</param>
        /// <param name="thisObj">The object to move.</param>
        /// <returns>The position this object should be at.</returns>
        private static Vector2 GetRelativePosition(GroupScrolledObject preceedingObj, GroupScrolledObject thisObj, Vector3 direction)
        {
            return preceedingObj.transform.localPosition +
                ((preceedingObj.Width + thisObj.Width) / 2) * direction;
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
