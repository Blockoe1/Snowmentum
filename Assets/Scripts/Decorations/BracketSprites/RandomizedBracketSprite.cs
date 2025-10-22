/*****************************************************************************
// File Name : RandomizedBracketSprite.cs
// Author : Brandon Koederitz
// Creation Date : 10/16/2025
// Last Modified : 10/16/2025
//
// Brief Description : Updates the sprite on this object's sprite renderer from a random list based on the 
current bracket.
*****************************************************************************/
using Snowmentum.Size;
using UnityEngine;

namespace Snowmentum
{
    public class RandomizedBracketSprite : BracketSprite
    {
        [SerializeField] private bool updateOnEnable;
        [SerializeField] private SpriteGroup[] spriteGroups;

        private void OnEnable()
        {
            if (updateOnEnable)
            {
                UpdateSprite();
            }
        }

        /// <summary>
        /// Uses sprite groups when getting the index of the sprite.
        /// </summary>
        /// <param name="bracket"></param>
        /// <returns></returns>
        protected override int GetSpriteIndex(int bracket)
        {
            int index = Mathf.Clamp(bracket - 1, 0, spriteGroups.Length - 1);
            // Back up the index to find the first non-null sprite.  That is the sprite for this current bracket.
            while (spriteGroups[index] == null && index > 0)
            {
                index--;
            }
            return index;
        }

        /// <summary>
        /// Updates the sprite to a random one from a sprite group.
        /// </summary>
        public override void UpdateSprite()
        {
            int index = GetSpriteIndex(SizeBracket.Bracket);

            // Dont check for a new bracket as we have to update the sprite every time this runs.
            SetSprite(spriteGroups[index].GetRandom(), index);
        }
    }
}
