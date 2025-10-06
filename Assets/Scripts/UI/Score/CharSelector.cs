/*****************************************************************************
// File Name : CharSelector.cs
// Author : Brandon Koederitz
// Creation Date : 10/4/2025
// Last Modified : 10/4/2025
//
// Brief Description : Allows the player to select a given char for their initials by scrolling the trackball.
*****************************************************************************/
using System;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;

namespace Snowmentum
{
    public class CharSelector : InitialsInputComponent
    {
        #region CONSTS
        private const float LERP_SNAP_LEEWAY = 0.001f;
        #endregion

        [SerializeField] private string validCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        [SerializeField] private TMP_Text[] charTexts;
        [SerializeField, Tooltip("Determines how each text element at each position looks when scrolling.")] 
        private TextVizData[] displaySettings; // This should not be changed at runtime.
        [SerializeField] private float lerpSpeed;

        private int selectedTextIndex;
        private int charIndex;
        private Coroutine moveRoutine;

        #region Properties
        private int CharIndex
        {
            get { return charIndex; }
            set
            {
                charIndex = value;

                // Loop the char index around if is beyond the bounds of our valid characters.
                if (charIndex < 0)
                {
                    charIndex = validCharacters.Length - 1;
                }
                else if (charIndex >= validCharacters.Length)
                {
                    charIndex = 0;
                }

                //charDisplayTexts.text = validCharacters[charIndex].ToString();
            }
        }
        #endregion

        #region Nested
        [System.Serializable]
        private struct TextVizData
        {
            [SerializeField] internal float yPos;
            [SerializeField] internal float fontSize;
            [SerializeField] internal float alpha;

            [SerializeField] internal bool isSelectedChar;
        }
        #endregion

        /// <summary>
        /// Set up all text character displays  with the correct character based on the character that is selected
        /// by default.
        /// </summary>
        private void Awake()
        {
            selectedTextIndex = Array.FindIndex(displaySettings, item => item.isSelectedChar);

            // Set all text displays to their starting char.
            for (int i = 0; i < charTexts.Length; i++)
            {
                UpdateChar(charTexts[i], i);
            }
        }

        /// <summary>
        /// Reads the selected character
        /// </summary>
        /// <returns></returns>
        public char ReadChar()
        {
            return validCharacters[CharIndex];
        }

        /// <summary>
        /// Scroll the currently selected char by a given amount when the player inputs vertically on the trackball.
        /// </summary>
        /// <param name="inputAmount"></param>
        public override void OnVerticalInput(int inputAmount)
        {
            CharIndex += inputAmount;
            ScrollDisplay(inputAmount);
        }

        #region Char Display Scrolling
        /// <summary>
        /// Scrolls the text component that display the characters.
        /// </summary>
        /// <param name="inputAmount"></param>
        private void ScrollDisplay(int inputAmount)
        {
            TMP_Text[] tempArray = new TMP_Text[charTexts.Length];
            int newIndex;
            // Loop through all display texts.
            for (int i = 0; i < charTexts.Length; i++)
            {
                // Move the display texts to a new index position.
                newIndex = i - inputAmount;

                bool didLoop = CollectionHelpers.LoopIndex(charTexts, ref newIndex);
                if (didLoop)
                {
                    UpdateChar(charTexts[i], newIndex);
                }

                tempArray[newIndex] = charTexts[i];
            }

            // Overwrite our charTexts array with the new positions.
            charTexts = tempArray;

            // Move all of the texts to their new positions.
            if (moveRoutine != null)
            {
                StopCoroutine(moveRoutine);
                moveRoutine = null;
            }
            moveRoutine = StartCoroutine(MoveRoutine(lerpSpeed));
        }

        /// <summary>
        /// Update the char displayed by a given text component 
        /// </summary>
        /// <param name="textIndex">The index of the text component.</param>
        private void UpdateChar(TMP_Text updateText, int textIndex)
        {
            int index = charIndex + (textIndex - selectedTextIndex);
            CollectionHelpers.LoopIndex(validCharacters, ref index);
            updateText.text = validCharacters[index].ToString();
        }

        /// <summary>
        /// Continually moves the texts that display the different possible characters to their new positions.
        /// </summary>
        /// <returns></returns>
        private IEnumerator MoveRoutine(float lerpSpeed)
        {
            bool[] completedRoutines = new bool[charTexts.Length];
            float step;
            TextVizData currentData;
            while (completedRoutines.Contains(false))
            {
                // Calculate the lerp step for this iteration so that it's framerate independent.
                step = 1 - (Mathf.Pow(0.5f, lerpSpeed * Time.deltaTime));
                for (int i = 0; i < charTexts.Length; i++)
                {
                    if (completedRoutines[i]) { continue; }

                    // Lerp the values of the current text object towards it's target data.
                    currentData = GetData(charTexts[i]);
                    currentData = LerpData(currentData, displaySettings[i], step);

                    // If the text object's data is very close to the target settings, then snap it to the target
                    // settings and mark it as complete.
                    if (CompareTextData(currentData, displaySettings[i]))
                    {
                        completedRoutines[i] = true;
                        SetData(charTexts[i], displaySettings[i]);
                    }
                    else
                    {
                        SetData(charTexts[i], currentData);
                    }
                }
                yield return null;
            }

            moveRoutine = null;
        }
        #endregion

        #region TextVisData Helpers
        /// <summary>
        /// Checks if two TextVizDatas are approximately the same.
        /// </summary>
        /// <param name="data1"></param>
        /// <param name="data2"></param>
        /// <returns></returns>
        private static bool CompareTextData(TextVizData data1, TextVizData data2)
        {
            bool isApprox = true;
            isApprox &= MathHelpers.ApproximatelyWithin(data1.yPos, data2.yPos, LERP_SNAP_LEEWAY);
            isApprox &= MathHelpers.ApproximatelyWithin(data1.fontSize, data2.fontSize, LERP_SNAP_LEEWAY);
            isApprox &= MathHelpers.ApproximatelyWithin(data1.alpha, data2.alpha, LERP_SNAP_LEEWAY);
            return isApprox;
        }

        /// <summary>
        /// Extracts a TextVisData from a given TMP_Text component.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private static TextVizData GetData(TMP_Text text)
        {
            TextVizData result = new TextVizData();
            result.yPos = ((RectTransform)text.transform).anchoredPosition.y;
            result.fontSize = text.fontSize;
            result.alpha = text.alpha;
            return result;
        }

        /// <summary>
        /// Sets the values of the TMP_Text object equal to the values stored in a TextVisData struct.
        /// </summary>
        /// <param name="toSet"></param>
        /// <param name="data"></param>
        private static void SetData(TMP_Text toSet, TextVizData data)
        {
            toSet.fontSize = data.fontSize;
            toSet.alpha = data.alpha;

            RectTransform rTrans = (RectTransform)toSet.transform;
            Vector2 anchPos = rTrans.anchoredPosition;
            anchPos.y = data.yPos;
            rTrans.anchoredPosition = anchPos;
        }

        /// <summary>
        /// Lerps the values of a TextVizData struct towards the values of another TVD struct.
        /// </summary>
        /// <param name="baseData"></param>
        /// <param name="step"></param>
        /// <returns></returns>
        private static TextVizData LerpData(TextVizData baseData, TextVizData targetData, float step)
        {
            baseData.yPos = Mathf.Lerp(baseData.yPos, targetData.yPos, step);
            baseData.fontSize = Mathf.Lerp(baseData.fontSize, targetData.fontSize, step);
            baseData.alpha = Mathf.Lerp(baseData.alpha, targetData.alpha, step);
            return baseData;
        }
        #endregion

        #region Editor Helpers
        /// <summary>
        /// Automatically assigns values to the display settings array based on the values on the text components.
        /// </summary>
        [ContextMenu("Autofill Display Settings")]
        private void AutofillDisplaySettings()
        {
            displaySettings = new TextVizData[charTexts.Length];
            for (int i = 0; i < displaySettings.Length; i++)
            {
                displaySettings[i] = GetData(charTexts[i]);
            }
        }
        #endregion
    }
}
