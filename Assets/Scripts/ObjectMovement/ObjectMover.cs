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
    public class ObjectMover : ObjectMoverBase
    {

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
                QueryModifiers(transform, ref targetPos);
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
                QueryModifiers(transform, ref targetPos);
                transform.localPosition = targetPos;
            }
        }


    }
}
