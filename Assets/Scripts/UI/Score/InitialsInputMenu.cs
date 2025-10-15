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
using UnityEngine.Events;

namespace Snowmentum.UI
{
    public class InitialsInputMenu : MonoBehaviour
    {
        [SerializeReference] private InitialsInputComponent[] components;
        [SerializeField] private RectTransform selectionIndicator;
        [SerializeField, Tooltip("The minimum delta value that players have to exceed to confirm they want to " +
            "change the current selection.")] 
        private float inputThreshold = 7500;
        [SerializeField, Tooltip("The amount of delay that should be waited between each time we detect input.")] 
        private float inputDelay;
        [SerializeField] private UnityEvent OnSubmitInitials;

        private HighScore targetHS;
        private Vector2 totalDelta;
        private int selectedIndex;
        private bool pauseInput;

        #region Properties
        private int SelectedIndex
        {
            get { return selectedIndex; }
            set
            {
                // Deselect our previously selected component.
                if (selectedIndex > 0 && selectedIndex < components.Length && components[selectedIndex] != null)
                {
                    components[selectedIndex].OnDeselect();
                }

                selectedIndex = Mathf.Clamp(value, 0, components.Length - 1);

                // Loop the char index around if is beyond the bounds of our valid characters.
                //if (selectedIndex < 0)
                //{
                //    selectedIndex = components.Length - 1;
                //}
                //else if (selectedIndex >= components.Length)
                //{
                //    selectedIndex = 0;
                //}

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
        private void OnEnable()
        {
            InputManager.OnDeltaUpdate += HandleMouseInput;

            // Properly sort the array on awake.
            components = components.OrderBy(item => item.transform.position.x).ToArray();
        }
        private void OnDisable()
        {
            InputManager.OnDeltaUpdate -= HandleMouseInput;
        }

        /// <summary>
        /// Loads a high score to modify the initials of.
        /// </summary>
        public void LoadHighScore(HighScore toLoad)
        {
            targetHS = toLoad;
            gameObject.SetActive(true);

            // Reset total delta when we start modifying a high score.
            totalDelta = Vector2.zero;
        }

        /// <summary>
        /// When the player inputs on the trackball, either scroll the current char selector or swich to selecting
        /// a new one.
        /// </summary>
        /// <param name="delta"></param>
        private void HandleMouseInput(Vector2 delta)
        {
            if (pauseInput) { return; }
            // Increasee our stored total delta value by the delta of this frame.
            totalDelta += delta;

            // Checks if the player sufficiently moves the trackball.
            if (Mathf.Abs(totalDelta.x) > inputThreshold)
            {
                // Horizontal inputs should switch which char selector is selected.
                SelectedIndex += Math.Sign(totalDelta.x);

                StartCoroutine(InputDelay(inputDelay));

                // Reset delta on input.
                totalDelta = Vector2.zero;
            }
            else if (Mathf.Abs(totalDelta.y) > inputThreshold)
            {
                // Vertical inputs should increment/decrement the char selector.
                components[selectedIndex].OnVerticalInput(Math.Sign(delta.y));

                StartCoroutine(InputDelay(inputDelay));

                // Update our current high score object that we're modifying with the new initials
                if (targetHS != null)
                {
                    targetHS.initials = GetInitials();
                }

                // Reset delta on input.
                totalDelta = Vector2.zero;
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
            foreach (InitialsInputComponent component in components)
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
            //ScoreStatic.AddHighScore(GetInitials(), ScoreStatic.Score);

            ScoreStatic.SaveHighScores();
            OnSubmitInitials?.Invoke();
            gameObject.SetActive(false);
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
