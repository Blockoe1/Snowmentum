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
    public class SnowballScaler : MonoBehaviour
    {
        #region CONSTS
        // The scale that the snowball should be set at for size 1.
        public static readonly Vector3 REFERENCE_SCALE = new Vector3(0.125f, 0.125f, 0.125f);
        #endregion

        #region Component References
        [Header("Components")]
        [SerializeReference] protected Rigidbody2D snowballRigidbody;

        /// <summary>
        /// Get components on reset.
        /// </summary>
        [ContextMenu("Get Component References")]
        private void Reset()
        {
            snowballRigidbody = GetComponent<Rigidbody2D>();
        }
        #endregion

        /// <summary>
        /// Subscribe/unsubscribe events.
        /// </summary>
        private void Awake()
        {
            SnowballSize.OnValueChanged += OnSnowballSize;
            EnvironmentSize.OnValueChanged += OnEnvironmentSize;
        }
        private void OnDestroy()
        {
            SnowballSize.OnValueChanged -= OnSnowballSize;
            EnvironmentSize.OnValueChanged -= OnEnvironmentSize;
        }

        /// <summary>
        /// Handles changes to the snowball's size.
        /// </summary>
        /// <param name="newSnowballSize">The newsize of the snowball</param>
        /// <param name="oldSnowballSize">the old size of the snowball</param>
        private void OnSnowballSize(float newSnowballSize, float oldSnowballSize)
        {
            //When the snowball's size changes, just directly update the snowball scale.
            UpdateSnowballScale();
        }

        /// <summary>
        /// Handles changes to the environment's size
        /// </summary>
        /// <param name="newEnvSize">The new size of the environment</param>
        /// <param name="oldEnvSize">The old size of the environment</param>
        private void OnEnvironmentSize(float newEnvSize, float oldEnvSize)
        {
            // calculate a theoretical old and new sizes so we ignore natural growth to the snowball.
            float oldSize = 1 / oldEnvSize;
            float newSize = 1 / newEnvSize;
            SnowballPerspective(oldSize, newSize);

            // Update the snowball's actual scale.
            UpdateSnowballScale();
        }

        /// <summary>
        /// Updates the scale of the snowball based on the size of it and the environment.
        /// </summary>
        private void UpdateSnowballScale()
        {
            // Prevent /0 error.
            if (SnowballSize.Value == 0 || EnvironmentSize.Value == 0) { return; }
            // Sets the snowball scale based on it's size and our environment size.
            transform.localScale = REFERENCE_SCALE * (SnowballSize.Value / EnvironmentSize.Value);
        }

        /// <summary>
        /// Moves the snowball based on how it scales to keep perspective.
        /// </summary>
        private void SnowballPerspective(float oldScale, float newScale)
        {
            //Debug.Log($"OldScale: {oldScale}.  NewScale: {newScale}.");
            //Debug.Log(SizeHelpers.CalculateScaledPosition(EnvironmentSize.ScalePivot,
            //    snowballRigidbody.position, oldScale, newScale));
            //Debug.Log("Current Position " + snowballRigidbody.position);
            //Debug.Log("New Position" + SizeHelpers.CalculateScaledPosition(EnvironmentSize.ScalePivot,
            //    snowballRigidbody.position, oldScale, newScale));

            // MovePosition refuses to work, so I've switched it over to .position instead.
            snowballRigidbody.position = SizeHelpers.CalculateScaledPosition(EnvironmentSize.ScalePivot, 
                snowballRigidbody.position, oldScale, newScale);
        }
    }
}
