/*****************************************************************************
// File Name : ObstacleAnimator.cs
// Author : Brandon Koederitz
// Creation Date : 12/1/2025
// Last Modified : 12/1/2025
//
// Brief Description : Crude animator for obstacles.  Not using unity animator because it's too bulky to
// put on every obstacle.
*****************************************************************************/
using System.Collections;
using UnityEngine;

namespace Snowmentum
{
    public class ObstacleAnimator : MonoBehaviour
    {
        [SerializeField] private Sprite[] spriteFrames;
        [SerializeField] private int fps;

        private Coroutine animRoutine;

        #region Component References    
        [SerializeReference, ReadOnly] protected SpriteRenderer rend;

        /// <summary>
        /// Get components on reset.
        /// </summary>
        [ContextMenu("Get Component References")]
        private void Reset()
        {
            rend = GetComponent<SpriteRenderer>();
        }
        #endregion

        #region Properties
        public int FPS
        {
            get { return fps; }
            set { fps = value; }
        }
        public Sprite[] AnimationFrames
        { 
            get { return spriteFrames; }
            set { spriteFrames = value; }
        }
        #endregion

        /// <summary>
        /// Stops all animation when the object is disabled.
        /// </summary>
        private void OnDisable()
        {
            Stop();
        }

        /// <summary>
        /// Start/Stop the animation.
        /// </summary>
        public void Play()
        {
            Debug.Log("Play Called");
            // Prevent playing the animation if there are no specified sprite frames.
            if (spriteFrames == null || spriteFrames.Length == 0) { return; }
            if (animRoutine == null)
            {
                Debug.Log("Started Animation");
                animRoutine = StartCoroutine(AnimationRoutine());
            }
        }
        public void Stop()
        {
            if (animRoutine != null)
            {
                StopCoroutine(animRoutine);
                animRoutine = null;
            }
        }

        /// <summary>
        /// Controls swapping the obstacle's sprite to animate it.
        /// </summary>
        /// <returns></returns>
        private IEnumerator AnimationRoutine()
        {
            int frameIndex = 0;
            while (true)
            {
                rend.sprite = spriteFrames[frameIndex];
                yield return new WaitForSeconds(1 / (float)fps);
                // Increment the current frame.
                frameIndex++;
                CollectionHelpers.LoopIndex(spriteFrames.Length, ref frameIndex);
            }
        }
    }
}
