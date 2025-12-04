/*****************************************************************************
// File Name : ScoreInputMenu.cs
// Author : Brandon Koederitz
// Creation Date : 10/4/2025
// Last Modified : 10/4/2025
//
// Brief Description : Controls players inputting their high score to be saved on the machine.
*****************************************************************************/
using Snowmentum.Score;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Snowmentum.UI
{
    public class InitialsInputMenu : MonoBehaviour
    {
        [SerializeReference] private InitialsInputComponent[] components;
        [SerializeField] private string validCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        [SerializeField] private RectTransform selectionIndicator;
        [SerializeField, Tooltip("The minimum delta value that players have to exceed to confirm they want to " +
            "change the current selection.")] 
        private float inputThreshold = 7500;
        [SerializeField, Tooltip("The amount of delay that should be waited between each time we detect input.")] 
        private float inputDelay;
        [SerializeField]
        private string[] censoredInitials;
        [Header("Events")]
        [SerializeField] private UnityEvent OnScroll;
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

        #region Nested
        private struct CharBan
        {
            internal int charIndex;
            internal char character;
        }
        #endregion

        /// <summary>
        /// Subscribe/Unsubscribe input functions.
        /// </summary>
        private void OnEnable()
        {
            InputManager.OnDeltaUpdate += HandleMouseInput;
        }
        private void OnDisable()
        {
            InputManager.OnDeltaUpdate -= HandleMouseInput;
        }

        /// <summary>
        /// Setup char selectors and the component array on awake.
        /// </summary>
        private void Awake()
        {
            // Properly sort the array on awake.
            components = components.OrderBy(item => item.transform.position.x).ToArray();

            // Assign all char selectors the default valid characters.
            CharSelector[] charSelectors =
    CollectionHelpers.FilterToChildList<CharSelector, InitialsInputComponent>(components).ToArray();
            foreach(CharSelector charSelector in charSelectors)
            {

                charSelector.Initialize(validCharacters);
                //charSelector.RefreshDisplay();
            }
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
                HandleInput(new Vector2Int(Math.Sign(totalDelta.x), 0));

                //OnInput();
            }
            else if (Mathf.Abs(totalDelta.y) > inputThreshold)
            {
                HandleInput(new Vector2Int(0, Math.Sign(delta.y)));
                

                //OnInput();
            }
        }

        /// <summary>
        /// Handles directional input.
        /// </summary>
        /// <param name="inputDirection"></param>
        public void HandleInput(Vector2Int inputDirection)
        {
            if (pauseInput) { return; }
            if (Mathf.Abs(inputDirection.x) > 0)
            {
                // Horizontal inputs should switch which char selector is selected.
                SelectedIndex += inputDirection.x;

                OnInput();
            }
            else if (Mathf.Abs(inputDirection.y) > 0)
            {
                // Vertical inputs should increment/decrement the char selector.
                components[selectedIndex].OnVerticalInput(inputDirection.y);

                // Check for possible censored initial combinations, and disable any relevant characters.
                CheckCensoredInitials();

                // Update our current high score object that we're modifying with the new initials
                if (targetHS != null)
                {
                    targetHS.initials = GetInitials();
                }

                OnInput();
            }
        }

        /// <summary>
        /// Handles events that should happen when any kind of input occurs.
        /// </summary>
        private void OnInput()
        {
            OnScroll?.Invoke();

            StartCoroutine(InputDelay(inputDelay));

            // Reset delta on input.
            totalDelta = Vector2.zero;
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

        #region Censoring

        /// <summary>
        /// Checks and applies bans to certain character to prevent a player from entering a banned initial combination.
        /// </summary>
        private void CheckCensoredInitials()
        {
            CharSelector[] charSelectors =
                CollectionHelpers.FilterToChildList<CharSelector, InitialsInputComponent>(components).ToArray();

            // Set the default valid characters
            string[] validCharsArray = new string[charSelectors.Length];
            Array.Fill(validCharsArray, validCharacters);


            string initials = GetInitials();
            List<CharBan> bans = new List<CharBan>();
            foreach (var censoredWord in censoredInitials)
            {
                // If we've found a character we need to censor, then add it to our list of bans.
                if (CheckCensored(initials, censoredWord, out CharBan toCensor))
                {
                    bans.Add(toCensor);
                }
            }

            // Apply each ban to the respective valid characters string by removing the banned character.
            foreach(var ban in bans)
            {
                //Debug.Log($"Found censored combo of char {ban.character} at index {ban.charIndex}");
                validCharsArray[ban.charIndex] = validCharsArray[ban.charIndex].Replace(ban.character.ToString(), "");
                //Debug.Log(validCharsArray[ban.charIndex]);
            }

            // Update the char selectors.
            for (int i = 0; i < charSelectors.Length; i++)
            {
                // Display gets auto-refreshed when ValidCharacters is updated.
                charSelectors[i].ValidCharacters = validCharsArray[i];
            }
        }

        /// <summary>
        /// Checks if our current initials could match a censored word if one character changes.
        /// </summary>
        /// <param name="initials"></param>
        /// <param name="censoredWord"></param>
        private bool CheckCensored(string initials, string censoredWord, out CharBan toCensor)
        {
            int sharedCharacters = 0;
            toCensor = new CharBan();
            // Loop through the letters the initials and censored word share.
            for(int i = 0; i < initials.Length && i < censoredWord.Length; i++)
            {
                if (initials[i] == censoredWord[i])
                {
                    sharedCharacters++;
                }
                else
                {
                    toCensor.character = censoredWord[i];
                    toCensor.charIndex = i;
                }
            }
            // If the initials and censored word share more than 1 character in common, then we need to ban
            // the last remaining character in the censored word.
            if (sharedCharacters > 1)
            {
                return true;
            }
            // If they dont share 2 characters in common, no characters need to be banned.
            return false;
        }
        #endregion

        #region Debug
        [ContextMenu("Print Initials")]
        private void PrintInitials()
        {
            Debug.Log(GetInitials());
        }
        #endregion
    }
}
