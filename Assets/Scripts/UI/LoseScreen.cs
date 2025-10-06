/*****************************************************************************
// File Name : LoseScreen.cs
// Author : Brandon Koederitz
// Creation Date : 10/3/2025
// Last Modified : 10/3/2025
//
// Brief Description : Controls button functions for the lose screen.
*****************************************************************************/
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Snowmentum.UI
{
    public class LoseScreen : MonoBehaviour
    {
        /// <summary>
        /// Restarts the game
        /// </summary>
        public void Restart()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        /// <summary>
        /// Returns the player to the main menu.
        /// </summary>
        public void ReturnToMainMenu()
        {
            // Main Menu should be 0 in build settings.
            SceneManager.LoadScene(0);
        }
    }
}
