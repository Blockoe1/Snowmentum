/*****************************************************************************
// File Name : ScorePopupManager.cs
// Author : Brandon Koederitz
// Creation Date : 12/4/2025
// Last Modified : 12/4/2025
//
// Brief Description : Spawns score popups on the UI to display the score the player gained.
*****************************************************************************/
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Snowmentum.Score
{
    public class ScorePopupManager : MonoBehaviour
    {
        [SerializeField] private ScorePopupAnim scorePopupPrefab;
        // Event to call the display score function on the actual MonoBehaviour.  Not an event as only one should
        // exist at a time.
        private static ScorePopupManager instance;

        private readonly Queue<ScorePopupAnim> inactivePopupPool = new();

        /// <summary>
        /// Setup the private singleton.
        /// </summary>
        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                Debug.Log("Duplicate ScorePopupManager found.");
                return;
            }
            else
            {
                instance = this;
            }
        }
        private void OnDestroy()
        {
            if (instance == this)
            {
                instance = null;
            }
        }

        /// <summary>
        /// Static public interface function to allow other scripts to display popups.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="worldPos"></param>
        public static void DisplayScore(string text, Vector2 worldPos)
        {
            if (instance != null)
            {
                instance.DisplayScore_Internal(text, worldPos);
            }
        }
        /// <summary>
        /// Displays a score popup at a given position.
        /// </summary>
        /// <param name="text">The text to display on the popup.</param>
        /// <param name="worldPos"></param>
        private void DisplayScore_Internal(string text, Vector2 worldPos)
        {
            ScorePopupAnim popup = GetPopup();
            popup.Play(text, worldPos, ReturnPopup);
        }

        #region Object Pooling
        /// <summary>
        /// Get an unused popup from the pool of unused popups.
        /// </summary>
        /// <returns></returns>
        private ScorePopupAnim GetPopup()
        {
            ScorePopupAnim toGet = inactivePopupPool.Count > 0 ? inactivePopupPool.Dequeue() : 
                Instantiate(scorePopupPrefab, transform);
            toGet.gameObject.SetActive(true);
            return toGet;
        }
        /// <summary>
        /// Return a popup to the pool.
        /// </summary>
        /// <param name="toReturn"></param>
        private void ReturnPopup(ScorePopupAnim toReturn)
        {
            toReturn.gameObject.SetActive(false);
            inactivePopupPool.Enqueue(toReturn);
        }
        #endregion
    }
}
