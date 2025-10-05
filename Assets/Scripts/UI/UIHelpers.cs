/*****************************************************************************
// File Name : UIHelpers.cs
// Author : Brandon Koederitz
// Creation Date : 10/5/2025
// Last Modified : 10/5/2025
//
// Brief Description : Set of helper functions for the UI.
*****************************************************************************/
using UnityEngine;

namespace Snowmentum
{
    public class UIHelpers
    {
        /// <summary>
        /// Formats a given number to display a number of 0s before and after the digits to give it an arcade feel.
        /// </summary>
        /// <param name="num">The number to be displayed.</param>
        /// <param name="digits">The number of digits that should be displayed.</param>
        /// <param name="postfixDigits">The number of additional 0s to add to the end of the digit.</param>
        /// <returns>The formatted digit as a string.</returns>
        public static string ArcadeFormat(int num, int digits, int postfixDigits = 0)
        {
            string scoreString = num.ToString();
            //Debug.Log(scoreString);
            for (int i = scoreString.Length; i < digits; i++)
            {
                scoreString = "0" + scoreString;
            }
            // Add Postfix digits.
            for (int i = 0; i < postfixDigits; i++)
            {
                scoreString = scoreString + "0";
            }

            //Debug.Log(scoreString);
            return scoreString;
        }
    }
}
