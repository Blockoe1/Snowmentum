/*****************************************************************************
// File Name : ScoreDisplayAnimator.cs
// Author : Brandon Koederitz
// Creation Date : 11/9/2025
// Last Modified : 11/9/2025
//
// Brief Description : Plays an animation on the score display text when the score updates.
*****************************************************************************/
using TMPro;
using UnityEngine;

namespace Snowmentum.Score
{
    public class ScoreDisplayAnimator : MonoBehaviour
    {
        #region CONSTS
        private const string ANIM_TRIGGER = "ScoreUpdated";
        #endregion

        #region Component References
        [Header("Components")]
        [SerializeReference] private Animator anim;

        /// <summary>
        /// Get components on reset.
        /// </summary>
        [ContextMenu("Get Component References")]
        private void Reset()
        {
            anim = GetComponent<Animator>();
        }
        #endregion

        /// <summary>
        /// Subscribe/Unsubscribe from events.
        /// </summary>
        private void Awake()
        {
            ScoreStatic.OnScoreUpdated += AnimateScore;
        }

        private void OnDestroy()
        {
            ScoreStatic.OnScoreUpdated -= AnimateScore;
        }

        /// <summary>
        /// Plays an animation on the score UI when the score updates.
        /// </summary>
        /// <param name="score"></param>
        private void AnimateScore(int score)
        {
            // Skip the animation if we're resetting back to 0.
            if (score != 0)
            {
                anim.SetTrigger(ANIM_TRIGGER);
            }
        }
    }
}
