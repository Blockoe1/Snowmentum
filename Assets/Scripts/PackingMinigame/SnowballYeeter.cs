/*****************************************************************************
// File Name : SnowballYeeter.cs
// Author : Brandon Koederitz
// Creation Date : 11/17/2025
// Last Modified : 11/17/2025
//
// Brief Description : Starts the snowball off with some initial speed when the throw minigame ends.
*****************************************************************************/
using UnityEngine;

namespace Snowmentum
{
    public class SnowballYeeter : MonoBehaviour
    {
        [SerializeField] private SnowballSpeed snowballSpeed;
        [SerializeField, Tooltip("Multiplied by the scaled throwStrength to get the starting speed of the snowball")]
        private float startingSpeedScaler;
        [SerializeField, Tooltip("The minimum starting speed that the snowball is thrown at.")]
        private float minStartingSpeed;

        /// <summary>
        /// Gives the snowball initial speed based on the quality of the player's throw.
        /// </summary>
        /// <param name="throwQuality"></param>
        public void YeetSnowball(float throwQuality)
        {
            if (snowballSpeed != null)
            {
                snowballSpeed.Value_Local = Mathf.Max(throwQuality * startingSpeedScaler, minStartingSpeed);
            }
        }
    }
}
