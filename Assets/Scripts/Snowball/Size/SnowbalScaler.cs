/*****************************************************************************
// File Name : SnowbalScaler.cs
// Author : Brandon Koederitz
// Creation Date : 9/30/2025
// Last Modified : 9/30/2025
//
// Brief Description : Scales the snowball based on it's size value and the size of the environment
*****************************************************************************/
using UnityEngine;

namespace Snowmentum.Size
{
    public class SnowbalScaler : MonoBehaviour
    {
        #region CONSTS
        // The scale that the snowball should be set at for size 1.
        public static readonly Vector3 REFERENCE_SCALE = new Vector3(1f, 1f, 1f);
        #endregion

        /// <summary>
        /// Subscribe/unsubscribe events.
        /// </summary>
        private void Awake()
        {
            SnowballSize.OnValueChanged += UpdateSnowballScale;
        }
        private void OnDestroy()
        {
            SnowballSize.OnValueChanged -= UpdateSnowballScale;
        }

        /// <summary>
        /// Updates the snowball gameObject's actual scale based on the size value of the snowball and the scale of 
        /// our environemnt
        /// </summary>
        /// <param name="unused1">Not used.  Reference SnowballSize.Value directly.</param>
        /// <param name="unused2">Not Used.  Reference EnvironmentSize.Value directly.</param>
        private void UpdateSnowballScale(float unused1, float unused2)
        {
            // Sets the snowball scale based on it's size and our environment size.
            transform.localScale = REFERENCE_SCALE * (SnowballSize.Value / EnvironmentSize.Value);
            //Debug.Log(EnvironmentSize.Value);
        }
    }
}
