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
        [SerializeField] protected ObstacleSettings settings;
        [SerializeField] private ScriptableValue obstacleSpeed;

        #region Component References
        [Header("Components")]
        [SerializeReference] protected Rigidbody2D rb;

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
            //Debug.Log(name + "" + (settings.MoveVector * obstacleSpeed.Value * Time.fixedDeltaTime));
            // Default movement determined by the speed of the snowball.
            Vector2 targetPos = rb.position + (settings.MoveVector * obstacleSpeed.Value * Time.fixedDeltaTime);
            // Update the target pos based on overrides of child classes.
            targetPos = MoveUpdate(targetPos);
            rb.MovePosition(targetPos);
        }

        /// <summary>
        /// Loop for determining updates to the player's position each physics update.
        /// </summary>
        /// <param name="targetPos">The current target position that the object is trying to move to.</param>
        /// <returns>The new target position after the update.</returns>
        protected virtual Vector2 MoveUpdate(Vector2 targetPos)
        {
            // By default, just pass target pos back in.
            return targetPos;
        }
    }
}
