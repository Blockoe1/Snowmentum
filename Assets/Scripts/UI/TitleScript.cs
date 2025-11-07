/*****************************************************************************
// File Name : TitleScript.cs
// Author : Jack Fisher
// Creation Date : October 1, 2025
// Last Modified : October 3, 2025
//
// Brief Description : This script has the function the start game button uses to load the gameplay scene
*****************************************************************************/
using Snowmentum.Score;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Snowmentum
{
    public class TitleScript : MonoBehaviour
    {
        [SerializeField] private GameObject mainMenu;
        [SerializeField] private GameObject scoresDisplay;

        private void Start()
        {
            scoresDisplay.SetActive(false);
            mainMenu.SetActive(true);
        }

        public void PlayGame()
        {
            //We can make the title scene 0 in the build and the gameplay scene 1
            TransitionManager.LoadScene("GameplayScene", TransitionType.Snowy);
        }

        /// <summary>
        /// Displays the high scores.
        /// </summary>
        public void ShowScores()
        {
            mainMenu.SetActive(false);
            scoresDisplay.SetActive(true);
        }

        /// <summary>
        /// Goes back to the main menu.
        /// </summary>
        public void GoBack()
        {
            scoresDisplay.SetActive(false);
            mainMenu.SetActive(true);
        }

        /// <summary>
        /// Quits the game.
        /// </summary>
        public void Quit()
        {
            Application.Quit();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
    }
}
