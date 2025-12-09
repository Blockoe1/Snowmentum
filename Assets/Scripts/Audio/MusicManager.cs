/*****************************************************************************
// File Name : MusicManager.cs
// Author :
// Creation Date : 
// Last Modified : 
//
// Brief Description : 
*****************************************************************************/
using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Timeline;

namespace Snowmentum
{
    public class MusicManager : MonoBehaviour
    {
        [SerializeField] private float transitionTime;
        [SerializeField] private float overrideFadeTime; 
        [SerializeField, Range(0f, 1f)] private float overwrittenMultiplier;
        [SerializeField] private MusicTrack[] musicTracks;

        private MusicTrack currentTrack;
        private MusicTrack overrideTrack;

        private static MusicManager instance;

        #region Nested
        private delegate void TrackFadeCallback(MusicTrack track);
        [System.Serializable]
        private class MusicTrack : SoundBase
        {
            #region CONSTS
            private const double LOOP_PRELOAD_TIME = 1;
            #endregion

            [Header("Tracks")]
            [SerializeField] private AudioClip loopTrack;
            [SerializeField] private AudioClip introTrack;
            [SerializeField] private int msIntroBuffer;

            internal float Volume => volume;
            // Gets the volume of the audio sources.  Should be the same for the normal and loop sources.
            internal float SourceVolume => source.volume;
            //internal AudioSource Source => isLoop ? loopSource : source;

            // Need to make a second audioSource for the loop track.
            private AudioSource introSource;

            /// <summary>
            /// When this track is set up, add an additional audio source that we set as our loop source.
            /// </summary>
            /// <param name="source"></param>
            protected override void Setup(AudioSource source)
            {
                base.Setup(source);
                source.clip = loopTrack;

                if (introTrack != null)
                {
                    introSource = source.AddComponent<AudioSource>();
                    base.Setup(introSource);
                    introSource.clip = introTrack;
                    introSource.loop = false;
                }
            }

            /// <summary>
            /// Sets the volume of this track's audio sources.
            /// </summary>
            /// <param name="volume"></param>
            internal void SetSourceVolume(float volume)
            {
                source.volume = volume;
                if (introSource != null)
                {
                    introSource.volume = volume;
                }
            }

            /// <summary>
            /// Start playing with the intro track, then transition to the loop track once the intro finishes.
            /// </summary>
            public override void Play()
            {
                if (introSource != null)
                {
                    introSource.Play();
                    // Use doubles for more precision.
                    // Calculate the precise time to start the looping track.
                    double introDuraction = introTrack.samples / (double)introTrack.frequency;
                    double playTime = AudioSettings.dspTime + introDuraction;

                    Debug.Log(introDuraction);
                    //instance.StartCoroutine(ToLoopRoutine(loopSource, playTime));
                    source.PlayScheduled(playTime + (msIntroBuffer / 1000.0));
                }
                else
                {
                    // Directly play the loop source if we aren't using the intro.
                    source.Play();
                }
            }

            /// <summary>
            /// Stop both sources.
            /// </summary>
            public override void Stop()
            {
                base.Stop();
                if (introSource != null)
                {
                    introSource.Stop();
                }
            }

            /// <summary>
            /// Coroutine run on the audio source that swaps the track to the loop track once the intro track finishes.
            /// </summary>
            /// <returns></returns>
            //private IEnumerator ToLoopRoutine(AudioSource source, double playTime)
            //{
            //    // Wait until we've reached the right time to queue up our loop clip.
            //    while (AudioSettings.dspTime > playTime - LOOP_PRELOAD_TIME)
            //    {
            //        yield return null;
            //    }
            //    source.PlayScheduled(playTime);
            //}
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
                DontDestroyOnLoad(gameObject);
                instance = this;
            }

            MusicRelay.RelaySetTrackEvent = SetTrack;
            MusicRelay.RelayPlayOverrideTrackEvent = StartOverrideTrack;
            MusicRelay.RelayStopOverrideTrackEvent = StopOverrideTrack;

            SoundBase.SetupSounds(musicTracks, gameObject);
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

        #region Base Track Transitions
        /// <summary>
        /// Sets the current main track and transitions to it.
        /// </summary>
        /// <param name="trackName"></param>
        private void SetTrack(string trackName, bool hasFade)
        {
            MusicTrack newTrack = Array.Find(musicTracks, itme => itme.Name == trackName);
            if (newTrack != null && newTrack != currentTrack)
            {

                if (currentTrack != null)
                {
                    // Transition to the new track.
                    if (hasFade)
                    {
                        StartCoroutine(FadeTrack(currentTrack, transitionTime, 0, ResetTrack));
                    }
                    else
                    {
                        ResetTrack(currentTrack);
                    }
                }

                newTrack.Play();
                // Reset the track's source volume so it fades in.
                if (hasFade)
                {
                    newTrack.SetSourceVolume(0);
                    // Fade the track to it's new volume.
                    StartCoroutine(FadeTrack(newTrack, transitionTime, newTrack.Volume));
                }

                    currentTrack = newTrack;
            }
        }

        /// <summary>
        /// Stops a music track and resets it's volume back to the default.
        /// </summary>
        /// <param name="track"></param>
        private void ResetTrack(MusicTrack track)
        {
            track.Stop();
            track.SetSourceVolume(track.Volume);
            //track.Source.volume = track.Volume;
        }

        /// <summary>
        /// Fades a given music track in or out.
        /// </summary>
        /// <param name="track"></param>
        /// <param name="duration"></param>
        /// <param name="targetVolume"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        private IEnumerator FadeTrack(MusicTrack track, float duration, float targetVolume, TrackFadeCallback callback = null)
        {
            float timer = duration;
            float startingVolume = track.SourceVolume;
            float normalizedProgress;
            while (timer > 0)
            {
                normalizedProgress = 1 - (timer / duration);
                track.SetSourceVolume(Mathf.Lerp(startingVolume, targetVolume, normalizedProgress));

                // Should not scale with timeScale.
                timer -= Time.unscaledDeltaTime;
                yield return null;
            }
            // Call a callback so we can run some code when the fade finishes.
            callback?.Invoke(track);
        }
        #endregion

        /// <summary>
        /// Plays an override track in place of the main track.
        /// </summary>
        /// <param name="trackName"></param>
        private void StartOverrideTrack(string trackName)
        {
            MusicTrack overrideTrack = Array.Find(musicTracks, item => item.Name == trackName);
            if (overrideTrack != null)
            {
                // Stop any current override tracks.
                StopOverrideTrack();

                // Disable the current track by setting volume to 0.
                //currentTrack.SetSourceVolume(currentTrack.SourceVolume * overwrittenMultiplier);
                StartCoroutine(FadeTrack(currentTrack, overrideFadeTime, currentTrack.SourceVolume * overwrittenMultiplier));

                // Play the override track.
                this.overrideTrack = overrideTrack;
                overrideTrack.Play();
                overrideTrack.SetSourceVolume(0);
                StartCoroutine(FadeTrack(overrideTrack, overrideFadeTime, overrideTrack.Volume));
            }
        }

        /// <summary>
        /// Stops the current override track.
        /// </summary>
        private void StopOverrideTrack()
        {
            if (overrideTrack != null)
            {
                float currentVolume = overwrittenMultiplier == 0 ? currentTrack.Volume :
                        currentTrack.SourceVolume / overwrittenMultiplier;
                StartCoroutine(FadeTrack(currentTrack, overrideFadeTime, currentVolume));

                // Stop the override track.
                //overrideTrack.Stop();
                StartCoroutine(FadeTrack(overrideTrack, overrideFadeTime, 0, ResetTrack));
                overrideTrack = null;
            }
        }
    }
}
