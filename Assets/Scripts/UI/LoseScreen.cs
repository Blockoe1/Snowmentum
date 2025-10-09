/*****************************************************************************
// File Name : LoseScreen.cs
// Author : Brandon Koederitz
// Creation Date : 10/3/2025
// Last Modified : 10/3/2025
//
// Brief Description : Controls button functions for the lose screen.
*****************************************************************************/
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Snowmentum.Score;

namespace Snowmentum.UI
{
    public class LoseScreen : MonoBehaviour
    {

        /// <summary>
        /// Restarts the game
        /// </summary>
        //public void Restart()
        //{
        //    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        //}

        /// <summary>
        /// Returns the player to the main menu.
        /// </summary>
        //public void ReturnToMainMenu()
        //{
        //    // Main Menu should be 0 in build settings.
        //    SceneManager.LoadScene(0);
        //}

        IEnumerator ScreenDuration()
        {
            //Screen duration is 5 seconds
            yield return new WaitForSeconds(5f);
            ScoreStatic.CheckHighScore();
            if (ScoreStatic.CheckHighScore())
            {
                TransitionToScene("HighScoreScene");
            }
            else
            {
                TransitionToScene("TitleScene");
            }
        }

        /// <summary>
        /// Sends player to HighScoreScene after 5 seconds
        /// </summary>
        public void TransitionToScene(string targetScene)
        {
            SceneManager.LoadScene(targetScene);
        }

        public void StartScreenDelay()
        {
            StartCoroutine(ScreenDuration());
        }
    }
}
