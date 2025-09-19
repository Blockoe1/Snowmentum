/*****************************************************************************
// File Name : PlayerController.cs
// Author : Jack Fisher
// Creation Date : September 17th, 2025
// Last Modified : September 19th, 2025
//
// Brief Description : As of script creation this script just handles player movement. 
// We can expand this script to handle more parts of the snowball if needed though. 
*****************************************************************************/
using UnityEngine;
using UnityEngine.InputSystem;


namespace Snowmentum
{
    public class PlayerController : MonoBehaviour
    {
        //We can change this during gameplay in order to make it harder to move the snowball as it grows in size
        // And also adjust it as needed to make it feel responsive enough
        [SerializeField] private float baseMovementSensitivity;

        [SerializeField] private PlayerInput playerInput;
        private InputAction mouseMovement;

        private Vector2 mouseDelta;

        //used for Player Movement
        private Rigidbody2D snowballRigidbody;

        //values used for clamping the position of the snowball
        [SerializeField] private float minY;
        [SerializeField] private float maxY;

        private void Awake()
        {
            //set the player input active
            playerInput = GetComponent<PlayerInput>();
            playerInput.currentActionMap.Enable();
            mouseMovement = playerInput.currentActionMap.FindAction("MouseMovement");
            mouseMovement.started += Handle_SnowballMouseMovement;


            snowballRigidbody = GetComponent<Rigidbody2D>();
            
        }

        private void OnDestroy()
        {
            mouseMovement.started -= Handle_SnowballMouseMovement;
        }

        void FixedUpdate()
        {
            //will keep track of the mouse and move the snowball accordingly each frame
            ApplyMovementForce();

            //this should clamp the snowball and prevent it from moving off the screen, but currently the snowball is just teleporting between the two positions
            //transform.position = new Vector3(Mathf.Clamp(transform.position.y, minY, maxY), transform.position.x);
            ClampY();
        }

        ///  Player Movement
        /// 
        /// 
        //this function will track the movement of the mouse and trackball
        //I set it to public so I could use a feature in the inspector to automatically stop the snowball when the mouse stops moving
        //hopefully this function being public is fine
        public void Handle_SnowballMouseMovement(InputAction.CallbackContext context)
        {
            mouseDelta = context.ReadValue<Vector2>();
            //Debug.Log(mouseDelta);
        }

        //this function moves the snowball up and down in accordance with the movement of the mouse
        private void ApplyMovementForce()
        {
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
