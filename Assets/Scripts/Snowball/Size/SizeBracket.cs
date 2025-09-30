/*****************************************************************************
// File Name : SizeBracket.cs
// Author : Brandon Koederitz
// Creation Date : 9/29/2025
// Last Modified : 9/29/2025
//
// Brief Description : Controls the current size bracket that gameplay is in.
*****************************************************************************/
using System;
using UnityEngine;

namespace Snowmentum.Size
{
    public static class SizeBracket
    {
        #region CONSTS
        // How much each bracket scales up by.
        public const float BRACKET_SCALE = 2;
        // The maximum size held by bracket one.
        public const float BASE_BRACKET_MAX = 4;
        #endregion

        private static int bracket;

        public static event Action<int> OnBracketChanged;

        private static float sizeMin;
        private static float sizeMax;

        #region Properties
        public static int Bracket
        {
            get { return bracket; }
            set
            {
                // Bracket must be at least 1
                bracket = Mathf.Max(1, value);
                OnBracketChanged?.Invoke(bracket);

                // Update the size limits of our bracket.
                sizeMin = GetMaxSize(bracket - 1);
                sizeMax = GetMaxSize(bracket);
            }
        }
        #endregion

        /// <summary>
        /// Calculataes the bracket that the snowball falls into based on it's current size.
        /// </summary>
        /// <param name="size">The current size of the snowball.</param>
        /// <returns>It's current size bracket.</returns>
        private static int GetBracket(float size)
        {
            return Mathf.CeilToInt(Mathf.Log(size, BRACKET_SCALE));
        }

        /// <summary>
        /// Calculates the maximum size allowed in a size bracket.
        /// </summary>
        /// <param name="bracket">The size bracket to get the max size of.</param>
        /// <returns>The maximum snowball size allowed in that bracket.</returns>
        private static float GetMaxSize(int bracket)
        {
            return BASE_BRACKET_MAX * Mathf.Pow(BRACKET_SCALE, bracket - 2);
        }

        /// <summary>
        /// Updates the current bracket of the snowball.
        /// </summary>
        /// <param name="snowballSize">The size of the snowball</param>
        public static void UpdateBracket(float snowballSize)
        {
            // Only change our bracket if we are beyond the pre-calculated limits of our current bracket.
            if (snowballSize < sizeMin || snowballSize > sizeMax)
            {
                Bracket = GetBracket(snowballSize);
            }
        }
    }
}
