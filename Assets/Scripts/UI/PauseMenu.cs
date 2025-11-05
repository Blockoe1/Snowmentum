/*****************************************************************************
// File Name : PauseMenu.cs
// Author : Brandon Kooederitz
// Creation Date : 11/5/2025
// Last Modified : 11/5/2025
//
// Brief Description : Controls the pause menu on the PC build.
*****************************************************************************/
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Snowmentum
{
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField] private GameObject pauseMenuPanel;
        [SerializeField] private Button selectedButton;

        private InputAction pauseAction;
        private bool isPaused;

        /// <summary>
        /// Setup input.
        /// </summary>
        private void Awake()
        {
            pauseAction = InputSystem.actions.FindAction("Pause");
            pauseAction.performed += PauseAction_performed;
        }
        private void OnDestroy()
        {
            pauseAction.performed -= PauseAction_performed;
        }

        /// <summary>
        /// Toggle the puase menu when the player presses escape.
        /// </summary>
        /// <param name="obj"></param>
        private void PauseAction_performed(InputAction.CallbackContext obj)
        {
            TogglePause(!isPaused);
        }

        /// <summary>
        /// Toggles the pause menu.
        /// </summary>
        /// <param name="isPaused"></param>
        public void TogglePause(bool isPaused)
        {
            pauseMenuPanel.SetActive(isPaused);
            this.isPaused = isPaused;
            Time.timeScale = isPaused ? 1 : 0;

            // Unlock the cursor when paused.
            Cursor.lockState = isPaused ? CursorLockMode.None : CursorLockMode.Locked;

            if (isPaused)
            {

            }
            else
            {

            }
        }
    }
}
