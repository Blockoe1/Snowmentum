/*****************************************************************************
// File Name : ScoreManager.cs
// Author : Brandon Koederitz
// Creation Date : 10/3/2025
// Last Modified : 10/3/2025
//
// Brief Description : Controls the loading and management of scores during gameplay.
*****************************************************************************/
using Snowmentum.UI;
using UnityEngine;
using UnityEngine.Events;

namespace Snowmentum.Score
{
    public class ScoreManager : MonoBehaviour
    {
        [SerializeField] private HighScoreMenu displayMenu;
        [SerializeField] private InitialsInputMenu initialsInputMenu;
        private void Awake()
        {
            // Load the high scores whenwe enter this screen.
            ScoreStatic.LoadHighScores();
        }

        /// <summary>
        /// Saves the current high score
        /// </summary>
        [ContextMenu("Test HS Saving")]
        public void SaveNewHighScore()
        {
            // Add a new high score to the score static.
            int hsIndex = ScoreStatic.AddHighScore();

            // Update the high score displays for the high score displayer.
            displayMenu.DisplayHighScores();

            // Set a given dispalyer as selected.
            displayMenu.SetSelected(hsIndex, true);

            // Start the initials input.
            initialsInputMenu.LoadHighScore(ScoreStatic.GetHighScore(hsIndex));
        }
    }
}
