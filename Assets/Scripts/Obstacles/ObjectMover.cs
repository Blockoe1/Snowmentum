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
        [SerializeField] ObstacleSettings settings;
        [SerializeField] private ScriptableValue obstacleSpeed;

        #region Component References
        [Header("Components")]
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
        /// Continually moves the obstacles along their given move angle.
        /// </summary>
        private void FixedUpdate()
        {
            rb.linearVelocity = settings.MoveVector * obstacleSpeed.Value;
        }
    }
}
