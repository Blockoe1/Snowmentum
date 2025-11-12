/*****************************************************************************
// File Name : TransitionManager.cs
// Author : Brandon Koederitz
// Creation Date : 11/5/2025
// Last Modified : 11/5/2025
//
// Brief Description : Controls scene transitions between different scenes.
*****************************************************************************/
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Snowmentum
{
    public enum TransitionType
    {
        Snowy,
        FadeToWhite,
        FadeToBlack
    }

    public class TransitionManager : MonoBehaviour
    {
        [SerializeField] private ParticleSystem transitionParticles;
        [SerializeField] private Image fadeToWhite;
        [SerializeField] private Image fadeToBlack;
        [SerializeField] private float fadeTime;
        [SerializeField] private UnityEvent OnTransitionEvent;

        private static TransitionManager instance;
        private bool isTransitioning;

        /// <summary>
        /// Setup the transition manager on game start.
        /// </summary>
        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                Debug.Log("Destroyed duplicate TransitionManager.");
            }
            else
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }

        /// <summary>
        /// Transitions to a new scene.
        /// </summary>
        /// <param name="sceneName"></param>
        public static void LoadScene(string sceneName, TransitionType tType = TransitionType.FadeToBlack)
        {
            // Prevent scene loading if we are already in the middle of a transition.
            if (instance != null && instance.isTransitioning) { return; }
            if (instance != null)
            {
                // If there is a TransitionManager, use it to transition to the new scene.
                switch (tType)
                {
                    case TransitionType.Snowy:
                        instance.StartCoroutine(instance.TransitionToScene(sceneName, instance.fadeToWhite));
                        // snowy transition plays extra particles.
                        instance.transitionParticles.Play();
                        break;
                    case TransitionType.FadeToWhite:
                        instance.StartCoroutine(instance.TransitionToScene(sceneName, instance.fadeToWhite));
                        break;
                    case TransitionType.FadeToBlack:
                    default:
                        instance.StartCoroutine(instance.TransitionToScene(sceneName, instance.fadeToBlack));
                        break;
                }
            }
            else
            {
                // If there is no TransitionManager, then just load the scene directly with SceneManagement.
                SceneManager.LoadScene(sceneName);
            }
        }

        /// <summary>
        /// Transitions to a new scene.
        /// </summary>
        /// <param name="sceneName"></param>
        private IEnumerator TransitionToScene(string sceneName, Image fadeImage)
        {
            Debug.Log("Transitioning.");
            isTransitioning = true;

            OnTransitionEvent?.Invoke();

            fadeImage.gameObject.SetActive(true);
            SetImageAlpha(fadeImage, 0);
            yield return StartCoroutine(FadeImageTo(fadeImage, 1));

            // Load the scene asyncronously and wait until it's loaded.
            AsyncOperation loadingOp = SceneManager.LoadSceneAsync(sceneName);
            // Wait until we've loaded the scene.
            yield return new WaitWhile(() => !loadingOp.isDone);

            yield return StartCoroutine(FadeImageTo(fadeImage, 0));
            fadeImage.gameObject.SetActive(false);

            isTransitioning = false;
        }

        /// <summary>
        /// Fades the image to a given alpha.
        /// </summary>
        /// <param name="targetAlpha"></param>
        /// <returns></returns>
        private IEnumerator FadeImageTo(Image img, int targetAlpha)
        {
            float step = Mathf.Abs((targetAlpha - img.color.a) / fadeTime * Time.unscaledDeltaTime);
            //Debug.Log(step);

            while (img.color.a != targetAlpha)
            {
                SetImageAlpha(img, Mathf.MoveTowards(img.color.a, targetAlpha, step));
                yield return null;
            }
        }

        /// <summary>
        /// Sets the alpha of the transition image.
        /// </summary>
        /// <param name="alpha"></param>
        private void SetImageAlpha(Image img, float alpha)
        {
            Color col = img.color;
            col.a = alpha;
            img.color = col;
        }
    }
}
