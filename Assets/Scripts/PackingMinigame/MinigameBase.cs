/*****************************************************************************
// File Name : MinigameBase.cs
// Author : Brandon Koederitz
// Creation Date : 11/17/2025
// Last Modified : 11/17/2025
//
// Brief Description : Base class for minigame sections.
*****************************************************************************/
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Snowmentum
{
    [RequireComponent(typeof(Animator))]
    public abstract class MinigameBase : MonoBehaviour
    {
        [SerializeField, Tooltip("Divides the player's delta input by this value to reduce it to a reasonable " +
            "number.")]
        protected float scaler = 1000000;
        [SerializeField, Tooltip("The maximum value that can be output from this minigame.")] 
        protected float maxOutput;
        [SerializeField, Tooltip("The amount of time that this segment of the minigame reads player input.")] 
        protected float sampleTime;
        [SerializeField] private bool playOnAwake;
        [SerializeField, Tooltip("Event called when this segment of the minigame is complete.")] 
        private UnityEvent<float> OnMinigameComplete;

        #region Component References
        [Header("Components")]
        [SerializeReference] protected Animator anim;

        /// <summary>
        /// Get components on reset.
        /// </summary>
        [ContextMenu("Get Component References")]
        protected virtual void Reset()
        {
            anim = GetComponent<Animator>();
        }
        #endregion

        /// <summary>
        /// Start the minigame on awake if it's set.
        /// </summary>

        private void Awake()
        {
            if (playOnAwake)
            {
                StartMinigame();
            }
        }

        /// <summary>
        /// Starts the minigame.
        /// </summary>
        public void StartMinigame()
        {
            InputManager.OnDeltaUpdate += MouseUpdate;
        }

        /// <summary>
        /// Handles mouse input.
        /// </summary>
        /// <param name="delta"></param>
        protected abstract void MouseUpdate(Vector2 mouseDelta);

        /// <summary>
        /// Controls the progression of time through this minigame segment.
        /// </summary>
        /// <returns></returns>
        protected abstract IEnumerator Timer();

        /// <summary>
        /// Finishes this minigame and output's it's value.
        /// </summary>
        /// <param name="minigameQuality"></param>
        protected virtual void CompleteMinigame(float minigameQuality)
        {
            minigameQuality /= scaler;
            minigameQuality = Mathf.Min(minigameQuality, maxOutput);

            OnMinigameComplete?.Invoke(minigameQuality);

            CleanUp();
        }

        /// <summary>
        /// Cleans up this minigame when it finishes.
        /// </summary>
        protected void CleanUp()
        {
            InputManager.OnDeltaUpdate -= MouseUpdate;
        }

        /// <summary>
        /// Clean up the minigame if it is destroyed.
        /// </summary>
        private void OnDestroy()
        {
            CleanUp();
        }

    }
}
