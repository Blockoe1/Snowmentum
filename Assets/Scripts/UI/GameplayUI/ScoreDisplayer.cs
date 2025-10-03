/*****************************************************************************
// File Name : ScoreDisplayer.cs
// Author :
// Creation Date : 
// Last Modified : 
//
// Brief Description : 
*****************************************************************************/
using UnityEngine;
using TMPro;

namespace Snowmentum.Score
{
    public class ScoreDisplayer : MonoBehaviour
    {
        [SerializeField] private int displayedDigits = 5;
        [SerializeField] private int postfixDigits = 1;
        #region Component References
        [Header("Components")]
        [SerializeReference] private TMP_Text textComponent;

        /// <summary>
        /// Get components on reset.
        /// </summary>
        [ContextMenu("Get Component References")]
        private void Reset()
        {
            textComponent = GetComponent<TMP_Text>();
        }
        #endregion

        private void Awake()
        {
            // Reset score whenever the game begins.
            ScoreStatic.Score = 0;
            ScoreStatic.OnScoreUpdated += UpdateScoreText;
        }
        private void OnDestroy()
        {
            ScoreStatic.OnScoreUpdated -= UpdateScoreText;
        }

        /// <summary>
        /// Converts an iteger score into a string with zeros appended to the beginning to fill a certain 
        /// number of digits.
        /// </summary>
        /// <param name="digits"></param>
        /// <param name="score"></param>
        /// <param name="postfixDigits">The number of arbituary 0s to add to the end of the score to give it an 
        /// arcade feel.</param>
        /// <returns></returns>
        private static string ScoreToDigits(int digits, int score, int postfixDigits)
        {
            string scoreString = score.ToString();
            //Debug.Log(scoreString);
            for (int i = scoreString.Length; i < digits; i ++)
            {
                scoreString = "0" + scoreString;
            }
            // Add Postfix digits.
            for (int i = 0; i < postfixDigits; i++)
            {
                scoreString = scoreString + "0";
            }

            //Debug.Log(scoreString);
            return scoreString;
        }

        /// <summary>
        /// Updates the score text whenever the player's score changes.
        /// </summary>
        /// <param name="score"></param>
        private void UpdateScoreText(int score)
        {
            textComponent.text = ScoreToDigits(displayedDigits, score, postfixDigits);
        }
    }
}
