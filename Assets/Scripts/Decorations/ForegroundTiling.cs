/*****************************************************************************
// File Name : ForegroundTiling.cs
// Author : Brandon Koederitz
// Creation Date : 9/22/2025
// Last Modified : 9/22/2025
//
// Brief Description : Scales the foreground so it appears to get smaller the greater the snowball's size value is.
*****************************************************************************/
using UnityEngine;
using Snowmentum.Size;

namespace Snowmentum.Decoration
{
    public class ForegroundTiling : MonoBehaviour
    {
        #region CONSTS
        private const int REFERENCE_SCALE = 1;
        #endregion

        private Vector2 spriteDimensions;

        #region Component References
        [Header("Components")]
        [SerializeReference] protected SpriteRenderer sRend;

        /// <summary>
        /// Get components on reset.
        /// </summary>
        [ContextMenu("Get Component References")]
        private void Reset()
        {
            sRend = GetComponent<SpriteRenderer>();
        }
        #endregion

        /// <summary>
        /// Subscribe/Unsubscribe to snowball size's OnValueChanged event so we can adjust the tiling of the foreground
        /// whenever the size changes.
        /// </summary>
        private void Awake()
        {
            spriteDimensions = sRend.size;
            SnowballSize.OnValueChanged += UpdateTiling;
        }
        private void OnDestroy()
        {
            SnowballSize.OnValueChanged -= UpdateTiling;
        }

        /// <summary>
        /// Causes the object scale up in tiling to give the appearance that the snowball is getting larger.
        /// </summary>
        /// <param name="snowballSize"></param>
        /// <param name="oldSize"></param>
        private void UpdateTiling(float snowballSize, float oldSize)
        {
            if (snowballSize <= 0) { return; }
            float sizeRatio = REFERENCE_SCALE / snowballSize;

            // Update the size of the sprite
            sRend.size = spriteDimensions / sizeRatio;

            // Update the scale of the object's transform.
            transform.localScale = Vector3.one * sizeRatio;
        }
    }
}
