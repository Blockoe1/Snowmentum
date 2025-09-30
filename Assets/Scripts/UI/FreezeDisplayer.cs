/*****************************************************************************
// File Name : FreezeDisplayer.cs
// Author : Brandon Koederitz
// Creation Date : 9/23/2025
// Last Modified : 9/23/2025
//
// Brief Description : Displays the current freezeAmount of the snowball by moving a UI sprite.
*****************************************************************************/
using UnityEngine;

namespace Snowmentum
{
    public class FreezeDisplayer : MonoBehaviour
    {
        [SerializeField] private float emptyYPos;
        [SerializeField] private float targetYPos;

        #region Properties
        private RectTransform rectTransform => transform as RectTransform;
        #endregion

        private void Awake()
        {
            SnowballFreezing.OnFreezeAmountChanged += UpdateFreezeDisplay;
            UpdateFreezeDisplay(0);
        }
        private void OnDestroy()
        {
            SnowballFreezing.OnFreezeAmountChanged -= UpdateFreezeDisplay;
        }

        /// <summary>
        /// Updates the displayed freeze amount of the snowball.
        /// </summary>
        /// <param name="freezeAmount"></param>
        private void UpdateFreezeDisplay(float freezeAmount)
        {
            Vector2 anchorPos = rectTransform.anchoredPosition;
            float y = Mathf.Lerp(emptyYPos, targetYPos, freezeAmount);
            anchorPos.y = y;
            rectTransform.anchoredPosition = anchorPos;
        }
    }
}
