/*****************************************************************************
// File Name : Score.cs
// Author : Brandon Koederitz
// Creation Date : 9/22/2025
// Last Modified : 9/22/2025
//
// Brief Description : Script for holding the player's current score.
*****************************************************************************/
using System;
using UnityEngine;

namespace Snowmentum.Score
{
    public static class ScoreStatic
    {
        private static int score;
        public static event Action<int> OnScoreUpdated;

        #region Properties
        public static int Score
        {
            get
            {
                return score;
            }
            set
            {
                score = value;
                OnScoreUpdated?.Invoke(score);
            }
        }
        #endregion
    }
}
