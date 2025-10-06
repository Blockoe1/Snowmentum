/*****************************************************************************
// File Name : SnowballAnimations.cs
// Author : Brandon Koederitz
// Creation Date : 10/6/2025
// Last Modified : 10/6/2025
//
// Brief Description : Controls animating the snowball based on the size of the snowball.
*****************************************************************************/
using Snowmentum.Size;
using UnityEngine;

namespace Snowmentum
{
    public class SnowballAnimations : MonoBehaviour
    {
        #region CONSTS
        private const string ANIM_SIZE_NAME = "Size";
        #endregion

        [SerializeField] private float baseAnimationSpeed = 1f;

        #region Component References
        [Header("Components")]
        [SerializeReference] protected Animator anim;

        /// <summary>
        /// Get components on reset.
        /// </summary>
        [ContextMenu("Get Component References")]
        private void Reset()
        {
            anim = GetComponent<Animator>();
        }
        #endregion

        private void Awake()
        {
            SnowballSize.OnValueChanged += SnowballSize_OnValueChanged;
            SnowballSpeed.OnValueChanged += SnowballSpeed_OnValueChanged;
            EnvironmentSize.OnValueChanged += EnvironmentSize_OnValueChanged;
        }

        private void OnDestroy()
        {
            SnowballSize.OnValueChanged -= SnowballSize_OnValueChanged;
            SnowballSpeed.OnValueChanged -= SnowballSpeed_OnValueChanged;
            EnvironmentSize.OnValueChanged -= EnvironmentSize_OnValueChanged;
        }

        /// <summary>
        /// When the snowball or environment size changes, update the animation used for the snowball.
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        private void EnvironmentSize_OnValueChanged(float arg1, float arg2)
        {
            UpdateAnimSize();
        }

        private void SnowballSize_OnValueChanged(float arg1, float arg2)
        {
            UpdateAnimSize();
        }

        /// <summary>
        /// Updates the animation's size parameter based on the size of the snowball and environment.
        /// </summary>
        private void UpdateAnimSize()
        {
            // Prevent /0 error.
            if (SnowballSize.Value == 0 || EnvironmentSize.Value == 0) { return; }
            // Sets the snowball scale based on it's size and our environment size.
            anim.SetFloat(ANIM_SIZE_NAME, SnowballSize.Value / EnvironmentSize.Value);
        }

        /// <summary>
        /// Updates the animator's speed so that the snowball animation speeds up as the snowball speeds up.
        /// </summary>
        /// <param name="snowballSpeed"></param>
        /// <param name="oldSpeed"></param>
        private void SnowballSpeed_OnValueChanged(float snowballSpeed, float oldSpeed)
        {
            anim.speed = baseAnimationSpeed * snowballSpeed;
        }
    }
}
