/*****************************************************************************
// File Name : ObstacleBracket.cs
// Author : Brandon Koederitz
// Creation Date : 11/3/2025
// Last Modified : 11/3/2025
//
// Brief Description : A ScriptableObject to hold data about the obstacles that spawn in a bracket to clean up the editor.
*****************************************************************************/
using UnityEngine;

namespace Snowmentum
{
    [CreateAssetMenu(fileName = "ObstacleBracket", menuName = "ScriptableObjects/ObstacleBracket")]
    public class ObstacleBracket : ScriptableObject
    {
        [SerializeField] private ObstacleSpawnData[] spawnData;

        #region Properties
        public ObstacleSpawnData[] SpawnData => spawnData;
        #endregion

        /// <summary>
        /// Run OnSelected for all spawn data objects in this bracket so they start with the correct weight.
        /// </summary>
        private void OnEnable()
        {
            if (spawnData != null)
            {
                // Reset all obstacles to their base weight
                foreach (var obstacle in spawnData)
                {
                    obstacle.OnSelected();
                }
            }
        }
    }
}
