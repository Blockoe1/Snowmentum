/*****************************************************************************
// File Name : ScorePopupAnim.cs
// Author : Brandon Koederitz
// Creation Date : 11/9/2025
// Last Modified : 11/9/2025
//
// Brief Description : Animates the score popup when an obstacle is destroyed.
*****************************************************************************/
using System.Collections;
using TMPro;
using UnityEngine;

namespace Snowmentum
{
    [RequireComponent(typeof(TMP_Text))]
    public class ScorePopupAnim : MonoBehaviour
    {
        #region CONSTS
        private const string ANIM_TRIGGER = "ScoreGained";
        #endregion

        [SerializeField] private Vector2 animTargetPos;
        [SerializeField] private float animTime;
        [SerializeField] private float pauseTime;

        private Vector2 startingPos;
        private bool isAnim;

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
        /// Store the starting position.
        /// </summary>
        private void Awake()
        {
            startingPos = transform.position;
        }

        /// <summary>
        /// Plays the score popup and updates the text with the score gained.
        /// </summary>
        /// <param name="scoreGained"></param>
        public void PlayAnimation(int scoreGained)
        {
            if (isAnim) { return; }
            // Enable the text.
            textComponent.text = scoreGained.ToString();
            StartCoroutine(AnimationRoutine());
        }

        /// <summary>
        /// Animates the popup animation using a sine function.
        /// </summary>
        /// <returns></returns>
        private IEnumerator AnimationRoutine()
        {
            isAnim = true;
            textComponent.enabled = true;

            float timer = 0;
            while (timer < animTime)
            {
                float normalizedTime = timer / animTime;
                // Animate the text between the starting and target position using a sin wave.
                transform.localPosition = Vector2.Lerp(startingPos, animTargetPos, Mathf.Sin(normalizedTime * Mathf.PI));
                timer += Time.deltaTime;
                yield return null;
            }

            yield return new WaitForSeconds(pauseTime);

            textComponent.enabled = false;
            isAnim = false;
        }
    }
}
