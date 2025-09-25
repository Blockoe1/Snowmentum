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
        [SerializeField] private Transform obstacleParent;
        [Header("Spawn Parameters")]
        [SerializeField] private int obstacleSpawnAmount = 1;  //amount of obstacles that will be spawned
        [SerializeField] private float spawnCooldown;  //cooldown on spawning obstacles so it isn't going constantly
        [SerializeField] private bool scaleWithSpeed;

        //used to set the y axis area the obstacles can spawn in
        [SerializeField] private float minYSpawn;
        [SerializeField] private float maxYSpawn;

        [SerializeField] private ObstacleSpawnData[] obstacles;  //holds the obstacle prefabs

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
            // Reset all obstacles to their base weight
            foreach(var obstacle in obstacles)
            {
                ObstacleSpawnData.OnSelected(obstacle);
            }
            StartCoroutine(SpawnObstacles());
        }

        IEnumerator SpawnObstacles()
        {
            isSpawning = true;
            GameObject obstacleSpawn;
            while(isSpawning)
            {
                for (int i = 0; i < obstacleSpawnAmount; i++)
                {
                    //Pick an obstacle prefab
                    obstacleSpawn = GetObstaclePrefab(obstacles);

                    // If no obstacle is valid to be spawned right now, then we should skip spawning.
                    if (obstacleSpawn == null)
                    {
                        continue;
                    }
                    //Pick a random spawn point
                    float randomY = UnityEngine.Random.Range(minYSpawn, maxYSpawn);
                    Vector3 SpawnArea = transform.position + (Vector3.up * randomY);

                    //Spawn obstacle
                    Instantiate(obstacleSpawn, SpawnArea, Quaternion.identity, obstacleParent);
                    
                    //StartCoroutine(SpawnObstacles());
                }

                if (scaleWithSpeed)
                {
                    float timer = spawnCooldown;
                    while(timer > 0)
                    {
                        timer -= Time.deltaTime * SnowballSpeed.Value; 
                        yield return null;
                    }
                }
                else
                {
                    yield return new WaitForSeconds(spawnCooldown);
                }                
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
                totalWeight += GetObstacleWeight(obs);
            }

            // Get a random weight value
            int randomWeight = UnityEngine.Random.Range(0, totalWeight);

            // Subtract item weight from random weight until it hits 0.
            for (int i = 0; i < validObstacles.Length; i++)
            {
                randomWeight -= GetObstacleWeight(validObstacles[i]);
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
            // The obstacle's size must be within double or half of the snowball's size.
            return obstacleSpawnData.Size < SnowballSize.TargetValue * SnowballSize.OBSTACLE_RANGE_SCALE && 
                obstacleSpawnData.Size > SnowballSize.TargetValue / SnowballSize.OBSTACLE_RANGE_SCALE;
        }

        /// <summary>
        /// Gets the weight of an obstacle, taking into account the size difference between it and the snowball.
        /// </summary>
        /// <returns></returns>
        private static int GetObstacleWeight(ObstacleSpawnData obstacle)
        {
            //Debug.Log(Mathf.RoundToInt(Mathf.Abs(obstacle.Size - SnowballSize.TargetValue)));
            int effectiveWeight = Mathf.Max(obstacle.weight - 
                Mathf.RoundToInt(Mathf.Abs(obstacle.Size - SnowballSize.TargetValue)), 1);
            //Debug.Log($"Obstacle {obstacle.gameObject.name} has efffective weight of {effectiveWeight}");
            return effectiveWeight;
        }
    }
}
