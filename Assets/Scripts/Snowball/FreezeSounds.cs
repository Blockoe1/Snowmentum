/*****************************************************************************
// File Name : FreezeSounds.cs
// Author : Brandon Koederitz
// Creation Date : 11/13/2025
// Last Modified : 11/13/2025
//
// Brief Description : Interprets events from the snowba'lls freeze controller and plays sounds when the snowball blinks.
*****************************************************************************/
using UnityEngine;
using UnityEngine.Audio;

namespace Snowmentum
{
    public class FreezeSounds : MonoBehaviour
    {
        [SerializeField] private string toNormalSound;
        [SerializeField] private string toFrozenSound;

        #region Component References
        [Header("Components")]
        [SerializeReference] protected AudioRelay audioRelay;

        /// <summary>
        /// Get components on reset.
        /// </summary>
        [ContextMenu("Get Component References")]
        private void Reset()
        {
            audioRelay = GetComponent<AudioRelay>();
        }
        #endregion

        /// <summary>
        /// Plays a different sound effect when the snowball blinks based on if it blinked to ice or normal.
        /// </summary>
        /// <param name="isFrozen"></param>
        public void PlaySnowballBlinkSound(bool isFrozen)
        {
            audioRelay.Play(isFrozen ? toFrozenSound : toNormalSound);
        }
    }
}
