/*****************************************************************************
// File Name : TitleVideo.cs
// Author : Brandon Koederitz
// Creation Date : 10/23/2025
// Last Modified : 10/23/2025
//
// Brief Description : Controls transitions to and playing the tutorial video on the title screen.
*****************************************************************************/
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace Snowmentum.UI
{
    public class TitleVideo : MonoBehaviour
    {
        [SerializeField] private Image fadeToBlack;
        [SerializeField] private GameObject titleScreen;
        [SerializeField] private VideoPlayer tutorialVideo;
        [SerializeField] private float titleScreenTime;
        [SerializeField] private float fadeTime;

        /// <summary>
        /// Start the video transition routine once the game starts.
        /// </summary>
        private void Awake()
        {
            StartCoroutine(VideoRoutine());
        }

        /// <summary>
        /// Continually loops and transitions between the title screen and tutorial video.
        /// </summary>
        /// <returns></returns>
        private IEnumerator VideoRoutine()
        {
            float halfFadeTime = fadeTime / 2;
            float videoLength = (float)tutorialVideo.length;

            // Local Functions for readability.
            Coroutine FadeToBlack()
            {
                fadeToBlack.color = SetAlpha(fadeToBlack.color, 0);
                fadeToBlack.gameObject.SetActive(true);
                return StartCoroutine(FadeToAlpha(halfFadeTime, 1));
            }

            Coroutine FadeIn()
            {
                return StartCoroutine(FadeToAlpha(halfFadeTime, 0));
            }

            // Toggles between showing the title screen and the video.  True for title.
            void ToggleTitle(bool toggle)
            {
                titleScreen.SetActive(toggle);
                tutorialVideo.gameObject.SetActive(!toggle);
            }

            while (true)
            {
                // Wait a bit to show the title screen.
                yield return new WaitForSeconds(titleScreenTime);

                // Fade to black.
                yield return FadeToBlack();

                // Start the video and hide the title screen.
                ToggleTitle(false);
                tutorialVideo.Play();

                // Fade in.
                yield return FadeIn();
                fadeToBlack.gameObject.SetActive(false);

                // Wait until the video finishes.
                yield return new WaitForSeconds(videoLength - fadeTime);

                // Fade to black
                yield return FadeToBlack();

                // Disable video and re-show the title
                ToggleTitle(true);
                tutorialVideo.Stop();

                // Fade back in.
                yield return FadeIn();
                fadeToBlack.gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// Fades the FadeToBlack object in or out based on the target alpha.
        /// </summary>
        /// <param name="time">The time the fade should take.</param>
        /// <param name="targetAlpha">The target alpha of the FadeToBlack image.</param>
        /// <returns></returns>
        private IEnumerator FadeToAlpha(float time, float targetAlpha)
        {
            float startingAlpha = fadeToBlack.color.a;
            float timer = 0;
            while (timer <= time)
            {
                float normalizedTime = timer / time;
                float alpha = Mathf.Lerp(startingAlpha, targetAlpha, normalizedTime);
                fadeToBlack.color = SetAlpha(fadeToBlack.color, alpha);

                timer += Time.deltaTime;
                yield return null;
            }
        }

        /// <summary>
        /// Sets the alpha value of a color.
        /// </summary>
        /// <param name="color"></param>
        /// <param name="alpha"></param>
        /// <returns></returns>
        public static Color SetAlpha(Color color, float alpha)
        {
            color.a = alpha;
            return color;
        }
    }
}
