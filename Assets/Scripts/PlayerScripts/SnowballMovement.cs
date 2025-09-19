/*****************************************************************************
// File Name : SnowballMovement.cs
// Author : Jack Fisher
// Creation Date : September 17th, 2025
// Last Modified : September 19th, 2025
//
// Brief Description : As of script creation this script just handles player movement. 
// We can expand this script to handle more parts of the snowball if needed though. 
*****************************************************************************/
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;


namespace Snowmentum
{
    public class SnowballMovement : MonoBehaviour
    {
        //We can change this during gameplay in order to make it harder to move the snowball as it grows in size
        // And also adjust it as needed to make it feel responsive enough
        [SerializeField] private float baseMovementSensitivity;
        //[SerializeField, Tooltip("The maximum mouseDelta that will be read in one frame.  Done to prevent massive" +
        //    " forces from being applied at the start of the game when delta is tracked across a long laggy frame.")]
        //private int maxDelta = 100;

        //values used for clamping the position of the snowball
        [SerializeField] private float acceleration;
        [SerializeField] private float minY;
        [SerializeField] private float maxY;
        [SerializeField] private int debug_targetFrameRate;

        [SerializeField] private PlayerInput playerInput;

        private InputAction mouseMovement;

        private Vector2 mouseDelta;
        private Vector2 targetVelocity;

        //used for Player Movement
        private Rigidbody2D snowballRigidbody;

        private void Start()
        {
            Application.targetFrameRate = debug_targetFrameRate;

            //set the player input active
            playerInput = GetComponent<PlayerInput>();
            playerInput.currentActionMap.Enable();
            mouseMovement = playerInput.currentActionMap.FindAction("MouseMovement");
            //mouseMovement.started += Handle_SnowballMouseMovement;

            snowballRigidbody = GetComponent<Rigidbody2D>();

            Cursor.lockState = CursorLockMode.Locked;
            
        }

        //private void OnDestroy()
        //{
        //    mouseMovement.started -= Handle_SnowballMouseMovement;
        //}

        void FixedUpdate()
        {
            //Debug.Log(mouseDelta + "And actual value is " + mouseMovement.ReadValue<Vector2>());
            //will keep track of the mouse and move the snowball accordingly each frame
            ApplyMovementForce();

            //this should clamp the snowball and prevent it from moving off the screen, but currently the snowball is just teleporting between the two positions
            //transform.position = new Vector3(Mathf.Clamp(transform.position.y, minY, maxY), transform.position.x);
            ClampY();
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

        private void Update()
        {
            // Need to read input in Update to ensure that we're using the right time value.
            // Use Time.deltaTime here because mouseDelta records delta since last update.
            // If our last update was longer ago, we'll need to average out our delta into pixels / second.
            mouseDelta = mouseMovement.ReadValue<Vector2>() / Time.deltaTime;
            //targetVelocity = Vector2.up * baseMovementSensitivity * mouseDelta.y;
        }

        //this function moves the snowball up and down in accordance with the movement of the mouse
        private void ApplyMovementForce()
        {
            //Debug.Log(mouseDelta);
            snowballRigidbody.AddForce(Vector2.up * baseMovementSensitivity * mouseDelta.y);

            //Old movement that used transform.translate. Can probably remove but leaving it for now just in case, I guess
            //transform.Translate(Vector3.up * mouseDelta.y * movementSensitivity);
        }

        /// <summary>
        /// Clamps the Y position of the snowball between the min and max values.
        /// </summary>
        private void ClampY()
        {
            float yPos = Mathf.Clamp(snowballRigidbody.position.y, minY, maxY);
            snowballRigidbody.position = new Vector2(snowballRigidbody.position.x, yPos);
        }
    }
}
