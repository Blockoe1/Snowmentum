/*****************************************************************************
// File Name : CursorController.cs
// Author : Brandon Koederitz
// Creation Date : 10/17/2025
// Last Modified : 10/17/2025
//
// Brief Description : Enables the mouse cursor in this scene.
*****************************************************************************/
using UnityEngine;

namespace Snowmentum
{
    public class CursorManager : MonoBehaviour
    {
        /// <summary>
        /// Unlock the cursor when this script is loaded.
        /// </summary>
        private void Awake()
        {
            // Unlock the cursor on snowball death.
            Cursor.lockState = CursorLockMode.None;
        }

        /// <summary>
        /// Lock the cursor when the script is destroyed.
        /// </summary>
        private void OnDestroy()
        {
            // Unlock the cursor on snowball death.
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
