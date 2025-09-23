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
            ScoreStatic.OnScoreUpdated += UpdateScoreText;
        }
        private void OnDestroy()
        {
            ScoreStatic.OnScoreUpdated -= UpdateScoreText;
        }

        /// <summary>
        /// Updates the score text whenever the player's score changes.
        /// </summary>
        /// <param name="score"></param>
        private void UpdateScoreText(int score)
        {
            textComponent.text = score.ToString();
        }
    }
}
