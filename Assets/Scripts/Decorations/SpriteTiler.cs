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
        [SerializeField, Tooltip("If set to true, then the object will also be scaled based on the bracket.")] 
        private bool scale;
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

        /// <summary>
        /// Susbscribe/Unsubscribe events.
        /// </summary>
        private void Awake()
        {
            SizeBracket.OnBracketChanged += UpdateTiling;
            baseSize = rend.size;
            baseScale = transform.localScale;
        }
        private void OnDestroy()
        {
            SizeBracket.OnBracketChanged -= UpdateTiling;
        }

        /// <summary>
        /// Updates the width of this sprite when the bracket changes.
        /// </summary>
        /// <param name="bracket"></param>
        private void UpdateTiling(int bracket)
        {
            // The width and height of this sprite renderer should be set to the min size of the bracket.
            float minSize = SizeBracket.GetMinSize(bracket);

            // Update the size of the sprite renderer.
            Vector2 size = rend.size;
            size.x = baseSize.x * minSize;
            size.y = baseSize.y * minSize;
            rend.size = size;

            // Update the scale of the transform.
            if (scale)
            {
                transform.localScale = baseScale / minSize;
            }
        }
    }
}
