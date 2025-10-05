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
        [SerializeField] private TMP_Text placementText;
        //[SerializeField] private int highScoreIndex;

        private HighScore loadedHighScore;

        private void OnDestroy()
        {
            if (loadedHighScore != null)
            {
                loadedHighScore.OnInitialsChanged -= SetInitials;
            }
        }

        /// <summary>
        /// Loads a given high score and displays it.
        /// </summary>
        /// <param name="hs"></param>
        public void LoadHighScore(HighScore hs, int index)
        {
            if (loadedHighScore != null)
            {
                loadedHighScore.OnInitialsChanged -= SetInitials;
            }

            loadedHighScore = hs;

            // Subscribe to change events.
            if (loadedHighScore != null)
            {
                initialsText.text = hs.initials;
                scoreText.text = ScoreStatic.FormatScore(hs.value);
                placementText.text = index + ":";

                loadedHighScore.OnInitialsChanged += SetInitials;

            }
        }

        /// <summary>
        /// Whenever the high score this displayer is showing has it's initials changed, then we should update our
        /// text object to reflect that change.
        /// </summary>
        /// <param name="initials"></param>
        private void SetInitials(string initials)
        {
            initialsText.text = initials;
        }
        //public void LoadHighScore()
        //{
        //    LoadHighScore(ScoreStatic.GetHighScore(highScoreIndex));
        //}
    }
}
