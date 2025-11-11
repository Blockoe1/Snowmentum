/*****************************************************************************
// File Name : SizeBracket.cs
// Author : Brandon Koederitz
// Creation Date : 9/29/2025
// Last Modified : 9/29/2025
//
// Brief Description : Controls the current size bracket that gameplay is in.
*****************************************************************************/
using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Snowmentum.Size
{
    public static class SizeBracket
    {
        #region CONSTS
        // The maximum size held by bracket one.
        public const float BRACKET_SCALE = 2;
        public const float SMALLEST_BRACKET_LIMIT = 1;
        #endregion

        private static int bracket;

        public static event Action<int, int> OnBracketChanged;

        private static float sizeMin;
        private static float sizeMax;

        #region Properties
        public static int Bracket
        {
            get { return bracket; }
            set
            {
                int oldBracket = bracket;
                // Bracket must be at least 1
                bracket = Mathf.Max(1, value);
                Debug.Log("Size bracket is now: " + bracket);
                // Update the size limits of our bracket.
                sizeMin = GetMaxSize(bracket - 1);
                sizeMax = GetMaxSize(bracket);

                OnBracketChanged?.Invoke(bracket, oldBracket);   
            }
        }
        #endregion

        /// <summary>
        /// Calculataes the bracket that the snowball falls into based on it's current size.
        /// </summary>
        /// <param name="size">The current size of the snowball.</param>
        /// <returns>It's current size bracket.</returns>
        public static int GetBracket(float size)
        {
            return Mathf.Max(Mathf.CeilToInt(Mathf.Log(size, BRACKET_SCALE)), 1);
        }

        /// <summary>
        /// Calculates the maximum size allowed in a size bracket.
        /// </summary>
        /// <param name="bracket">The size bracket to get the max size of.</param>
        /// <returns>The maximum snowball size allowed in that bracket.</returns>
        public static float GetMaxSize(int bracket)
        {
            return Mathf.Pow(BRACKET_SCALE, bracket);
        }
        public static float GetMaxSize()
        {
            return GetMaxSize(bracket);
        }

        /// <summary>
        /// Calculates the minimum size allowed in a size bracket.
        /// </summary>
        /// <param name="bracket">The size bracket to get the max size of.</param>
        /// <returns>The maximum snowball size allowed in that bracket.</returns>
        public static float GetMinSize(int bracket)
        {
            return GetMaxSize(bracket - 1);
        }
        public static float GetMinSize()
        {
            return GetMinSize(bracket);
        }

        /// <summary>
        /// Updates the current bracket of the snowball.
        /// </summary>
        /// <param name="snowballSize">The size of the snowball</param>
        public static void UpdateBracket(float snowballSize)
        {
            // Only change our bracket if we are beyond the pre-calculated limits of our current bracket.
            // If the snowball is lower than size 1, that's too small for any bracket.
            if (snowballSize > SMALLEST_BRACKET_LIMIT && snowballSize < sizeMin || snowballSize > sizeMax)
            {
                Bracket = GetBracket(snowballSize);
            }
        }
    }
}
