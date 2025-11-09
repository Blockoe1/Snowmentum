/*****************************************************************************
// File Name : SnowballMovement.cs
// Author : Jack Fisher
// Creation Date : September 17th, 2025
// Last Modified : September 23th, 2025
//
// Brief Description : As of script creation this script just handles player movement. 
// We can expand this script to handle more parts of the snowball if needed though. 
*****************************************************************************/
using Snowmentum.Size;
using UnityEngine;


namespace Snowmentum
{
    public class SnowballMovement : MonoBehaviour
    {
        //We can change this during gameplay in order to make it harder to move the snowball as it grows in size
        // And also adjust it as needed to make it feel responsive enough
        [SerializeField] private float baseMovementSensitivity;
        [SerializeField] private bool scaleWithSnowballSize;
        //[SerializeField, Tooltip("The maximum mouseDelta that will be read in one frame.  Done to prevent massive" +
        //    " forces from being applied at the start of the game when delta is tracked across a long laggy frame.")]
        //private int maxDelta = 100;

        [Header("Movement Restrictions")]
        [SerializeField, Tooltip("The maximum speed the snowball can move at.  Set to 0 for no max speed")] 
        private float maxSpeed;
        [SerializeField] private float minY;
        [SerializeField] private float maxY;

        #region Component References
        [Header("Components")]
        [SerializeReference] protected Rigidbody2D snowballRigidbody;

        /// <summary>
        /// Get components on reset.
        /// </summary>
        [ContextMenu("Get Component References")]
        private void Reset()
        {
            snowballRigidbody = GetComponent<Rigidbody2D>();
        }
        #endregion

        private void Awake()
        {
            //set the player input active
            //playerInput = GetComponent<PlayerInput>();
            //playerInput.currentActionMap.Enable();
            //mouseMovement = playerInput.currentActionMap.FindAction("MouseMovement");
            //mouseMovement.started += Handle_SnowballMouseMovement;

            // Locks the cursor so it's invisible on screen.
            //Cursor.lockState = CursorLockMode.Locked;
        }

        void FixedUpdate()
        {
            //Debug.Log(mouseDelta + "And actual value is " + mouseMovement.ReadValue<Vector2>());
            //will keep track of the mouse and move the snowball accordingly each frame
            ApplyMovementForce();

            // Restrict the snowball's speed if maxSpeed is not set to 0.
            if (!Mathf.Approximately(maxSpeed, 0))
            {
                ClampSpeed(maxSpeed);
            }

            //this should clamp the snowball and prevent it from moving off the screen, but currently the snowball is just teleporting between the two positions
            //transform.position = new Vector3(Mathf.Clamp(transform.position.y, minY, maxY), transform.position.x);
            ClampY();
        }

        //this function moves the snowball up and down in accordance with the movement of the mouse
        private void ApplyMovementForce()
        {
            //Debug.Log(mouseDelta);
            float sizeScale = scaleWithSnowballSize ? SnowballSize.Value : 1f;
            snowballRigidbody.AddForce((baseMovementSensitivity * InputManager.MouseDelta.y / sizeScale) * Vector2.up);

            //Old movement that used transform.translate. Can probably remove but leaving it for now just in case, I guess
            //transform.Translate(Vector3.up * mouseDelta.y * movementSensitivity);
        }

        /// <summary>
        /// Clamps the Y position of the snowball between the min and max values.
        /// </summary>
        private void ClampY()
        {
            float storeY = snowballRigidbody.position.y;
            float yPos = Mathf.Clamp(snowballRigidbody.position.y, minY, maxY);
            snowballRigidbody.position = new Vector2(snowballRigidbody.position.x, yPos);

            // Check to see if our y position was clamped.  It it was, then we should reset velocity back to 0.
            if (storeY != yPos)
            {
                ClampSpeed(0);
            }
        }

        /// <summary>
        /// Clamps the speed of the snowball to a max value.
        /// </summary>
        /// <param name="maxSpeed"></param>
        private void ClampSpeed(float maxSpeed)
        {
            Vector2 velocity = snowballRigidbody.linearVelocity;
            velocity.y = Mathf.Clamp(velocity.y, -maxSpeed, maxSpeed);
            snowballRigidbody.linearVelocity = velocity;
        }

        //private static Vector2 CalculateMouseDelta(Vector2 current, Vector2 old)
        //{
        //    return current - old;
        //}

        /////  Player Movement
        ///// 
        ///// 
        ////this function will track the movement of the mouse and trackball
        ////I set it to public so I could use a feature in the inspector to automatically stop the snowball when the mouse stops moving
        ////hopefully this function being public is fine
        //public void Handle_SnowballMouseMovement(InputAction.CallbackContext context)
        //{

        //    // Ensure MouseDelta doesnt become an absurdly large number to due long framse (Such as script compilation
        //    // at the beginning.)
        //    //mouseDelta.y = Mathf.Clamp(mouseDelta.y, -maxDelta, maxDelta);
        //}


        //private void OnDestroy()
        //{
        //    mouseMovement.started -= Handle_SnowballMouseMovement;
        //}
    }
}
