/*****************************************************************************
// File Name : ObjectMover.cs
// Author : Brandon Koederitz
// Creation Date : 9/13/2025
// Last Modified : 9/16/2025
//
// Brief Description : Moves objects across the screen based on some settings for this type of object.
*****************************************************************************/
using UnityEngine;

namespace Snowmentum
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class ObjectMover : MonoBehaviour
    {
        [SerializeField, Tooltip("The angle that the snowball moves at, based on the approximate angle of " +
            "the hillside.  Should be based on 0 degrees being to the right.")]
        private float moveAngle;
        [SerializeField, Tooltip("How quickly this object should move in comparison to the snowball's speed.  " +
            "Obstacles should have this value set to 1.")]
        private float speedScale = 1;

        [SerializeField, HideInInspector] private Vector2 moveVector;

        private IMovementModifier[] moveModifiers;

        #region Component References
        [Header("Components")]
        [SerializeReference] protected Rigidbody2D myRigidbody;

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
        /// Get all components on this object that modify movement so that they can be updated each FixedUpdate.
        /// </summary>
        private void Awake()
        {
            moveModifiers = GetComponents<IMovementModifier>();
        }

        /// <summary>
        /// Continually moves the obstacles along their given move angle.
        /// </summary>
        private void FixedUpdate()
        {
            // Default movement determined by the speed of the snowball and the speedScale of our settings.
            Vector2 targetPos = myRigidbody.position + 
                (moveVector * SnowballSpeed.Value * Time.fixedDeltaTime * speedScale);
            //Debug.Log(SnowballSpeed.Value);

            // Update the target pos based on overrides of movement modifiers, such as scaling with perspective.
            foreach(IMovementModifier modifier in moveModifiers)
            {
                targetPos = modifier.MoveUpdate(targetPos);
            }
            myRigidbody.MovePosition(targetPos);
        }
    }
}
