/*****************************************************************************
// File Name : SnowballProxy.cs
// Author : Brandon Koederitz
// Creation Date : 11/14/2025
// Last Modified : 11/14/2025
//
// Brief Description : Script that mimicks snowball growth so that we can get screenshots for an obstacle size survey.
*****************************************************************************/
using Snowmentum.Size;
using System.Linq;
using UnityEngine;

namespace Snowmentum
{
    public class SnowballProxy : MonoBehaviour
    {
        [SerializeField] private int bracket;
        [SerializeField] private float size;
        [Header("Internal Data")]
        [SerializeField] private Vector3 referenceScale = new Vector3(0.2f, 0.2f, 0.2f);
        [SerializeField] private UpdateSprite[] updateSprites;

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

        #region Nested
        [System.Serializable]
        private struct UpdateSprite
        {
            [SerializeField] internal Sprite sprite;
            [SerializeField] internal float transitionSize;
        }
        #endregion

        private void OnValidate()
        {
            // Sets the snowball scale based on it's size and our environment size
            float sVal = size / SizeBracket.GetMinSize(bracket);
            transform.localScale = referenceScale * sVal;
            // Update the sprite based on the UpdateSprites array.
            updateSprites = updateSprites.OrderBy(item => item.transitionSize).Reverse().ToArray();
            foreach (var item in updateSprites)
            {
                if (sVal > item.transitionSize)
                {
                    rend.sprite = item.sprite;
                    break;
                }
            }
        }
    }
}
