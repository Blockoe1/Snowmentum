/*****************************************************************************
// File Name : Obstacle Spawner.cs
// Author : Jack Fisher
// Creation Date : 9/22/2025
// Last Modified : 9/23/2025
//
// Brief Description : Continually spawns obstacles.
*****************************************************************************/
using System;
using System.Collections;
using UnityEngine;

namespace Snowmentum
{

    
    public class ObstacleSpawner : MonoBehaviour
    {
        [SerializeField] private ObstacleSpawnData[] obstacles;  //holds the obstacle prefabs
        
        [SerializeField] private int obstacleSpawnAmount = 1;  //amount of obstacles that will be spawned
        [SerializeField] private float spawnCooldown;  //cooldown on spawning obstacles so it isn't going constantly

        //used to set the y axis area the obstacles can spawn in
        [SerializeField] private float minYSpawn;
        [SerializeField] private float maxYSpawn;

        private bool isSpawning;

        #region Nested
        [System.Serializable]
        private class ObstacleSpawnData
        {
            [SerializeField] private ObjectScaler obstaclePrefab;
            [SerializeField] private int baseWeight;
            [SerializeField] private int addedWeight;

            internal int weight;

            #region Properties
            internal GameObject gameObject => obstaclePrefab.gameObject;
            internal float Size => obstaclePrefab.Size;
            #endregion

            #region Weight Updaters
            /// <summary>
            /// Increments the spawn data's weight when it isnt selected.
            /// </summary>
            /// <param name="data"></param>
            internal static void OnNotSelected(ObstacleSpawnData data)
            {
                data.weight += data.addedWeight;
            }

            /// <summary>
            /// Resets the spawn data's weight back to the default when it is selected.
            /// </summary>
            /// <param name="data"></param>
            internal static void OnSelected(ObstacleSpawnData data)
            {
                data.weight = data.baseWeight;
            }
            #endregion
        }
        #endregion

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            StartCoroutine(SpawnObstacles());
        }

        IEnumerator SpawnObstacles()
        {
            isSpawning = true;
            while(isSpawning)
            {
                for (int i = 0; i < obstacleSpawnAmount; i++)
                {
                    float randomY = UnityEngine.Random.Range(minYSpawn, maxYSpawn);
                    //Pick an obstacle prefab
                    GameObject obstacleSpawn = GetObstaclePrefab(obstacles);

                    //Pick a random spawn point
                    Vector3 SpawnArea = transform.position + (Vector3.up * randomY);
                    //Spawn obstacle
                    Instantiate(obstacleSpawn, SpawnArea, Quaternion.identity);
                    
                    //StartCoroutine(SpawnObstacles());
                }
                yield return new WaitForSeconds(spawnCooldown);
            }
        }

        /// <summary>
        /// Gets a random obstacle from a list to spawn.  Uses weighted randomness.
        /// </summary>
        /// <param name="spawnData">The list of obstacles to pick from.</param>
        /// <returns></returns>
        private static GameObject GetObstaclePrefab(ObstacleSpawnData[] spawnData)
        {
            // Find the valid obstacles that can be spawned.
            ObstacleSpawnData[] validObstacles = Array.FindAll(spawnData, CheckValidObstacle);

            // Return null if there are no valid obstacles to spawn at the moment.
            if (validObstacles.Length == 0) { return null; }

            // Calculate the total weight of the valid objects
            int totalWeight = 0;
            foreach(ObstacleSpawnData obs in  validObstacles)
            {
                totalWeight += obs.weight;
            }

            // Get a random weight value
            int randomWeight = UnityEngine.Random.Range(0, totalWeight);

            // Subtract item weight from random weight until it hits 0.
            for (int i = 0; i < validObstacles.Length; i++)
            {
                randomWeight -= validObstacles[i].weight;
                if (randomWeight <= 0)
                {
                    // Before we return our found obstacle, we need to update the weight value of all the
                    // non-selected obstacles.
                    for(int j = i + 1; j < validObstacles.Length; j++)
                    {
                        ObstacleSpawnData.OnNotSelected(validObstacles[j]);
                    }

                    ObstacleSpawnData.OnSelected(validObstacles[i]);

                    return validObstacles[i].gameObject;
                }

                ObstacleSpawnData.OnNotSelected(validObstacles[i]);
            }

            // If all else fails, return null.
            return null;
        }

        /// <summary>
        /// Checks if a certain obstacle is valid to be spawned based on it's size compared to the size of the snowball
        /// </summary>
        /// <param name="obstacleSpawnData"></param>
        /// <returns></returns>
        private static bool CheckValidObstacle(ObstacleSpawnData obstacleSpawnData)
        {
            return obstacleSpawnData.Size < SnowballSize.TargetValue;
        }
    }
}
