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

        private static AudioManager instance;

        #region Nested
        [System.Serializable]
        private abstract class SoundBase
        {
            [SerializeField] internal string name;
            [SerializeField] internal AudioMixerGroup mixerGroup;
            [SerializeField, Range(0f, 1f)] internal float volume = 1f;
            [SerializeField, Range(-3f, 3f)] internal float pitch = 1f;
            [SerializeField] internal bool loop;

            internal AudioSource source;

            internal abstract void Play();

            internal void Stop()
            {
                source.Stop();
            }
        }

        // Plays 1 singular sound.
        [System.Serializable]
        private class Sound :SoundBase
        {
            [Header("Clip")]
            [SerializeField] internal AudioClip audioClip;

            /// <summary>
            /// Plays the set audio clip of this sound.
            /// </summary>
            internal override void Play()
            {
                source.clip = audioClip;
                source.Play();
            }
        }

        // Plays a random sound from an array of sounds.
        [System.Serializable]
        private class RandomizedSound : SoundBase
        {
            [Header("Clip")]
            [SerializeField] internal AudioClip[] audioClips;

            /// <summary>
            /// Gets a random audio clip from the array and plays it.
            /// </summary>
            internal override void Play()
            {
                AudioClip clip = audioClips[UnityEngine.Random.Range(0, audioClips.Length - 1)];
                source.clip = clip;
                source.Play();
            }
        }
        #endregion

        private void Awake()
        {

            // If an AudioManager already exists, then we should destroy this AudioManager and prevent any setup.
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }
            else
            {
                // Make the AudioManager DontDestroyOnLoad so that sounds can persist across scenes.
                DontDestroyOnLoad(gameObject);
                instance = this;
            }

            SetupSounds(sounds);
            SetupSounds(randomizedSounds);

            AudioRelay.RelayPlayEvent = Play;
            AudioRelay.RelayPlayRandomizedEvent = PlayRandom;
            AudioRelay.RelayStopEvent = Stop;
            AudioRelay.RelayStopAllEvent = StopAll;
                
        }

        /// <summary>
        /// When the Audio Manager is destroyed, reset all relay actions.
        /// </summary>
        private void OnDestroy()
        {
            if (instance == this)
            {
                instance = null;
                AudioRelay.RelayPlayEvent = null;
                AudioRelay.RelayPlayRandomizedEvent = null;
                AudioRelay.RelayStopEvent = null;
                AudioRelay.RelayStopAllEvent = null;
            }
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
        /// Plays a given randomized sound.
        /// </summary>
        /// <param name="soundName">The sound to play.</param>
        public void PlayRandom(string soundName)
        {
            SoundBase toPlay = Array.Find(randomizedSounds, item => item.name == soundName);
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
            // Check both normal and randomized sounds.
            SoundBase toStop = Array.Find(sounds, item => item.name == soundName);
            if (toStop != null)
            {
                toStop.Stop();
                return;
            }

            toStop = Array.Find(randomizedSounds, item => item.name == soundName);
            if (toStop != null)
            {
                toStop.Stop();
                return;
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
