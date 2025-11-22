/*****************************************************************************
// File Name : AnimatorParamSetter.cs
// Author : Brandon Koederitz
// Creation Date : 11/22/2025
// Last Modified : 11/22/2025
//
// Brief Description : Updates an animation parameter.
*****************************************************************************/
using UnityEngine;

namespace Snowmentum
{
    public class AnimatorParamSetter : MonoBehaviour
    {
        [SerializeField] private string paramName;

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

        /// <summary>
        /// Sets a float parameter.
        /// </summary>
        /// <param name="value"></param>
        public void SetFloat(float value)
        {
            anim.SetFloat(paramName, value);
        }

        /// <summary>
        /// Sets a bool parameter.
        /// </summary>
        /// <param name="value"></param>
        public void SetBool(bool value)
        {
            anim.SetBool(paramName, value);
        }

        /// <summary>
        /// Sets an int parameter.
        /// </summary>
        /// <param name="value"></param>
        public void SetInt(int value)
        {
            anim.SetInteger(paramName, value);
        }
    }
}
