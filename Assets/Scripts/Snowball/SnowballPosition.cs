/*****************************************************************************
// File Name : SnowballPosition.cs
// Author : Brandon Koederitz
// Creation Date : 10/7/2025
// Last Modified : 10/7/2025
//
// Brief Description : Continually tracks the snowball's position so that obstacles know where it is for outlines.
*****************************************************************************/
using UnityEngine;
using System.Collections;
using System;

namespace Snowmentum
{
    public class SnowballPosition : MonoBehaviour
    {
        public static event Action<Vector2> OnPositionChanged;
        private Vector2 lastPosition;

        /// <summary>
        /// Start the coroutine.
        /// </summary>
        private void Awake()
        {
            StartCoroutine(UpdatePosition());
        }

        /// <summary>
        /// Continually updates the position of the snowball.
        /// </summary>
        /// <returns></returns>
        private IEnumerator UpdatePosition()
        {
            while (true)
            {
                OnPositionChanged?.Invoke(transform.position);
                lastPosition = transform.position;
                // Should run on update because this is purely visual.
                yield return null;
            }
        }
    }
}
