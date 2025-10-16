/*****************************************************************************
// File Name : BracketSprite.cs
// Author : 10/16/2025
// Creation Date : 10/16/2025
// Last Modified : 10/16/2025
//
// Brief Description : Updates the sprite of an object based on the current size bracket.
*****************************************************************************/
using Snowmentum.Size;
using UnityEngine;

namespace Snowmentum
{
    public class BracketSprite : MonoBehaviour
    {
        [SerializeField] private Sprite[] bracketSprites;
        [SerializeField] private Sprite[] transitionSprites;

        private static bool hasTransitioned = true;

        #region Component References    
        [Header("Components")]
        [SerializeReference] protected SpriteRenderer rend;

        /// <summary>
        /// Get components on reset.
        /// </summary>
        [ContextMenu("Get Component References")]
        private void Reset()
        {
            rend = GetComponent<SpriteRenderer>();
        }
        #endregion

        /// <summary>
        /// Updates the sprite to reflect the current bracket.
        /// </summary>
        public void UpdateSprite()
        {
            // Subtract 1 to convert the bracket to an index.
            if (hasTransitioned)
            {
                rend.sprite = bracketSprites[SizeBracket.Bracket - 1];
            }
            else
            {
                rend.sprite = transitionSprites[SizeBracket.Bracket - 1];
                hasTransitioned = true;
            }
        }
    }
}
