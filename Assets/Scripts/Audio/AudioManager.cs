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

        #region Nested
        [System.Serializable]
        private class Sound
        {
            [SerializeField] internal string name;
            [SerializeField] internal AudioClip audioClip;
            [SerializeField] internal AudioMixerGroup mixerGroup;
            [SerializeField, Range(0f, 1f)] internal float volume = 1f;
            [SerializeField, Range(-3f, 3f)] internal float pitch = 1f;
            [SerializeField] internal bool loop;

            internal AudioSource source;
        }
        #endregion

        private void Awake()
        {
            SetupSounds();
        }

        /// <summary>
        /// Sets up all of the sounds on this audio manager with an audio source.
        /// </summary>
        private void SetupSounds()
        {
            foreach (var sound in sounds)
            {
                sound.source = gameObject.AddComponent<AudioSource>();

                sound.source.clip = sound.audioClip;
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
            Sound toPlay = Array.Find(sounds, item => item.name == soundName);
            if (toPlay != null)
            {
                toPlay.source.Play();
            }
        }

        /// <summary>
        /// Stops a given sound.
        /// </summary>
        /// <param name="soundName">The sound to stop.</param>
        public void Stop(string soundName)
        {
            Sound toPlay = Array.Find(sounds, item => item.name == soundName);
            if (toPlay != null)
            {
                toPlay.source.Stop();
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
