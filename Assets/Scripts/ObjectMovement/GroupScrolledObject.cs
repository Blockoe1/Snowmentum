/*****************************************************************************
// File Name : GroupScrolledObject.cs
// Author : Brandon Koederitz
// Creation Date : 10/16/2025
// Last Modified : 10/16/2025
//
// Brief Description : Controls objects scrolled by a GroupScroller component.
*****************************************************************************/
using UnityEngine;
using UnityEngine.Events;

namespace Snowmentum
{
    public class GroupScrolledObject : MoveModifierController
    {
        [SerializeField] private UnityEvent OnObjectLooped;

        #region Component References    
        [Header("Components")]
        [SerializeReference] protected SpriteRenderer rend;

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
        public float Width
        {
            get
            {
                return rend.size.x * transform.localScale.x;
            }
        }
        #endregion

        /// <summary>
        /// Called by the GroupScroller when this object islooped to the other side of the screen.
        /// </summary>
        public void CallObjectLooped()
        {
            OnObjectLooped?.Invoke();
        }
    }
}
