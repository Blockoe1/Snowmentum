/*****************************************************************************
// File Name : ScoreInputMenu.cs
// Author : Brandon Koederitz
// Creation Date : 10/4/2025
// Last Modified : 10/4/2025
//
// Brief Description : Controls players inputting their high score to be saved on the machine.
*****************************************************************************/
using UnityEngine;
using System;
using System.Collections;
using Snowmentum.Score;

namespace Snowmentum.UI
{
    public class ScoreInputMenu : MonoBehaviour
    {
        [SerializeField] private CharSelector[] initialSelectors;
        [SerializeField] private RectTransform selectionIndicator;
        [SerializeField] private ScoreConfirm scoreConfirm;
        [SerializeField, Tooltip("The minimum delta value that players have to exceed to confirm they want to " +
            "change the current selection.")] 
        private float inputThreshold = 7500;
        [SerializeField, Tooltip("The amount of delay that should be waited between each time we detect input.")] 
        private float inputDelay;

        private int selectedSelectorIndex;
        private bool pauseInput;

        #region Properties
        private int SelectedSelectorIndex
        {
            get { return selectedSelectorIndex; }
            set
            {
                selectedSelectorIndex = value;

                // Loop the char index around if is beyond the bounds of our valid characters.
                if (selectedSelectorIndex < 0)
                {
                    selectedSelectorIndex = initialSelectors.Length - 1;
                }
                else if (selectedSelectorIndex >= initialSelectors.Length)
                {
                    selectedSelectorIndex = 0;
                }

                // Update any visuals here.
                selectionIndicator.transform.position = initialSelectors[selectedSelectorIndex].transform.position;
            }
        }
        #endregion

        /// <summary>
        /// Subscribe/Unsubscribe input functions.
        /// </summary>
        private void Awake()
        {
            InputManager.OnDeltaUpdate += HandleMouseInput;
        }
        private void OnDestroy()
        {
            InputManager.OnDeltaUpdate -= HandleMouseInput;
        }

        /// <summary>
        /// When the player inputs on the trackball, either scroll the current char selector or swich to selecting
        /// a new one.
        /// </summary>
        /// <param name="delta"></param>
        private void HandleMouseInput(Vector2 delta)
        {
            if (pauseInput) { return; }
            // Checks if the player sufficiently moves the trackball.
            if (Mathf.Abs(delta.x) > inputThreshold)
            {
                // Horizontal inputs should switch which char selector is selected.
                SelectedSelectorIndex += Math.Sign(delta.x);

                StartCoroutine(InputDelay(inputDelay));
            }
            else if (Mathf.Abs(delta.y) > inputThreshold)
            {
                // Vertical inputs should increment/decrement the char selector.
                initialSelectors[selectedSelectorIndex].Scroll(Math.Sign(delta.y));

                StartCoroutine(InputDelay(inputDelay));
            }
        }

        /// <summary>
        /// Temporarily pauses taking input.
        /// </summary>
        /// <param name="duration"></param>
        /// <returns></returns>
        private IEnumerator InputDelay(float duration)
        {
            pauseInput = true;
            yield return new WaitForSeconds(duration);
            pauseInput = false;
        }

        /// <summary>
        /// Gets the initials of the player by reading the value of all of the initial selecters.
        /// </summary>
        /// <returns></returns>
        private string GetInitials()
        {
            string output = "";
            foreach (var selector in initialSelectors)
            {
                output += selector.ReadChar();
            }
            return output;
        }

        /// <summary>
        /// Saves the current score as a high score with the entered initials.
        /// </summary>
        public void SaveHighScore()
        {
            ScoreStatic.AddHighScore(GetInitials(), ScoreStatic.Score);
        }

        #region Debug
        [ContextMenu("Print Initials")]
        private void PrintInitials()
        {
            Debug.Log(GetInitials());
        }
        #endregion
    }
}
