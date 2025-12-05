/*****************************************************************************
// File Name : ScoreIncrementer.cs
// Author : Brandon Koederitz
// Creation Date : 9/22/2025
// Last Modified : 9/22/2025
//
// Brief Description : Componennt that allows for incrementing the player's score.
*****************************************************************************/
using UnityEngine;

namespace Snowmentum.Score
{
    public class ScoreIncrementer : MonoBehaviour
    {
        [SerializeField] private int baseScoreIncrease;

        #region Properties
        public int BaseScore
        {
            get { return baseScoreIncrease; }
            set { baseScoreIncrease = value; }
        }
        #endregion

        /// <summary>
        /// Increases the players score by a certain amount.
        /// </summary>
        /// <param name="toAdd"></param>
        public void AddScore(int toAdd)
        {
            ScoreStatic.Score += toAdd;
            //if (gainScoreAnimation != null)
            //{
            //    gainScoreAnimation.PlayAnimation(toAdd);
            //}
            ScorePopupManager.DisplayScore(UIHelpers.ArcadeFormat(toAdd, 0, 1), transform.position);

        }

        public void AddBaseScore()
        {
            AddScore(baseScoreIncrease);
        }

        /// <summary>
        /// Adds this object's base score multiplied by a given value to 
        /// </summary>
        /// <param name="multiplier"></param>
        public void AddScoreMultiplied(float multiplier)
        {
            //Debug.Log("Adding Score multiplied by " + multiplier);
            AddScore(Mathf.CeilToInt(baseScoreIncrease * multiplier));
        }
    }
}
