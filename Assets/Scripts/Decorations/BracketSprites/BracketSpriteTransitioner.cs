/*****************************************************************************
// File Name : BracketSpriteTransitioner.cs
// Author : Brandon Koederitz
// Creation Date : 10/16/2025
// Last Modified : 10/16/2026
//
// Brief Description : Controls if a specific set of bracket sprites have transitioned to a new bracket or not.
*****************************************************************************/
using Snowmentum.Size;
using UnityEngine;

namespace Snowmentum
{
    public class BracketSpriteTransitioner : MonoBehaviour
    {
        private bool hasTransitioned = true;

        #region Properties
        public bool HasTransitioned
        {
            get { return hasTransitioned; }
            set { hasTransitioned = value; }
        }
        #endregion

        /// <summary>
        /// Subscribe/Unsubscribe to events.
        /// </summary>
        private void Awake()
        {
            SizeBracket.OnBracketChanged += ResetHasTransitioned;
        }
        private void OnDestroy()
        {
            SizeBracket.OnBracketChanged -= ResetHasTransitioned;
        }

        /// <summary>
        /// Resets hasTransitioned when the bracket changes.
        /// </summary>
        /// <param name="bracket"></param>
        private void ResetHasTransitioned(int bracket)
        {
            hasTransitioned = false;
        }
    }
}
