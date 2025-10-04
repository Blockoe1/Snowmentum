/*****************************************************************************
// File Name : Score.cs
// Author : Brandon Koederitz
// Creation Date : 9/22/2025
// Last Modified : 9/22/2025
//
// Brief Description : Script for holding the player's current score.
*****************************************************************************/
using System;
using UnityEngine;

namespace Snowmentum.Score
{
    public static class ScoreStatic
    {
        #region CONSTS
        private const int HIGH_SCORE_COUNT = 10;
        private const int SCORE_DISPLAY_DIGITS = 5;
        private const int SCORE_POSTFIX_DIGITS = 1;
        #endregion

        private static int score;
        public static event Action<int> OnScoreUpdated;

        private static HighScore[] highScores;

        #region Properties
        //public static HighScore[] HighScores 
        //{
        //    get { return highScores; } 
        //}
        public static int Score
        {
            get
            {
                return score;
            }
            set
            {
                score = value;
                OnScoreUpdated?.Invoke(score);
            }
        }
        #endregion

        /// <summary>
        /// Converts an iteger score into a string with zeros appended to the beginning to fill a certain 
        /// number of digits.
        /// </summary>
        /// <param name="score"></param>
        /// <returns></returns>
        public static string FormatScore(int score)
        {
            string scoreString = score.ToString();
            //Debug.Log(scoreString);
            for (int i = scoreString.Length; i < SCORE_DISPLAY_DIGITS; i++)
            {
                scoreString = "0" + scoreString;
            }
            // Add Postfix digits.
            for (int i = 0; i < SCORE_POSTFIX_DIGITS; i++)
            {
                scoreString = scoreString + "0";
            }

            //Debug.Log(scoreString);
            return scoreString;
        }

        /// <summary>
        /// Saves all of the current high scores to a file on the arcade machine.
        /// </summary>
        public static void SaveHighScores()
        {
            // Not Implemented
        }

        /// <summary>
        /// Loads all high scores from the machine.
        /// </summary>
        /// <remarks>
        /// Overwrites any current high scores.
        /// </remarks>
        public static void LoadHighScores()
        {
            highScores = new HighScore[HIGH_SCORE_COUNT]
            {
                new HighScore("TST", 1000 ),
                new HighScore("TST", 900 ),
                new HighScore("TST", 800 ),
                new HighScore("TST", 700 ),
                new HighScore("TST", 600 ),
                new HighScore("TST", 500 ),
                new HighScore("TST", 400 ),
                new HighScore("TST", 300 ),
                new HighScore("TST", 200 ),
                new HighScore("TST", 100 )
            };
        }

        /// <summary>
        /// Checks if a given score beats one of the existing high scores.
        /// </summary>
        /// <returns>True if the score beats a high score.</returns>
        public static bool CheckHighScore(int score)
        {
            // We only need to check if the high score is higher than the lowest high score.
            return score > highScores[^1].value;
        }

        /// <summary>
        /// Adds a given score and corresponding initials to the high scores and removes the one that was displaced.
        /// </summary>
        /// <param name="initials">The initials to save with the high score.</param>
        /// <param name="score">The value of the high score.</param>
        public static void AddHighScore(string initials, int score)
        {

            // Loop through each high score to find the one that this score will replace
            HighScore prevScore = new HighScore();
            bool hasAddedScore = false;
            // Start from the lowest high score.
            for(int i = 0; i < highScores.Length; i++)
            {
                if (hasAddedScore)
                {
                    // Inserts our previous high score to this high score, then sets this high score as the previous
                    // to be assigned in the next iteration of the loop.
                    // The last high score is lost.
                    (highScores[i], prevScore) = (prevScore, highScores[i]);
                }
                else
                {
                    // If we havent already added the score to the array, then check if this is where we should
                    // insert the score.
                    if (score > highScores[i].value)
                    {
                        prevScore = highScores[i];
                        highScores[i] = new HighScore(initials, score);
                        hasAddedScore = true;
                    }
                }
            }
        }

        /// <summary>
        /// Gets the high score stored at a given index.
        /// </summary>
        /// <param name="index">The index of the high score to get.</param>
        /// <returns></returns>
        public static HighScore GetHighScore(int index)
        {
            if (index < highScores.Length)
            {
                return highScores[index];
            }
            return HighScore.Default;
        }
    }
}
