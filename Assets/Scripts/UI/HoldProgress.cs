/*****************************************************************************
// File Name : HoldProgress.cs
// Author :
// Creation Date : 
// Last Modified : 
//
// Brief Description : 
*****************************************************************************/
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Snowmentum
{
    public class HoldProgress : MonoBehaviour
    {
        #region CONSTS
        private const string MOUSE_POS_NAME = "MousePosition";
        #endregion

        [SerializeField] private Image fillImage;

        private InputAction mousePosAction;
        private static event Action<bool> setVisibleEvent;
        private bool isVisible;

        /// <summary>
        /// Setup the progress indicator on awake.
        /// </summary>
        private void Awake()
        {
            mousePosAction = InputSystem.actions.FindAction(MOUSE_POS_NAME);

            setVisibleEvent += SetVisible;
            SetVisible(false);
        }
        private void OnDestroy()
        {
            setVisibleEvent -= SetVisible;
        }

        /// <summary>
        /// Toggles if the mouse is currently hovering over a button.
        /// </summary>
        /// <param name="isVisible"></param>
        public static void SetProgressVisibility(bool isVisible)
        {
            setVisibleEvent?.Invoke(isVisible);
        }

        /// <summary>
        /// Toggles this progress bar being visible or not.
        /// </summary>
        /// <param name="isVisible"></param>
        private void SetVisible(bool isVisible)
        {
            gameObject.SetActive(isVisible);
            Cursor.visible = !isVisible;
            this.isVisible = isVisible;

            if (isVisible)
            {
                StartCoroutine(ToCursorRoutine());
            }
        }

        /// <summary>
        /// Continually mvoes this object to the mouse's position.
        /// </summary>
        /// <returns></returns>
        private IEnumerator ToCursorRoutine()
        {
            while (isVisible)
            {
                transform.position = mousePosAction.ReadValue<Vector2>();
                yield return null;
            }
        }
    }
}
