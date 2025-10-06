/*****************************************************************************
// File Name : SnowballLose.cs
// Author : Brandon Koederitz
// Creation Date : 9/22/2025
// Last Modified : 9/22/2025
//
// Brief Description : Detects when the player loses all of their size and ends the game.
*****************************************************************************/
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using Snowmentum.Size;

namespace Snowmentum
{
    public class SnowballLose : MonoBehaviour
    {
        [SerializeField] private UnityEvent OnDeathEvent;


        #region Component References
        [Header("Components")]
        [SerializeReference] protected SnowballSpeed speed;

        /// <summary>
        /// Get components on reset.
        /// </summary>
        [ContextMenu("Get Component References")]
        private void Reset()
        {
            speed = GetComponent<SnowballSpeed>();
        }
        #endregion


        private void Awake()
        {
            // Subscribe to size's OnChange function to detect size being set to 0;
            SnowballSize.OnTargetValueChanged += OnValueChanged;
        }
        private void OnDestroy()
        {
            SnowballSize.OnTargetValueChanged -= OnValueChanged;
            Cursor.lockState = CursorLockMode.None;
        }

        /// <summary>
        /// Checks for the snowball's size hitting 0.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="oldValue"></param>
        private void OnValueChanged(float value, float oldValue)
        {
            if (value <= 0)
            {
                // Reset speed to 0 so that the screen stops moving 
                speed.TargetValue_Local = 0;
                speed.Value_Local = 0;
                //Debug.Log(SnowballSpeed.Value);
                OnDeathEvent?.Invoke();
                Destroy(gameObject);
            }
        }
    }
}
