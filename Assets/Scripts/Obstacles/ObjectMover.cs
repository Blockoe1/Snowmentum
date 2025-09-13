/*****************************************************************************
// File Name : ObjectMover.cs
// Author : Brandon Koederitz
// Creation Date : 9/13/2025
// Last Modified : 9/13/2025
//
// Brief Description : Moves the obstacles towards the snowball.
*****************************************************************************/
using UnityEngine;

namespace Snowmentum
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class ObjectMover : MonoBehaviour
    {
        [SerializeField] private ScriptableValue obstacleSpeed;
        [SerializeField, Tooltip("The angle that the snowball moves at, based on the approximate angle of " +
            "the hillside.  Should be based on 0 degrees being to the right.")] 
        private float moveAngle;

        //private float speed = 5f;
        [SerializeField, HideInInspector] private Vector2 moveVector;

        #region Component References
        [SerializeReference] private Rigidbody2D rb;

        /// <summary>
        /// Get components on reset.
        /// </summary>
        [ContextMenu("Get Component References")]
        private void Reset()
        {
            rb = GetComponent<Rigidbody2D>();
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
        /// When the value of MoveAngle is changed, we should update our value of MoveVector automatically.
        /// </summary>
        private void OnValidate()
        {
            moveVector = MathHelpers.DegAngleToUnitVector(moveAngle);
        }

        /// <summary>
        /// Continually moves the obstacles along their given move angle.
        /// </summary>
        private void FixedUpdate()
        {
            rb.linearVelocity = moveVector * obstacleSpeed.Value;
        }
    }
}
