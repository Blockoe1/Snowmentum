/*****************************************************************************
// File Name : ForceQuit.cs
// Author : Brandon Koederitz
// Creation Date : 12/3/2025
// Last Modified : 12/3/2025
//
// Brief Description : Tab force quits the game.
*****************************************************************************/
using UnityEngine;
using UnityEngine.InputSystem;

namespace Snowmentum
{
    public class ForceQuit : MonoBehaviour
    {
        #region CONSTS
        private const string FORCE_QUIT_NAME = "ForceQuit";
        #endregion

        private static InputAction forceQuitAction;

        /// <summary>
        /// Setup the event reference on awake.
        /// </summary>
        private void Awake()
        {
            forceQuitAction = InputSystem.actions.FindAction(FORCE_QUIT_NAME);
            // Subscribe static function and don't unsubscribe on destroy so that even if this script isnt around the
            // player can force quit.
            forceQuitAction.performed += ForceQuitApp;
        }

        /// <summary>
        /// Force quits the game
        /// </summary>
        /// <param name="obj"></param>
        private void ForceQuitApp(InputAction.CallbackContext obj)
        {
            // Unsubscribe the event in case.
            forceQuitAction.performed -= ForceQuitApp;   
            Application.Quit();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
    }
}
