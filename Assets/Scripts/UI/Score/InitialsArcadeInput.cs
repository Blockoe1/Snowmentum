/*****************************************************************************
// File Name : InitialsArcadeInput.cs
// Author : Brandon Koederitz
// Creation Date : 10/22/2025
// Last Modified : 10/22/2025
//
// Brief Description : Reads arcade stick input and buttons and converts them into mouse input sent to the initials
// input menu.
*****************************************************************************/
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Snowmentum.UI
{
    [RequireComponent(typeof(InitialsInputMenu))]
    public class InitialsArcadeInput : MonoBehaviour
    {
        private InputAction navigateAction;
        private bool isInput;

        #region Component References
        [Header("Components")]
        [SerializeReference] protected InitialsInputMenu inputMenu;

        /// <summary>
        /// Get components on reset.
        /// </summary>
        [ContextMenu("Get Component References")]
        private void Reset()
        {
            inputMenu = GetComponent<InitialsInputMenu>();
        }
        #endregion

        /// <summary>
        /// Setup input.
        /// </summary>
        private void Awake()
        {
            navigateAction = InputSystem.actions.FindAction("Navigate");
            Debug.Log(navigateAction);

            navigateAction.started += NavigateAction_started;
            navigateAction.canceled += NavigateAction_canceled;
        }

        private void OnDestroy()
        {
            navigateAction.started -= NavigateAction_started;
            navigateAction.canceled -= NavigateAction_canceled;
        }

        /// <summary>
        /// Handles reading player input and sending that to the input menu.
        /// </summary>
        /// <param name="obj"></param>
        private void NavigateAction_started(InputAction.CallbackContext obj)
        {
            Debug.Log("Started");
            if (!isInput)
            {
                isInput = true;
                StartCoroutine(InputRoutine());
            }
        }
        private void NavigateAction_canceled(InputAction.CallbackContext obj)
        {
            Debug.Log("Canceled");
            isInput = false;
        }

        /// <summary>
        /// While the player holds down an input, continually send updates to the input menu.
        /// </summary>
        /// <returns></returns>
        private IEnumerator InputRoutine()
        {
            while(isInput)
            {
                Vector2Int input = MathHelpers.RoundVectorToInt(navigateAction.ReadValue<Vector2>());
                // Reverse Y input.
                input.y *= -1;
                inputMenu.HandleInput(input);
                yield return null;
            }
        }
    }
}
