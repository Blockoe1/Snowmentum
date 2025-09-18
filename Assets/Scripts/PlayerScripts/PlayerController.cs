/*****************************************************************************
// File Name : PlayerController.cs
// Author : Jack Fisher
// Creation Date : September 17th, 2025
// Last Modified : September 17th, 2025
//
// Brief Description : As of script creation this script just handles player movement. 
// We can expand this script to handle more parts of the snowball if needed though. 
*****************************************************************************/
using System;
using System.Collections;
using System.Net.Sockets;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
//I added a bunch of these Unity system things just so hopefully I will never be missing one. Most are currently uneccesary. 


namespace Snowmentum
{
    public class PlayerController : MonoBehaviour
    {
        //We can change this during gameplay in order to make it harder to move the snowball as it grows in size
        // And also adjust it as needed to make it feel responsive enough
        [SerializeField] private float movementSensitivity;

        [SerializeField] private PlayerInput playerInput;
        private InputAction mouseMovement;

        private Vector2 mouseDelta;


        private void Start()
        {
            //set the player input active
            playerInput = GetComponent<PlayerInput>();
            playerInput.currentActionMap.Enable();
            mouseMovement = playerInput.currentActionMap.FindAction("MouseMovement");
            mouseMovement.started += Handle_SnowballMouseMovement;
            
        }
        
        void Update()
        {
            //will keep track of the mouse and move the snowball accordingly each frame
            MoveSnowball();
        }

        ///  Player Movement
        /// 
        /// 
        //this function will track the movement of the mouse and trackball
        //I set it to public so I could use a feature in the inspector to automatically stop the snowball when the mouse stops moving
        //hopefully this function being public is fine
        public void Handle_SnowballMouseMovement(InputAction.CallbackContext context)
        {
           mouseDelta = context.ReadValue<Vector2>(); ;
        }

        //this function moves the snowball up and down in accordance with the movement of the mouse
        private void MoveSnowball()
        {
            transform.Translate(Vector3.up * mouseDelta.y * movementSensitivity);
        }
    }
}
