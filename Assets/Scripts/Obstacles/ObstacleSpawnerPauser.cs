/*****************************************************************************
// File Name : ObstacleSpawnerPauser.cs
// Author :
// Creation Date : 
// Last Modified : 
//
// Brief Description : 
*****************************************************************************/
using Snowmentum.Size;
using UnityEngine;

namespace Snowmentum
{
    public class ObstacleSpawnerPauser : MonoBehaviour
    {
        #region Component References
        [Header("Components")]
        [SerializeReference] protected ObstacleSpawner spawner;

        /// <summary>
        /// Get components on reset.
        /// </summary>
        [ContextMenu("Get Component References")]
        private void Reset()
        {
            spawner = GetComponent<ObstacleSpawner>();
        }
        #endregion

        /// <summary>
        /// Subscribe.Unsubscribe events.
        /// </summary>
        private void Awake()
        {
            SizeBracket.OnBracketChanged += SizeBracket_OnBracketChanged;
            EnvironmentSize.OnValueTransitionFinish += EnvironmentSize_OnValueTransitionFinish;
        }

        private void OnDestroy()
        {
            SizeBracket.OnBracketChanged -= SizeBracket_OnBracketChanged;
            EnvironmentSize.OnValueTransitionFinish -= EnvironmentSize_OnValueTransitionFinish;
        }

        /// <summary>
        /// Unpause the spawner when the transition finishes.
        /// </summary>
        private void EnvironmentSize_OnValueTransitionFinish()
        {
            spawner.IsPaused = false;
        }

        /// <summary>
        /// Pause the spawner when the player reaches a new size bracekt.
        /// </summary>
        /// <param name="newBracket"></param>
        /// <param name="oldBracket"></param>
        private void SizeBracket_OnBracketChanged(int newBracket, int oldBracket)
        {
            // Only pause if we aren't initializing the bracket.
            if (oldBracket > 0 && oldBracket != newBracket)
            {
                spawner.IsPaused = true;
            }
        }
    }
}
