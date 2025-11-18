/*****************************************************************************
// File Name : SnowballQualitySprite.cs
// Author : Brandon Koederitz
// Creation Date : 11/17/2025
// Last Modified : 11/17/2025
//
// Brief Description : Switches this object's sprite based on a set of sprites and a given value.
*****************************************************************************/
using System.Linq;
using UnityEngine;

namespace Snowmentum
{
    public class TieredSprite : MonoBehaviour
    {
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

        /// <summary>
        /// Sets this object's sprite based on a given value.
        /// </summary>
        /// <param name="val"></param>
        public void SetSprite(float val)
        {
            // Update the sprite based on the UpdateSprites array.
            updateSprites = updateSprites.OrderBy(item => item.transitionSize).Reverse().ToArray();
            foreach (var item in updateSprites)
            {
                if (val > item.transitionSize)
                {
                    rend.sprite = item.sprite;
                    break;
                }
            }
        }
    }
}
