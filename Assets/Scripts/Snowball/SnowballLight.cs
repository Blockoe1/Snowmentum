/*****************************************************************************
// File Name : SnowballLight.cs
// Author : Brandon koederitz
// Creation Date : 11/18/2025
// Last Modified : 11/18/2025
//
// Brief Description : Scales the snowball light based on the snowball's size.
*****************************************************************************/
using Snowmentum.Size;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Snowmentum
{
    public class SnowballLight : MonoBehaviour
    {
        [SerializeField] private float radiusScale;
        [SerializeField] private float innerOuterDist;

        #region Component References
        [Header("Components")]
        [SerializeReference] protected Light2D snowballLight;

        /// <summary>
        /// Get components on reset.
        /// </summary>
        [ContextMenu("Get Component References")]
        private void Reset()
        {
            snowballLight = GetComponent<Light2D>();
        }
        #endregion

        /// <summary>
        /// Subscribe events.
        /// </summary>
        private void Awake()
        {
            SnowballSize.OnValueChanged += UpdateLightRadius;
        }
        private void OnDestroy()
        {
            SnowballSize.OnValueChanged -= UpdateLightRadius;
        }

        /// <summary>
        /// Updates the radius of the light to match the snowball's radius.
        /// </summary>
        /// <param name="newSize"></param>
        /// <param name="oldSize"></param>
        private void UpdateLightRadius(float newSize, float oldSize)
        {
            snowballLight.pointLightInnerRadius = newSize * radiusScale / EnvironmentSize.Value;
            snowballLight.pointLightOuterRadius = (newSize * radiusScale / EnvironmentSize.Value) + innerOuterDist;
        }
    }
}
