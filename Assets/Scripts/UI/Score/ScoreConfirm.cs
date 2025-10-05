/*****************************************************************************
// File Name : ScoreConfirm.cs
// Author : Brandon Koederitz
// Creation Date : 10/4/2025
// Last Modified : 10/4/2025
//
// Brief Description : Allows the player to confirm their choice of initials for their high score.
*****************************************************************************/
using Snowmentum.UI;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Snowmentum
{
    public class ScoreConfirm : InitialsInputComponent
    {
        [SerializeField] private Image fillImage;
        [SerializeField] private float confirmTime;

        private InitialsInputMenu parentMenu;
        private Coroutine selectedCoroutine;

        /// <summary>
        /// Get the menu that this object is being used by.
        /// </summary>
        private void Awake()
        {
            parentMenu = GetComponentInParent<InitialsInputMenu>();
        }

        /// <summary>
        /// When this component is selected, wait a certain amount of time, then send a submit message to the input
        /// menu.
        /// </summary>
        public override void OnSelect()
        {
            if (selectedCoroutine != null)
            {
                StopCoroutine(selectedCoroutine);
                selectedCoroutine = null;
            }
            selectedCoroutine = StartCoroutine(SubmitRoutine(confirmTime));
        }

        /// <summary>
        /// Cancels the submit routine.
        /// </summary>
        public override void OnDeselect()
        {
            if (selectedCoroutine != null)
            {
                StopCoroutine(selectedCoroutine);
                selectedCoroutine = null;
            }

            fillImage.fillAmount = 0;
        }

        /// <summary>
        /// Wait for confirmTime seconds, updating our fill image, then when we reach the end send a confirm message 
        /// to the parent menu.
        /// </summary>
        /// <returns></returns>
        private IEnumerator SubmitRoutine(float submitTime)
        {
            float timer = submitTime;
            while (timer > 0)
            {
                fillImage.fillAmount = 1 - (timer / submitTime);

                timer -= Time.deltaTime;
                yield return null;
            }

            // When we reach the end of our coroutine, send the submit message.
            parentMenu.SaveHighScore();
            selectedCoroutine = null;
        }
    }
}
