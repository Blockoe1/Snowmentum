/*****************************************************************************
// File Name : ScoreDisplayAnimator.cs
// Author : Brandon Koederitz
// Creation Date : 11/9/2025
// Last Modified : 11/9/2025
//
// Brief Description : Plays an animation on the score display text when the score updates.
*****************************************************************************/
using System.Collections;
using TMPro;
using UnityEngine;

namespace Snowmentum.Score
{
    public class ScoreDisplayAnimator : MonoBehaviour
    {
        [SerializeField] private Vector2 targetPos;
        [SerializeField] private Color targetColor;
        [SerializeField, Tooltip("Calculates the proportion of the score that is used to determine the magnitude " +
            "of the animation"), Min(0.0001f)] 
        private float targetProportion;
        [SerializeField] private float animTime;

        private Vector2 startingPos;
        private Color baseColor;
        private Coroutine animRoutine;

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

        /// <summary>
        /// Subscribe/Unsubscribe from events.
        /// </summary>
        private void Awake()
        {
            baseColor = textComponent.color;
            startingPos = transform.localPosition;
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
        private void AnimateScore(int score, int oldScore)
        {
            // Skip the animation if we're resetting back to 0.
            if (score != 0)
            {
                if (animRoutine != null)
                {
                    StopCoroutine(animRoutine);
                    animRoutine = null;
                }
                animRoutine = StartCoroutine(AnimationRoutine(score - oldScore, score));
            }
        }

        /// <summary>
        /// Animates the popup animation using a sine function.
        /// </summary>
        /// <param name="scoreDiff">The amount of score gained.</param>
        /// <returns></returns>
        private IEnumerator AnimationRoutine(int scoreDiff, int score)
        {
            // Uses the scoreDiff to calculatae the appropriate target color and position.
            float lerpVal = score == 0 || targetProportion == 0 ? 1 : scoreDiff / (score * targetProportion);

            Color targetColor = Color.Lerp(baseColor, this.targetColor, lerpVal);
            Vector2 targetPos = Vector2.Lerp(startingPos, this.targetPos, lerpVal);

            float timer = 0;
            while (timer < animTime)
            {
                float normalizedTime = timer / animTime;

                // Animate the text between the starting and target position using a sin wave.
                transform.localPosition = Vector2.Lerp(startingPos, targetPos, Mathf.Sin(normalizedTime * Mathf.PI));
                textComponent.color = Color.Lerp(baseColor, targetColor, Mathf.Sin(normalizedTime * Mathf.PI));

                timer += Time.deltaTime;
                yield return null;
            }

            animRoutine = null;
        }
    }
}
