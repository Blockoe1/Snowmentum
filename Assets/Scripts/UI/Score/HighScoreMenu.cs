/*****************************************************************************
// File Name : HighScoreMenu.cs
// Author : Brandon Koederitz
// Creation Date : 10/4/2025
// Last Modified : 10/4/2025
//
// Brief Description : Controls the displaying of high scores.
*****************************************************************************/
using Snowmentum.Score;
using Snowmentum.UI;
using UnityEngine;

namespace Snowmentum
{
    public class HighScoreMenu : MonoBehaviour
    {
        [SerializeField] private HighScoreDisplayer displayerPrefab;

        private HighScoreDisplayer[] displays;

        /// <summary>
        /// Shows this menu and displays all the high scores.
        /// </summary>
        [ContextMenu("Test High Scores")]
        public void DisplayHighScores()
        {
            if (displays == null)
            {
                CreateHSDisplayers();
            }
            else
            {
                UpdateScores();
            }

            // Debug Line
            ScoreStatic.SaveHighScores();
        }

        /// <summary>
        /// If we have no displayers created, we should create new displayers for the high scores.
        /// </summary>
        private void CreateHSDisplayers()
        {
            gameObject.SetActive(true);
            displays = new HighScoreDisplayer[ScoreStatic.HighScoreCount];

            HighScoreDisplayer current;
            // Spawns a displayer for each high score and have it load a given score.
            for (int i = 0; i < ScoreStatic.HighScoreCount; i++)
            {
                current = Instantiate(displayerPrefab, transform);
                current.LoadHighScore(ScoreStatic.GetHighScore(i));
                displays[i] = current;
            }
        }

        /// <summary>
        /// Updates the score shown on each pre-existing display.
        /// </summary>
        public void UpdateScores()
        {
            for(int i = 0; i < displays.Length && i < ScoreStatic.HighScoreCount; i++)
            {
                displays[i].LoadHighScore(ScoreStatic.GetHighScore(i));
            }
        }
    }
}
