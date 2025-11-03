/*****************************************************************************
// File Name : ObjectMover.cs
// Author : Brandon Koederitz
// Creation Date : 9/13/2025
// Last Modified : 10/1/2025
//
// Brief Description : Moves objects across the screen based on some settings for this type of object.
*****************************************************************************/
using System.Linq;
using UnityEngine;

namespace Snowmentum
{
    public class ObjectMover : MoveModifierController
    {

        [SerializeField, Tooltip("The angle that the snowball moves at, based on the approximate angle of " +
"the hillside.  Should be based on 0 degrees being to the right.")]
        private float moveAngle = 180;
        [SerializeField, Tooltip("How quickly this object should move in comparison to the snowball's speed.  " +
            "Obstacles should have this value set to 1.")]
        protected float speedScale = 1;

        [SerializeField, HideInInspector] protected Vector2 moveVector = Vector2.left;

        #region Component References
        [Header("Components")]
        [SerializeReference, ReadOnly] protected Rigidbody2D myRigidbody;

        /// <summary>
        /// Get components on reset.
        /// </summary>
        [ContextMenu("Get Component References")]
        private void Reset()
        {
            myRigidbody = GetComponent<Rigidbody2D>();
        }
        #endregion

        /// <summary>
        /// When the value of MoveAngle is changed, we should update our value of MoveVector automatically.
        /// </summary>
        private void OnValidate()
        {
            moveVector = MathHelpers.DegAngleToUnitVector(moveAngle);
        }

        /// <summary>
        /// Continually moves the obstacles along their given move angle.
        /// </summary>
        /// <remarks>
        /// Need to use FixedUpdate because yield return WaitForFixedUpdate happens after the internal physics update
        /// and thus causes wonky behavior.
        /// </remarks>
        private void FixedUpdate()
        {
            if (myRigidbody != null)
            {
                // Default movement determined by the speed of the snowball and the speedScale of our settings.
                Vector2 targetPos = myRigidbody.position +
                    (SnowballSpeed.Value * speedScale * Time.fixedDeltaTime * moveVector);
                //Debug.Log(SnowballSpeed.Value);

                // Update the target pos based on overrides of movement modifiers, such as scaling with perspective.
                QueryModifiers(ref targetPos, moveVector);
                myRigidbody.MovePosition(targetPos);
            }
        }

        /// <summary>
        /// Use the transform of the object to move it if there is no rigidbody attached.
        /// </summary>
        private void LateUpdate()
        {
            if (myRigidbody == null)
            {
                Vector2 targetPos = (Vector2)transform.localPosition +
                    (SnowballSpeed.Value * speedScale * Time.deltaTime * moveVector);

                // Get modification from our movement modifiers.
                QueryModifiers(ref targetPos, moveVector);
                transform.localPosition = targetPos;
            }
        }


    }
}
