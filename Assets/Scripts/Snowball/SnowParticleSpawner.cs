/*****************************************************************************
// File Name : ParticleSpawner.cs
// Author : Brandon Koederitz
// Creation Date : 10/9/2025
// Last Modified : 10/9/2025
//
// Brief Description : Allows the snowball to spawn particles when it dies.
*****************************************************************************/
using UnityEngine;

namespace Snowmentum
{
    public class SnowParticleSpawner : MonoBehaviour
    {
        [SerializeField] private ParticleSystem particleToSpawn;
        [SerializeField] private int baseParticles = 3;
        [SerializeField, Tooltip("The number of particles that correspond to one unit of size.")] 
        private int particlesPerSize;

        /// <summary>
        /// Spawns the particle at the position of this object.
        /// </summary>
        public void SpawnParticles(int particleNum)
        {
            ParticleSystem particles = Instantiate(particleToSpawn, transform.position, Quaternion.identity);

            //Debug.Log("Spawning particle with " +  particleNum + " particles.");

            // Adjust the number of particles the particle system will spawn.
            // Update the number of particles emitted by the burst.
            var emissionModue = particles.emission;
            var burst = emissionModue.GetBurst(0);
            burst.count = particleNum + baseParticles;
            emissionModue.SetBurst(0, burst);

            particles.Play();
        }

        /// <summary>
        /// Spawns the particle at the position of this object.  The number of particles spawned is based on the
        /// snowball's size.
        /// </summary>
        public void SpawnParticles(float size)
        {

            //Debug.Log("Spawn particles with size " + size);
            // Calculate the number of particles to spawn based on snowball size;
            // Scale based on environment size.
            SpawnParticles((int)(particlesPerSize * size / EnvironmentSize.Value));
        }
    }
}
