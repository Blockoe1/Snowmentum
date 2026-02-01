/*****************************************************************************
// File Name : HillTiling.cs
// Author : Brandon Koederitz
// Creation Date : 10/1/2025
// Last Modified : 10/1/2025
//
// Brief Description : Increases the hill's width and height whenever the snowball changes brackets to give the
// illusion of the hill getting smaller.
*****************************************************************************/
using Snowmentum.Size;
using UnityEngine;

namespace Snowmentum
{
    public class SpriteTiler : MonoBehaviour
    {
        #region CONSTS
        // Prevents the 9-slice error by stopping size updates at sizes larger than 32.
        private const int CUTOFF_SIZE = 32;
        #endregion
        [SerializeField] private bool scaleX;
        [SerializeField] private bool scaleY;
        [SerializeField, Tooltip("The base bracket that this background should be scaled based on.")] 
        private int baseBracket = 1;
        private Vector2 baseSize;
        private Vector3 baseScale;

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

        #region Properties
        public int BaseBracket
        {
            get { return baseBracket; }
            set 
            { 
                baseBracket = value;
                // Need to immediately update the tiling of the object when baseBracket changes.
                UpdateTilingOnBracket(SizeBracket.Bracket, 0);
            }
        }
        #endregion

        /// <summary>
        /// Susbscribe/Unsubscribe events.
        /// </summary>
        private void Awake()
        {
            SizeBracket.OnBracketChanged += UpdateTilingOnBracket;
            EnvironmentSize.OnValueTransitionFinish += UpdateTilingFinishTransition;
            baseSize = rend.size;
            baseScale = transform.localScale;
        }
        private void OnDestroy()
        {
            SizeBracket.OnBracketChanged -= UpdateTilingOnBracket;
            EnvironmentSize.OnValueTransitionFinish -= UpdateTilingFinishTransition;
        }

        /// <summary>
        /// Updates the width of this sprite when the bracket changes.
        /// </summary>
        /// <param name="bracket"></param>
        private void UpdateTilingOnBracket(int bracket, int oldBracket)
        {
            // Only udpate the tiling when we reach a new bracket if we went up a bracket.
            if (oldBracket > bracket ) { return; }
            UpdateTiling(bracket);
        }

        /// <summary>
        /// Update the tiling when we finish transitioning our environment size.
        /// </summary>
        private void UpdateTilingFinishTransition()
        {
            Debug.Log("Transition finished.");
            UpdateTiling(SizeBracket.Bracket);
        }

        private void UpdateTiling(int bracket)
        {
            // The width and height of this sprite renderer should be set to the min size of the bracket.
            float minSize = SizeBracket.GetMinSize(bracket - (baseBracket - 1));

            // Prevent the "Cannot generate 9 slice" error by not tiling sprites beyond a certain environment size.
            if (minSize >= CUTOFF_SIZE) { return; }
            // Update the size of the sprite renderer.
            Vector2 size = rend.size;
            if (scaleX)
            {
                size.x = baseSize.x * minSize;
            }
            if (scaleY)
            {
                size.y = baseSize.y * minSize;
            }
            rend.size = size;
        }
    }
}
