/*****************************************************************************
// File Name : HighScoreSavingTest.cs
// Author :
// Creation Date : 
// Last Modified : 
//
// Brief Description : 
*****************************************************************************/
using Snowmentum.Score;
using UnityEngine;

namespace Snowmentum
{
    public class HighScoreSavingTest : MonoBehaviour
    {
        [SerializeField] private ScoreSaver scoreManager;
        [SerializeField] private int scoreToTest;


        private void Start()
        {
            ScoreStatic.Score = scoreToTest;
            scoreManager.SaveNewHighScore();
        }
    }
}
