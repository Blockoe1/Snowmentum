/*****************************************************************************
// File Name : AudioRelay.cs
// Author : Brandon Koederitz
// Creation Date : 10/6/2025
// Last Modified : 10/6/2025
//
// Brief Description : Allows obstacles to indirectly play sounds from the audio manager.
*****************************************************************************/
using System;
using UnityEngine;

namespace Snowmentum
{
    public class AudioRelay : MonoBehaviour
    {
        [SerializeField] private string soundName;

        public static event Action<string> RelayPlayEvent;

        #region Properties
        public string SoundName
        {
            get { return soundName; }
            set { soundName = value; }
        }
        #endregion

        /// <summary>
        /// Plays a sound on the audio manager via an event.
        /// </summary>
        public void Play()
        {
            RelayPlayEvent?.Invoke(soundName);
        }
    }
}
