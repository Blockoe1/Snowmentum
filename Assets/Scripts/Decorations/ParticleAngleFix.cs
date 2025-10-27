/*****************************************************************************
// File Name : ParticleAngleFix.cs
// Author : Brandon Koederitz
// Creation Date : 10/23/2025
// Last Modified : 10/23/2025
//
// Brief Description : Quick script to fix particles falling off of the hill.
*****************************************************************************/
using UnityEngine;

namespace Snowmentum
{
    public class ParticleAngleFix : MonoBehaviour
    {
        [SerializeField] private Transform collisionPlane;
        [SerializeField] private float maxY = 1;
        [SerializeField] private float minY = -0.15f;
        [SerializeField] private float minAngle = -10;
        [SerializeField] private float maxAngle = -30;

        /// <summary>
        /// Adjusts the angle of the collision plane based on the particle's Y position.
        /// </summary>
        private void Awake()
        {
            float lerpValue = Mathf.InverseLerp(minY, maxY, transform.position.y);
            float angle = Mathf.Lerp(maxAngle, minAngle, lerpValue);
            //Debug.Log(angle);
            collisionPlane.eulerAngles = new Vector3(angle, 0, 0);
        }
    }
}
