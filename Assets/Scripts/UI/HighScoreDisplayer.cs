/*****************************************************************************
// File Name : HighScoreDisplayer.cs
// Author : Brandon Koederitz
// Creation Date : 10/3/2025
// Last Modified : 10/3/2025
//
// Brief Description : Displays a given high score in game.
*****************************************************************************/
using UnityEngine;
using Snowmentum.Score;
using TMPro;

namespace Snowmentum.UI
{
    public class HighScoreDisplayer : MonoBehaviour
    {
        [SerializeField] private TMP_Text initialsText;
        [SerializeField] private TMP_Text scoreText;
        [SerializeField] private int highScoreIndex;


        private void Awake()
        {
            // Load the default high score on awake
            LoadHighScore();
        }

        /// <summary>
        /// Loads a given high score and displays it.
        /// </summary>
        /// <param name="hs"></param>
        private void LoadHighScore(HighScore hs)
        {
            initialsText.text = hs.initials;
            scoreText.text =ScoreStatic.FormatScore(hs.value);
        }
        public void LoadHighScore()
        {
            LoadHighScore(ScoreStatic.GetHighScore(highScoreIndex));
        }
    }
}
