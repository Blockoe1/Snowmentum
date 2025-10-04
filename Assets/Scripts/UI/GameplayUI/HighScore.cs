/*****************************************************************************
// File Name : HighScore.cs
// Author :
// Creation Date : 
// Last Modified : 
//
// Brief Description : 
*****************************************************************************/
using UnityEngine;

namespace Snowmentum.Score
{
    public struct HighScore
    {
        #region Consts
        private const string DEFAULT_INITIALS = "AAA";
        #endregion

        public string initials;
        public int value;

        #region Properties
        public static HighScore Default
        {
            get { return new HighScore(DEFAULT_INITIALS, 0); }
        }
        #endregion

        public HighScore(string initials, int value)
        {
            this.initials = initials;
            this.value = value;
        }

    }
}
