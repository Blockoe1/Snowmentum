/*****************************************************************************
// File Name : ParticleCompleteCallback.cs
// Author : Brandon Koederitz
// Creation Date : 10/9/2025
// Last Modified : 10/9/2025
//
// Brief Description : Relay for the particle system OnParticleSystemStop callback that allows object to be returned
// to the pool when the particle finishes.
*****************************************************************************/
using UnityEngine;
using UnityEngine.Events;

namespace Snowmentum
{
    public class ParticleCompleteRelay : MonoBehaviour
    {
        [SerializeField] private UnityEvent OnParticleStoppedEvent;

        /// <summary>
        /// Called by unity when this particle system stops.
        /// </summary>
        private void OnParticleSystemStopped()
        {
            OnParticleStoppedEvent?.Invoke();
        }
    }
}
