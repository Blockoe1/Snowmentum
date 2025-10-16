/*****************************************************************************
// File Name : BracketSpriite.cs
// Author : Brandon Koederitz
// Creation Date : 10/16/2025
// Last Modified : 10/16/2025
//
// Brief Description : Updates the sprite on this object's sprite renderer based on the current bracket.
*****************************************************************************/
using Snowmentum.Size;
using UnityEngine;
using UnityEngine.Events;

namespace Snowmentum
{
    public class BracketSprite : MonoBehaviour
    {
        [SerializeField] protected Sprite[] bracketSprites;
        [SerializeField] private UnityEvent<int> OnNewSprite;

        protected int currentIndex;

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
        /// Gets the index of the sprite for a given bracket.
        /// </summary>
        /// <param name="bracket"></param>
        /// <returns></returns>
        protected virtual int GetSpriteIndex(int bracket)
        {
            int index = Mathf.Clamp(bracket - 1, 0, bracketSprites.Length - 1);
            // Back up the index to find the first non-null sprite.  That is the sprite for this current bracket.
            while (bracketSprites[index] == null && index > 0)
            {
                index--;
            }
            return index;
        }

        /// <summary>
        /// Sets the sprite to a new sprite with a given index.
        /// </summary>
        /// <param name="sprite"></param>
        /// <param name="index"></param>
        protected void SetSprite(Sprite sprite, int index)
        {
            rend.sprite = sprite;
            currentIndex = index;
            //Debug.Log("Updated Sprite");
            // Broadcast out that a new sprite is being used, but convert it back to it's relevant bracket.
            OnNewSprite?.Invoke(currentIndex + 1);
        }

        /// <summary>
        /// Updates the sprite to reflect the current bracket.
        /// </summary>
        public virtual void UpdateSprite()
        {
            int index = GetSpriteIndex(SizeBracket.Bracket);

            // Only update sprites if we're getting a new sprite index.
            if (index != currentIndex)
            {
                SetSprite(bracketSprites[index], index);
            }
        }
    }
}
