/*****************************************************************************
// File Name : ThrowingStarter.cs
// Author : Brandon Koederitz
// Creation Date : 11/17/2025
// Last Modified : 11/17/2025
//
// Brief Description : Glue code to let the packing minigame start the throwing minigame from an animation event.
*****************************************************************************/
using UnityEngine;

namespace Snowmentum
{
    public class MinigameDisposer : MonoBehaviour
    {
        [SerializeField] private ThrowingMinigame throwMinigame;

        public void StartThrow()
        {
            if (throwMinigame != null)
            {
                throwMinigame.StartThrow();
            }
            DestroyMinigame();
        }

        /// <summary>
        /// Destroys this minigame.
        /// </summary>
        public void DestroyMinigame()
        {
            Destroy(gameObject);
        }
    }
}
