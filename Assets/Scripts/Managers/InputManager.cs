/*****************************************************************************
// File Name : InputManager.cs
// Author : Brandon Koederitz
// Creation Date : 10/4/2025
// Last Modified : 10/4/2025
//
// Brief Description : Centralized place to read framerate independent mouse delta input
*****************************************************************************/
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Snowmentum
{
    public class InputManager : MonoBehaviour
    {
        #region CONSTS
        private const string DELTA_ACTION_NAME = "MouseMovement";
        #endregion

        [SerializeField] private float mouseSensitivity = 1f;

        private static Vector2 mouseDelta;
        
        private InputAction mouseDeltaAction;

        #region Events
        public static event Action<Vector2> OnDeltaUpdate;
        #endregion

        #region Properties
        public static Vector2 MouseDelta => mouseDelta;
        #endregion

        private void Awake()
        {
            mouseDeltaAction = InputSystem.actions.FindAction(DELTA_ACTION_NAME);
        }

        /// <summary>
        /// Continually update our mouse delta value in update so that we can get a frame independent value.
        /// </summary>
        /// <remarks>
        /// Update boooo.
        /// </remarks>
        private void Update()
        {
            // Only allow mouse updates if the game is not paused.
            if ( Time.deltaTime > 0)
            {
                // Use Time.deltaTime here to average the delta into pixels / second.
                // Without this we get framerate dependence
                mouseDelta = mouseDeltaAction.ReadValue<Vector2>() * mouseSensitivity / Time.deltaTime;
                OnDeltaUpdate?.Invoke(mouseDelta);
            }
        }
    }
}
