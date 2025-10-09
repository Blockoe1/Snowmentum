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
    public class ParticleSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject particleToSpawn;

        /// <summary>
        /// Spawns the particle at the position of this object.
        /// </summary>
        public void SpawnParticles()
        {
            Instantiate(particleToSpawn, transform.position, Quaternion.identity);
        }
    }
}
