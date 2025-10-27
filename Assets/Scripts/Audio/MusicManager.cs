/*****************************************************************************
// File Name : MusicManager.cs
// Author :
// Creation Date : 
// Last Modified : 
//
// Brief Description : 
*****************************************************************************/
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Timeline;

namespace Snowmentum
{
    public class MusicManager : MonoBehaviour
    {
        private static MusicManager instance;

        #region Component References
        [Header("Components")]
        [SerializeReference] protected AudioSource musicSource;

        /// <summary>
        /// Get components on reset.
        /// </summary>
        [ContextMenu("Get Component References")]
        private void Reset()
        {
            musicSource = GetComponent<AudioSource>();
        }
        #endregion

        #region Nested
        [System.Serializable]
        private class MusicTrack
        {
            [SerializeField] internal string name;
            [SerializeField] internal AudioTrack track;
            [SerializeField, Range(0f, 1f)] internal float volume = 1f;
            [SerializeField, Range(-3f, 3f)] internal float pitch = 1f;

            internal AudioSource source;
        }
        #endregion

        /// <summary>
        /// Setup the singleton instance.
        /// </summary>
        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }
            else
            {
                instance = this;
            }

            MusicRelay.RelaySetTrackEvent = SetTrack;
            MusicRelay.RelayPlayOverrideTrackEvent = StartOverrideTrack;
            MusicRelay.RelayStopOverrideTrackEvent = StopOverrideTrack;
        }
        private void OnDestroy()
        {
            if (instance == this)
            {
                MusicRelay.RelaySetTrackEvent = null;
                MusicRelay.RelayPlayOverrideTrackEvent = null;
                MusicRelay.RelayStopOverrideTrackEvent = null;
            }
        }

        /// <summary>
        /// Sets the current main track and transitions to it.
        /// </summary>
        /// <param name="trackName"></param>
        private void SetTrack(string trackName)
        {

        }

        /// <summary>
        /// Plays an override track in place of the main track.
        /// </summary>
        /// <param name="trackName"></param>
        private void StartOverrideTrack(string trackName)
        {

        }

        /// <summary>
        /// Stops the current override track.
        /// </summary>
        private void StopOverrideTrack()
        {

        }
    }
}
