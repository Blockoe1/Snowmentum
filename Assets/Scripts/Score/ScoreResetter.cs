/*****************************************************************************
// File Name : ScoreResetter.cs
// Author : Brandon Koederitz
// Creation Date : 10/5/2025
// Last Modified : 10/5/2025
//
// Brief Description : Simple script that resets the current score on load.
*****************************************************************************/
using Snowmentum.Score;
using UnityEngine;

namespace Snowmentum
{
    public class ScoreResetter : MonoBehaviour
    {
        private void Awake()
        {
            // Reset score whenever the game begins.
            ScoreStatic.Score = 0;
        }
    }
}
