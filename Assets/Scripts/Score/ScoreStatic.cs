/*****************************************************************************
// File Name : Score.cs
// Author : Brandon Koederitz
// Creation Date : 9/22/2025
// Last Modified : 9/22/2025
//
// Brief Description : Script for holding the player's current score.
*****************************************************************************/
using System;
using System.IO;
using UnityEngine;

namespace Snowmentum.Score
{
    public static class ScoreStatic
    {
        #region CONSTS
        private const int HIGH_SCORE_COUNT = 10;
        private const int SCORE_DISPLAY_DIGITS = 5;
        private const int SCORE_POSTFIX_DIGITS = 1;
        private const string FILE_NAME = "sm_leaderboard.json";
        #endregion

        private static int score;
        public static event Action<int> OnScoreUpdated;

        private static HighScore[] highScores;

        #region Properties
        private static HighScore[] HighScores
        {
            get
            {
                if (highScores == null)
                {
                    LoadHighScores();
                }
                return highScores;
            }
        }
        //public static HighScore[] HighScores 
        //{
        //    get { return highScores; } 
        //}
        private static string FilePath => Path.Combine(Application.persistentDataPath, FILE_NAME);
        public static int HighScoreCount => HighScores == null ? 0 : HighScores.Length;
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

        #region Nested
        // Wrapper class for high score data when saved as JSON.
        [System.Serializable]
        public class HighScoreData
        {
            [SerializeField] internal HighScore[] scores;

            public HighScoreData(HighScore[] scores)
            {
                this.scores = scores;
            }
        }
        #endregion

        #region Save/Load
        /// <summary>
        /// Saves all of the current high scores to a file on the arcade machine.
        /// </summary>
        public static void SaveHighScores()
        {
            string filePath = FilePath;

            // Convert the high scores to JSON data.
            var jsonData = JsonUtility.ToJson(new HighScoreData(highScores), true);

            Debug.Log(jsonData);

            // Dont need to create a file since WriteAllText auto creates one if none exists.
            // Create a new save file if none exists.
            //if (!File.Exists(filePath))
            //{
            //    FileStream createdStream = File.Create(filePath);
            //    // Create opens
            //    createdStream.Close();
            //}

            // Write all the json data to the file.
            File.WriteAllText(filePath, jsonData);

            // Old binary save code
            //// Open a new file stream to the file where we will save the high scores.
            //using (FileStream stream = new FileStream(filePath, FileMode.Create))
            //{
            //    // Create a BinaryWriter that can then write the high scores to the file.
            //    using (BinaryWriter writer = new BinaryWriter(stream))
            //    {
            //        // Loop through all the high scores and save them to the file.
            //        for (int i = 0; i < HighScores.Length; i++)
            //        {
            //            writer.Write(HighScores[i].initials);
            //            writer.Write(HighScores[i].value);
            //        }

            //        Debug.Log("High Scores saved to file " + filePath);

            //        writer.Close();
            //    }
            //}


        }

        /// <summary>
        /// Loads all high scores from the machine.
        /// </summary>
        /// <remarks>
        /// Overwrites any current high scores.
        /// </remarks>
        public static void LoadHighScores()
        {
            string filePath = FilePath;

            static void SetDefaultScores()
            {
                // Set highScores to a default array.
                highScores = new HighScore[HIGH_SCORE_COUNT]
                {
                    HighScore.Default,
                    HighScore.Default,
                    HighScore.Default,
                    HighScore.Default,
                    HighScore.Default,
                    HighScore.Default,
                    HighScore.Default,
                    HighScore.Default,
                    HighScore.Default,
                    HighScore.Default
                };
            }

            if (File.Exists(filePath))
            {
                // Read the text from the JSON file.
                string readData = File.ReadAllText(filePath);
                // Convert the JSON text to high score information.

                HighScoreData data = JsonUtility.FromJson<HighScoreData>(readData);
                if (data != null && data.scores != null)
                {
                    highScores = data.scores;
                }
                else
                {
                    SetDefaultScores();
                    Debug.Log("Failed to load high scores from " + filePath + ".  No JSON data was detected.");
                }
            }
            else
            {
                SetDefaultScores();
                Debug.Log("Failed to load high scores from " + filePath + ".  No file exists at the specified path.");
            }

            // Old binary code
            //// Check if the file exists.
            //if (File.Exists(filePath))
            //{
            //    // Create a FileStream to the file that has all of the high scores saved.
            //    using (FileStream stream = new FileStream(filePath, FileMode.Open))
            //    {
            //        // Create a BinaryReader to read the contents of the saved high score file.
            //        using (BinaryReader reader = new BinaryReader(stream))
            //        {
            //            // Load data from the file for each high score that is set to be stored.
            //            highScores = new HighScore[HIGH_SCORE_COUNT];
            //            for(int i = 0; i < HIGH_SCORE_COUNT; i++)
            //            {
            //                highScores[i] = new HighScore(reader.ReadString(), reader.ReadInt32());
            //            }

            //            Debug.Log("Loaded high scores from file at " + filePath);

            //            reader.Close();
            //        }
            //    }

            //}
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
            return UIHelpers.ArcadeFormat(score, SCORE_DISPLAY_DIGITS, SCORE_POSTFIX_DIGITS);
        }

        /// <summary>
        /// Checks if a given score beats one of the existing high scores.
        /// </summary>
        /// <returns>True if the score beats a high score.</returns>
        public static bool CheckHighScore(int score)
        {
            // We only need to check if the high score is higher than the lowest high score.
            return score > HighScores[^1].value;
        }
        public static bool CheckHighScore()
        {
            // We only need to check if the high score is higher than the lowest high score.
            return CheckHighScore(Score);
        }

        /// <summary>
        /// Adds a given score and corresponding initials to the high scores and removes the one that was displaced.
        /// </summary>
        /// <param name="initials">The initials to save with the high score.</param>
        /// <param name="score">The value of the high score.</param>
        /// <returns>The index that the high score was added at.</returns>
        public static int AddHighScore()
        {
            return AddHighScore(Score);
        }
        public static int AddHighScore(int score)
        {
            // Loop through each high score to find the one that this score will replace
            HighScore prevScore = new HighScore();
            bool hasAddedScore = false;
            int hsIndex = -1;
            // Start from the lowest high score.
            for(int i = 0; i < HighScores.Length; i++)
            {
                if (hasAddedScore)
                {
                    // Inserts our previous high score to this high score, then sets this high score as the previous
                    // to be assigned in the next iteration of the loop.
                    // The last high score is lost.
                    (HighScores[i], prevScore) = (prevScore, HighScores[i]);
                    //Debug.Log(prevScore.value);
                }
                else
                {
                    // If we havent already added the score to the array, then check if this is where we should
                    // insert the score.
                    if (score > HighScores[i].value)
                    {
                        hsIndex = i;
                        prevScore = HighScores[i];
                        HighScores[i] = new HighScore(score);
                        hasAddedScore = true;
                    }
                }
            }
            return hsIndex;
        }

        /// <summary>
        /// Gets the high score stored at a given index.
        /// </summary>
        /// <param name="index">The index of the high score to get.</param>
        /// <returns></returns>
        public static HighScore GetHighScore(int index)
        {
            if (index < HighScores.Length)
            {
                return HighScores[index];
            }
            return HighScore.Default;
        }
    }
}
