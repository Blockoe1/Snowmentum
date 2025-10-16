/*****************************************************************************
// File Name : BracketSprite.cs
// Author : 10/16/2025
// Creation Date : 10/16/2025
// Last Modified : 10/16/2025
//
// Brief Description : Updates the sprite of an object based on the current size bracket.
*****************************************************************************/
using Snowmentum.Size;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Snowmentum
{
    public class TransitionedBracketSprite : BracketSprite
    {
        [Header("Transition Settings")]
        [SerializeField] private Sprite[] transitionSprites;

        private bool usedInTransition;

        #region Component References    
        [SerializeReference] protected BracketSpriteTransitioner transitioner;

        /// <summary>
        /// Get components on reset.
        /// </summary>
        [ContextMenu("Get Component References")]
        private void Reset()
        {
            transitioner = GetComponentInParent<BracketSpriteTransitioner>();
        }
        #endregion

        /// <summary>
        /// Updates the sprite to reflect the current bracket.
        /// </summary>
        public override void UpdateSprite()
        {
            int index = GetSpriteIndex(SizeBracket.Bracket);

            // Only update sprites if we're getting a new sprite index or we were used for transitioning last
            // update.
            if (index != currentIndex || usedInTransition)
            {
                //Debug.Log(transitioner.HasTransitioned);
                // If we've havent transitioned to the next bracket yet, use the transition sprite.
                if (transitioner != null && !transitioner.HasTransitioned)
                {
                    transitioner.HasTransitioned = true;
                    // True for going up in bracket, false for going down.
                    bool transitionDirection = Math.Sign(index - currentIndex) > 0;

                    // If we're going up to a new bracket, use the index of that new bracket.
                    // If we're going down to a previous bracket, use the transition sprite for our current
                    // bracket but in reverse.
                    int transitionSpriteIndex = transitionDirection ? index : currentIndex;

                    if (CollectionHelpers.IndexInRange(transitionSprites, transitionSpriteIndex) && 
                        transitionSprites[transitionSpriteIndex] != null)
                    {
                        SetSprite(transitionSprites[transitionSpriteIndex], transitionSpriteIndex);

                        // Flip the sprite renderer on X if we're making a transition to a lower bracket.
                        rend.flipX = transitionDirection;

                        usedInTransition = true;
                    }
                }
                else
                {
                    rend.flipX = false;
                    SetSprite(bracketSprites[index], index);
                    usedInTransition = false;
                }
            }
        }
    }
}
