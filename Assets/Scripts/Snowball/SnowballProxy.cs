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
    public class SnowballProxy : TieredSprite
    {
        [SerializeField] private int bracket;
        [SerializeField] private float size;
        [Header("Internal Data")]
        [SerializeField] private Vector3 referenceScale = new Vector3(0.2f, 0.2f, 0.2f);

        private void OnValidate()
        {
            // Sets the snowball scale based on it's size and our environment size
            float sVal = size / SizeBracket.GetMinSize(bracket);
            transform.localScale = referenceScale * sVal;
            SetSprite(sVal);
        }


    }
}
