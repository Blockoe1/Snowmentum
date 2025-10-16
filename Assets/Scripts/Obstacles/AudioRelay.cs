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

        public static Action<string> RelayPlayEvent;
        public static Action<string> RelayStopEvent;
        public static Action RelayStopAllEvent;

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
            Play(soundName);
        }
        public void Play(string soundName)
        {
            RelayPlayEvent?.Invoke(soundName);
        }

        /// <summary>
        /// Stops a given sound on the audioManager by event.
        /// </summary>
        public void Stop()
        {
            Stop(soundName);
        }
        public void Stop(string soundName)
        {
            RelayStopEvent?.Invoke(soundName);
        }

        /// <summary>
        /// Stops all sounds on the audioManager.
        /// </summary>
        public void StopAll()
        {
            RelayStopAllEvent?.Invoke();
        }
    }
}
