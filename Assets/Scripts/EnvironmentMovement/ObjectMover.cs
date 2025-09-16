/*****************************************************************************
// File Name : ObjectMover.cs
// Author : Brandon Koederitz
// Creation Date : 9/13/2025
// Last Modified : 9/16/2025
//
// Brief Description : Moves objects across the screen based on some settings for this type of object.
*****************************************************************************/
using System.Collections.Generic;
using UnityEngine;

namespace Snowmentum
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class ObjectMover : MonoBehaviour
    {
        [SerializeField] protected MovementSettings movementSettings;
        [SerializeField] private ScriptableValue obstacleSpeed;

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

        //#region Properties
        //public float Speed
        //{
        //    get { return speed; }
        //    set { speed = value; }
        //}
        //#endregion

        /// <summary>
        /// Get all components on this object that modify movement so that they can be updated each FixedUpdate.
        /// </summary>
        private void Awake()
        {
            moveModifiers = GetComponents<IMovementModifier>();
            // Give all of our movement modifiers our movement settings automatically.
            foreach(IMovementModifier modifier in moveModifiers)
            {
                modifier.PassSettings(movementSettings);
            }
        }

        /// <summary>
        /// Continually moves the obstacles along their given move angle.
        /// </summary>
        private void FixedUpdate()
        {
            //Debug.Log(name + "" + (settings.MoveVector * obstacleSpeed.Value * Time.fixedDeltaTime));
            // Default movement determined by the speed of the snowball.
            Vector2 targetPos = myRigidbody.position + (movementSettings.MoveVector * obstacleSpeed.Value * Time.fixedDeltaTime);
            // Update the target pos based on overrides of movement modifiers, such as scaling with perspective.
            foreach(IMovementModifier modifier in moveModifiers)
            {
                targetPos = modifier.MoveUpdate(targetPos);
            }
            myRigidbody.MovePosition(targetPos);
        }
    }
}
