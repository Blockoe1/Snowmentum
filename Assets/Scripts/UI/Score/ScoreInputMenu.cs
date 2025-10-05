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
using System.Linq;

namespace Snowmentum.UI
{
    public class ScoreInputMenu : MonoBehaviour
    {
        [SerializeReference] private ScoreInputComponent[] components;
        [SerializeField] private RectTransform selectionIndicator;
        [SerializeField, Tooltip("The minimum delta value that players have to exceed to confirm they want to " +
            "change the current selection.")] 
        private float inputThreshold = 7500;
        [SerializeField, Tooltip("The amount of delay that should be waited between each time we detect input.")] 
        private float inputDelay;

        private int selectedIndex;
        private bool pauseInput;

        #region Properties
        private int SelectedSelectorIndex
        {
            get { return selectedIndex; }
            set
            {
                // Deselect our previously selected component.
                if (selectedIndex > 0 && selectedIndex < components.Length && components[selectedIndex] != null)
                {
                    components[selectedIndex].OnDeselect();
                }

                selectedIndex = value;

                // Loop the char index around if is beyond the bounds of our valid characters.
                if (selectedIndex < 0)
                {
                    selectedIndex = components.Length - 1;
                }
                else if (selectedIndex >= components.Length)
                {
                    selectedIndex = 0;
                }

                // Select our current component
                // Deselect our previously selected component.
                if (components[selectedIndex] != null)
                {
                    components[selectedIndex].OnSelect();
                }

                // Update any visuals here.
                selectionIndicator.transform.position = components[selectedIndex].transform.position;
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
                components[selectedIndex].OnVerticalInput(Math.Sign(delta.y));

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
            foreach (ScoreInputComponent component in components)
            {
                if (component is CharSelector charSelector)
                {
                    output += charSelector.ReadChar();
                }
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
