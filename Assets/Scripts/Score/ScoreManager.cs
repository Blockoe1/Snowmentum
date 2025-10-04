/*****************************************************************************
// File Name : ScoreManager.cs
// Author : Brandon Koederitz
// Creation Date : 10/3/2025
// Last Modified : 10/3/2025
//
// Brief Description : Controls the loading and management of scores during gameplay
*****************************************************************************/
using Snowmentum.Score;
using UnityEngine;

namespace Snowmentum
{
    public class ScoreManager : MonoBehaviour
    {
        private void Awake()
        {
            // Reset score whenever the game begins.
            ScoreStatic.Score = 0;

            // Load the high scores when the game begins.
            ScoreStatic.LoadHighScores();
        }
    }
}
