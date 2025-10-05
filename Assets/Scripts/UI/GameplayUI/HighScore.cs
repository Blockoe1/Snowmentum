/*****************************************************************************
// File Name : HighScore.cs
// Author : Brandon Koederitz
// Creation Date : 10/5/2025
// Last Modified : 10/5/2025
//
// Brief Description : Object to store high score values.
*****************************************************************************/
using System;
using UnityEngine;

namespace Snowmentum.Score
{
    public class HighScore
    {
        #region Consts
        private const string DEFAULT_INITIALS = "AAA";
        #endregion

        private string _initials;
        private readonly int _value;

        public event Action<string> OnInitialsChanged;

        #region Properties
        public string initials
        {
            get { return _initials; }
            set 
            { 
                _initials = value; 
                OnInitialsChanged?.Invoke(initials);
            }
        }

        public int value
        {
            get { return _value; }
        }

        public static HighScore Default
        {
            get { return new HighScore(DEFAULT_INITIALS, 0); }
        }
        #endregion

        public HighScore()
        {
            initials = DEFAULT_INITIALS;
            _value = 0;
        }

        public HighScore(string initials, int value)
        {
            this.initials = initials;
            this._value = value;
        }

    }
}
