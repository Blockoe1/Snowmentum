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
using UnityEngine.Jobs;

namespace Snowmentum.Score
{
    public delegate void AnimationEndCallback(ScorePopupAnim thisAnim);
    [RequireComponent(typeof(TMP_Text))]
    public class ScorePopupAnim : MonoBehaviour
    {
        [SerializeField] private float animTime;
        [Header("Position Animation")]
        [SerializeField] private Vector2 targetOffset;
        [SerializeField] private AnimationCurve positionCurve;
        [Header("Color Animation")]
        [SerializeField] private Color targetColor;
        [SerializeField] private AnimationCurve colorCurve;

        private Color baseColor;
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

        #region Nested
        private struct AnimationData<T>
        {
            internal T startPos;
            internal T endPos;
            internal AnimationCurve curve;

            internal AnimationData(T startPos, T endPos, AnimationCurve curve)
            {
                this.startPos = startPos;
                this.endPos = endPos;
                this.curve = curve;
            }
        }

        #endregion

        /// <summary>
        /// Store the starting position.
        /// </summary>
        private void Awake()
        {
            baseColor = textComponent.color;
        }

        /// <summary>
        /// Plays the score popup and updates the text with the score gained.
        /// </summary>
        /// <param name="scoreGained"></param>
        public void Play(string display, Vector2 position, AnimationEndCallback endCallback = null)
        {
            if (isAnim) { return; }
            // Enable the text.
            textComponent.text = display;
            textComponent.color = baseColor;

            // Set the positions.
            transform.position = position;
            Debug.Log(position);

            // Starts the animation.
            StartCoroutine(AnimationRoutine(new AnimationData<Vector2>(position, position + targetOffset, positionCurve), 
                new AnimationData<Color>(baseColor, targetColor, colorCurve), endCallback));
        }

        /// <summary>
        /// Animates the popup animation using an animation curve.
        /// </summary>
        /// <param name="posAnim">The animation data for animation the popup's position.</param>
        /// <param name="colorAnim">The animation data for animating the text's color.</param>
        /// <param name="callback">The callback to trigger when the animation finishes.</param>
        /// <returns></returns>
        private IEnumerator AnimationRoutine(AnimationData<Vector2> posAnim, AnimationData<Color> colorAnim, 
            AnimationEndCallback callback)
        {
            isAnim = true;

            float timer = 0;
            while (timer < animTime)
            {
                float normalizedTime = timer / animTime;
                // Process the position animation
                transform.position = Vector2.Lerp(posAnim.startPos, posAnim.endPos, 
                    posAnim.curve.Evaluate(normalizedTime));
                textComponent.color = Color.Lerp(colorAnim.startPos, colorAnim.endPos, 
                    colorAnim.curve.Evaluate(normalizedTime));
                timer += Time.deltaTime;
                yield return null;
            }

            callback?.Invoke(this);
            isAnim = false;
        }
    }
}
