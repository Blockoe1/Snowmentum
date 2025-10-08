/*****************************************************************************
// File Name : ParallaxObject.cs
// Author : Zaden Westbrook
// Creation Date : 10/8/2025
// Last Modified : 10/8/2025
//
// Brief Description : Randomly selects from list of sprites, waits to be spawned into background as a parallax decoration
*****************************************************************************/
using System.Collections.Generic;
using UnityEngine;

namespace Snowmentum
{
    public class ParallaxObject : MonoBehaviour
    {
        public List<Sprite> sprites = new List<Sprite>();
        private int _selectedSprite = 0;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void OnEnable() //randomly select sprite from the list in the prefab 
        {
            _selectedSprite = Random.Range(0, sprites.Count);
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = sprites[_selectedSprite];
        }
    }
}
