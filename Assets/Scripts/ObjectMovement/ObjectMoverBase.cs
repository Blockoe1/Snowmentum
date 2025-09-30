/*****************************************************************************
// File Name : ObjectMoverBase.cs
// Author : Brandon Koederitz
// Creation Date : 9/30/2025
// Last Modified : 9/30/2025
//
// Brief Description : Base class for object movers that holds values that all variants need.
*****************************************************************************/
using UnityEngine;

namespace Snowmentum
{
    public abstract class ObjectMoverBase : MonoBehaviour
    {
        [SerializeField, Tooltip("The angle that the snowball moves at, based on the approximate angle of " +
        "the hillside.  Should be based on 0 degrees being to the right.")]
        private float moveAngle = 180;
        [SerializeField, Tooltip("How quickly this object should move in comparison to the snowball's speed.  " +
            "Obstacles should have this value set to 1.")]
        protected float speedScale = 1;

        [SerializeField, HideInInspector] protected Vector2 moveVector = Vector2.left;

        private IMovementModifier[] moveModifiers;

        /// <summary>
        /// When the value of MoveAngle is changed, we should update our value of MoveVector automatically.
        /// </summary>
        private void OnValidate()
        {
            moveVector = MathHelpers.DegAngleToUnitVector(moveAngle);
        }


        /// <summary>
        /// Get all components on this object that modify movement so that they can be updated each FixedUpdate.
        /// </summary>
        private void Awake()
        {
            moveModifiers = GetComponents<IMovementModifier>();
        }

        /// <summary>
        /// Queries all of our movement modifiers and applies changes to the target position based on them.
        /// </summary>
        /// <param name="targetPos"></param>
        protected void QueryModifiers(ref Vector2 targetPos)
        {
            if (moveModifiers == null) { return; }
            foreach (IMovementModifier modifier in moveModifiers)
            {
                if (modifier == null) { continue; }
                targetPos = modifier.MoveUpdate(targetPos, moveVector);
            }
        }
    }
}
