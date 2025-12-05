/*****************************************************************************
// File Name : SpriteRandomizer.cs
// Author : Brandon Koederitz
// Creation Date : 12/5/2025
// Last Modified : 12/5/2025
//
// Brief Description : Randomizes the sprite on this object's sprite renderer to be one from a list.
*****************************************************************************/
using UnityEngine;

namespace Snowmentum
{
    public class SpriteRandomizer : MonoBehaviour
    {
        [SerializeField] private SpriteRand[] sprites;

        #region Component References    
        [SerializeReference, ReadOnly] protected SpriteRenderer rend;

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
        private struct SpriteRand
        {
            [SerializeField] internal Sprite sprite;
            [SerializeField] internal int weight;
        }
        #endregion

        /// <summary>
        /// Randomizes this object's sprite.
        /// </summary>
        public void Randomize()
        {
            Sprite toSet = GetRandomSprite(sprites);
            rend.sprite = toSet;
        }

        /// <summary>
        /// Gets a randomized sprite from an array of SpriteRand structs.
        /// </summary>
        /// <param name="sprites"></param>
        /// <returns></returns>
        private static Sprite GetRandomSprite(SpriteRand[] sprites)
        {
            int totalWeight = 0;
            foreach (var rand in sprites)
            {
                totalWeight += rand.weight;
            }

            int randomWeight = Random.Range(0, totalWeight);

            for (int i = 0; i < sprites.Length; i++)
            {
                randomWeight -= sprites[i].weight;
                if (randomWeight <= 0)
                {
                    return sprites[i].sprite;
                }
            }
            return null;
        }
    }
}
