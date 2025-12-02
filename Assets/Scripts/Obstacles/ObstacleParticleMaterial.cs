/*****************************************************************************
// File Name : ObstacleParticleMaterial.cs
// Author : Brandon Koederitz
// Creation Date : 12/1/2025
// Last Modified : 12/1/2025
//
// Brief Description : Data container for sprites that make up sprite sheet for destruction particles.
*****************************************************************************/
using UnityEngine;

namespace Snowmentum
{
    [CreateAssetMenu(fileName = "ObstacleParticleMaterial", menuName = "ScriptableObjects/ObstacleParticleMaterial")]
    public class ObstacleParticleMaterial : ScriptableObject
    {
        [SerializeField] private Sprite[] particleSpriteSheet = new Sprite[6];

        #region Properties
        public Sprite[] ParticleSpriteSheet { get { return particleSpriteSheet; } }
        #endregion
    }
}
