/*****************************************************************************
// File Name : CharSelector.cs
// Author : Brandon Koederitz
// Creation Date : 10/4/2025
// Last Modified : 10/4/2025
//
// Brief Description : Allows the player to select a given char for their initials by scrolling the trackball.
*****************************************************************************/
using TMPro;
using UnityEngine;

namespace Snowmentum
{
    public class CharSelector : InitialsInputComponent
    {
        [SerializeField] private string validCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        [SerializeField] private TMP_Text charDisplayText;

        private int charIndex;

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

                charDisplayText.text = validCharacters[charIndex].ToString();
            }
        }
        #endregion

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
        }
    }
}
