/*****************************************************************************
// File Name : MusicRelay.cs
// Author :
// Creation Date : 
// Last Modified : 
//
// Brief Description : 
*****************************************************************************/
using System;
using UnityEngine;

namespace Snowmentum
{
    public class MusicRelay : MonoBehaviour
    {
        [SerializeField] private string defaultTrackName;

        public static Action<string, bool> RelaySetTrackEvent;
        public static Action<string> RelayPlayOverrideTrackEvent;
        public static Action RelayStopOverrideTrackEvent;

        /// <summary>
        /// Transitions the music to a given track.
        /// </summary>
        public void SetTrack()
        {
            SetTrack(defaultTrackName);
        }
        public void SetTrack(string trackName)
        {
            RelaySetTrackEvent?.Invoke(trackName, true);
        }
        public void SetTrackImmediate(string trackName)
        {
            RelaySetTrackEvent?.Invoke(trackName, false);
        }

        /// <summary>
        /// Sets an override track to play in place of the main track.
        /// </summary>
        public void PlayOverrideTrack()
        {
            PlayOverrideTrack(defaultTrackName);
        }
        public void PlayOverrideTrack(string trackName)
        {
            RelayPlayOverrideTrackEvent?.Invoke(trackName);
        }

        /// <summary>
        /// Stops the current override track and return to the main track.
        /// </summary>
        public void StopOverrideTrack()
        {
            RelayStopOverrideTrackEvent?.Invoke();
        }
    }
}
