/*****************************************************************************
// File Name : FreezeParticlesController.cs
// Author : Brandon Koederitz
// Creation Date : 10/24/2025
// Last Modified : 10/24/2025
//
// Brief Description : Adjusts the values of the snowball's freeze particles based on the size and speed of the 
// snowball.
*****************************************************************************/
using Snowmentum.Size;
using UnityEngine;

namespace Snowmentum
{
    public class FreezeParticlesController : MonoBehaviour
    {
        [SerializeField] private float speedScale;
        [SerializeField] private float radiusScale;

        #region Component References
        [Header("Components")]
        [SerializeReference] protected ParticleSystem particles;

        /// <summary>
        /// Get components on reset.
        /// </summary>
        [ContextMenu("Get Component References")]
        private void Reset()
        {
            particles = GetComponent<ParticleSystem>();
        }
        #endregion

        /// <summary>
        /// Subscribes/unsubscribes the update functions for the particle system to the events of snowball size and 
        /// speed.
        /// </summary>
        public void StartParticles()
        {
            SnowballSize.OnValueChanged += UpdateRadius;
            SnowballSpeed.OnValueChanged += UpdateSpeed;

            particles.Play();
        }
        public void StopParticles()
        {
            SnowballSize.OnValueChanged -= UpdateRadius;
            SnowballSpeed.OnValueChanged -= UpdateSpeed;

            particles.Stop();
        }

        /// <summary>
        /// Adjusts the speed of the particles based on the speed of the snowball.
        /// </summary>
        /// <param name="oldSpeed"></param>
        /// <param name="newSpeed"></param>
        private void UpdateSpeed(float newSpeed, float oldSpeed)
        {
            //Debug.Log("Updating Speed" + newSpeed);
            // Updates the speed of the particles based on the snowball's speed.
            var velocityModule = particles.velocityOverLifetime;
            var linear = velocityModule.x;
            linear.constant = newSpeed * -speedScale;
            velocityModule.x = linear;
        }

        /// <summary>
        /// Adjusts the radius of the particle system based on the size of the snowball.
        /// </summary>
        /// <param name="newSize"></param>
        /// <param name="oldSize"></param>
        private void UpdateRadius(float newSize, float oldSize)
        {
            var shapeModule = particles.shape;
            shapeModule.radius = newSize * radiusScale / EnvironmentSize.Value;
        }
    }
}
