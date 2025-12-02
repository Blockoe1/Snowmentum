/*****************************************************************************
// File Name : ObstacleSpawnData.cs
// Author : Brandon Koederitz
// Creation Date : 11/3/2025
// Last Modified : 11/3/2025
//
// Brief Description : Stores data about spawning an obstacle.
*****************************************************************************/
using UnityEngine;

namespace Snowmentum
{
    [System.Serializable]
    public class ObstacleSpawnData
    {
        [SerializeField] private Obstacle obstacle;
        [SerializeField] private int baseWeight;
        [SerializeField] private int addedWeight;

        private int weight;

        #region Properties
        public Obstacle Obs => obstacle;
        public float Size => obstacle.ObstacleSize;
        public int Weight => weight;
        #endregion

        #region Weight Updaters
        /// <summary>
        /// Increments the spawn data's weight when it isnt selected.
        /// </summary>
        /// <param name="data"></param>
        public void OnNotSelected()
        {
            weight += addedWeight;
        }

        /// <summary>
        /// Resets the spawn data's weight back to the default when it is selected.
        /// </summary>
        /// <param name="data"></param>
        public void OnSelected()
        {
            weight = baseWeight;
        }
        #endregion

        #region Properties
        public float RequiredBracketTime
        {  
            get 
            { 
                if (obstacle == null) { return 0f; }
                return obstacle.RequiredBracketTime; 
            } 
        }
        #endregion
    }
}
