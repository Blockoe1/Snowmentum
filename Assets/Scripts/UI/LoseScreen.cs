/*****************************************************************************
// File Name : LoseScreen.cs
// Author : Brandon Koederitz
// Creation Date : 10/3/2025
// Last Modified : 10/3/2025
//
// Brief Description : Controls button functions for the lose screen.
*****************************************************************************/
using Snowmentum.Score;
using System.Collections;
using UnityEngine;

namespace Snowmentum.UI
{
    public class LoseScreen : MonoBehaviour
    {
        [SerializeField] private float loseScreenTime = 5;
        [SerializeField] private TransitionType transitionType;

        #region CONSTS
        private const string HIGH_SCORE_SCENE_NAME = "SaveHighScoreScene";
        private const string TITLE_SCREEN_NAME = "TitleScene";
        #endregion

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
            yield return new WaitForSecondsRealtime(loseScreenTime);
            EndGame();
        }

        /// <summary>
        /// Sends player to HighScoreScene after 5 seconds
        /// </summary>
        public void TransitionToScene(string targetScene)
        {
            TransitionManager.LoadScene(targetScene, transitionType);
        }

        public void StartScreenDelay()
        {
            StartCoroutine(ScreenDuration());
        }

        /// <summary>
        /// Ends the current game and takes the player to the main menu or high score menu based on their score.
        /// </summary>
        public void EndGame()
        {
            ScoreStatic.CheckHighScore();
            if (ScoreStatic.CheckHighScore())
            {
                TransitionToScene(HIGH_SCORE_SCENE_NAME);
            }
            else
            {
                TransitionToScene(TITLE_SCREEN_NAME);
            }
        }
    }
}
