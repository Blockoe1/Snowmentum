/*****************************************************************************
// File Name : SnowballProxy.cs
// Author : Brandon Koederitz
// Creation Date : 11/14/2025
// Last Modified : 11/14/2025
//
// Brief Description : Script that mimicks snowball growth so that we can get screenshots for an obstacle size survey.
*****************************************************************************/
using Snowmentum.Size;
using UnityEngine;

namespace Snowmentum
{
    public class SnowballProxy : MonoBehaviour
    {
        [SerializeField] private int bracket;
        [SerializeField] private float size;
        [Tooltip("Internal Data")]
        [SerializeField] private Vector3 referenceScale = new Vector3(0.2f, 0.2f, 0.2f);
        [SerializeField] private UpdateSprite[] updateSprites;


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
            // Sets the snowball scale based on it's size and our environment size.
            transform.localScale = referenceScale * (size / SizeBracket.GetMinSize(bracket));
            // Update the sprite based on the UpdateSprites array.
        }
    }
}
