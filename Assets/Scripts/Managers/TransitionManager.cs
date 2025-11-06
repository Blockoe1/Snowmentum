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
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Snowmentum
{
    public class TransitionManager : MonoBehaviour
    {
        [SerializeField] private ParticleSystem transitionParticles;
        [SerializeField] private Image transitionImage;
        [SerializeField] private float fadeTime;

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
        public static void LoadScene(string sceneName)
        {
            if (instance != null)
            {
                // If there is a TransitionManager, use it to transition to the new scene.
                //Task.Run(async () => await instance.TransitionToScene(sceneName));
                instance.StartCoroutine(instance.TransitionToScene(sceneName));
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
        private IEnumerator TransitionToScene(string sceneName)
        {
            Debug.Log("Transitioning.");
            isTransitioning = true;

            // Play the particles for transition.
            //transitionParticles.Play();

            transitionImage.gameObject.SetActive(true);
            SetImageAlpha(0);
            yield return StartCoroutine(FadeImageTo(1));

            // Load the scene asyncronously and wait until it's loaded.
            AsyncOperation loadingOp = SceneManager.LoadSceneAsync(sceneName);
            // Wait until we've loaded the scene.
            yield return new WaitWhile(() => !loadingOp.isDone);

            yield return StartCoroutine(FadeImageTo(0));
            transitionImage.gameObject.SetActive(false);

            isTransitioning = false;
        }

        /// <summary>
        /// Fades the image to a given alpha.
        /// </summary>
        /// <param name="targetAlpha"></param>
        /// <returns></returns>
        private IEnumerator FadeImageTo(int targetAlpha)
        {
            float step = Mathf.Abs((targetAlpha - transitionImage.color.a) / fadeTime * Time.unscaledDeltaTime);
            Debug.Log(step);

            while (transitionImage.color.a != targetAlpha)
            {
                SetImageAlpha(Mathf.MoveTowards(transitionImage.color.a, targetAlpha, step));
                yield return null;
            }
        }

        /// <summary>
        /// Sets the alpha of the transition image.
        /// </summary>
        /// <param name="alpha"></param>
        private void SetImageAlpha(float alpha)
        {
            Color col = transitionImage.color;
            col.a = alpha;
            transitionImage.color = col;
        }
    }
}
