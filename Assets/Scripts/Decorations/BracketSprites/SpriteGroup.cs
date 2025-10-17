/*****************************************************************************
// File Name : SpriteGroiup.cs
// Author : Brandon Koederitz
// Creation Date : 10/16/2025
// Last Modified : 10/16/2025
//
// Brief Description : A group of multiple sprites.
*****************************************************************************/
using UnityEngine;

namespace Snowmentum
{
    [CreateAssetMenu(fileName = "SpriteGroup", menuName = "ScriptableObjects/Sprite Group")]
    public class SpriteGroup : ScriptableObject
    {
        [SerializeField] private Sprite[] sprites;

        /// <summary>
        /// Gets a random sprite from the sprite group.
        /// </summary>
        public Sprite GetRandom()
        {
            return sprites[Random.Range(0, sprites.Length)];
        }
    }
}
