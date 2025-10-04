/*****************************************************************************
// File Name : ScoreManager.cs
// Author : Brandon Koederitz
// Creation Date : 10/3/2025
// Last Modified : 10/3/2025
//
// Brief Description : Controls the loading and management of scores during gameplay
*****************************************************************************/
using UnityEngine;
using UnityEngine.Events;

namespace Snowmentum.Score
{
    public class ScoreManager : MonoBehaviour
    {
        [SerializeField] private UnityEvent OnHighScore;
        private void Awake()
        {
            // Reset score whenever the game begins.
            ScoreStatic.Score = 0;

            // Load the high scores when the game begins.
            ScoreStatic.LoadHighScores();

            //ScoreStatic.AddHighScore("TSB", 750);
        }

        /// <summary>
        /// Performs a check to see if the player has beat a high score and then do some actions if they have.
        /// </summary>
        public void CheckHighScore()
        {
            if (ScoreStatic.CheckHighScore())
            {
                OnHighScore?.Invoke();
            }
        }
    }
}
