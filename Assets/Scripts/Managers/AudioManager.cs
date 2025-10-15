/*****************************************************************************
// File Name : AudioManager.cs
// Author : Brandon Koederitz
// Creation Date : 10/3/2025
// Last Modified : 10/3/2025
//
// Brief Description : Allows for the mixing an playing of audio
*****************************************************************************/
using System;
using UnityEngine;
using UnityEngine.Audio;

namespace Snowmentum
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private Sound[] sounds;
        [SerializeField] private RandomizedSound[] randomizedSounds;

        #region Nested
        [System.Serializable]
        private abstract class SoundBase
        {
            [SerializeField] internal string name;
            [SerializeField] internal AudioMixerGroup mixerGroup;
            [SerializeField, Range(0f, 1f)] internal float volume = 1f;
            [SerializeField, Range(-3f, 3f)] internal float pitch = 1f;
            [SerializeField] internal bool loop;
            [SerializeField] internal bool randomizedClip;

            internal AudioSource source;

            internal abstract void Play();

            internal void Stop()
            {
                source.Stop();
            }
        }

        // Plays 1 singular sound.
        private class Sound :SoundBase
        {
            [SerializeField] internal AudioClip audioClip;
        }

        // Plays a random sound from an array of sounds.
        private class RandomizedSound : SoundBase
        {
            [SerializeField] internal AudioClip[] audioClips;
        }
        #endregion

        private void Awake()
        {
            SetupSounds();

            AudioRelay.RelayPlayEvent += Play;
        }

        private void OnDestroy()
        {
            AudioRelay.RelayPlayEvent -= Play;
        }

        /// <summary>
        /// Sets up all of the sounds on this audio manager with an audio source.
        /// </summary>
        private void SetupSounds(SoundBase[] sounds)
        {
            foreach (var sound in sounds)
            {
                sound.source = gameObject.AddComponent<AudioSource>();

                sound.source.outputAudioMixerGroup = sound.mixerGroup;
                sound.source.volume = sound.volume;
                sound.source.pitch = sound.pitch;
                sound.source.loop = sound.loop;
                sound.source.playOnAwake = false;
            }
        }

        /// <summary>
        /// Plays a given sound.
        /// </summary>
        /// <param name="soundName">The sound to play.</param>
        public void Play(string soundName)
        {
            SoundBase toPlay = Array.Find(sounds, item => item.name == soundName);
            if (toPlay != null)
            {
                toPlay.Play();
            }
        }

        /// <summary>
        /// Stops a given sound.
        /// </summary>
        /// <param name="soundName">The sound to stop.</param>
        public void Stop(string soundName)
        {
            SoundBase toPlay = Array.Find(sounds, item => item.name == soundName);
            if (toPlay != null)
            {
                toPlay.Stop();
            }
        }

        /// <summary>
        /// Stops all playing sounds.
        /// </summary>
        public void StopAll()
        {
            foreach(var sound in sounds)
            {
                sound.source.Stop();
            }
        }
    }
}
