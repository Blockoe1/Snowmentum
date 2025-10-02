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
using Snowmentum.Size;
using System.Collections.Generic;

namespace Snowmentum
{

    
    public class ObstacleSpawner : MonoBehaviour
    {
        [SerializeField] private Transform obstacleParent;
        [SerializeField] private ObstacleController obstaclePrefab;
        [SerializeField] private bool spawnOnStart = true;
        [Header("Spawn Parameters")]
        [SerializeField] private int obstacleSpawnAmount = 1;  //amount of obstacles that will be spawned
        [SerializeField] private float spawnCooldown;  //cooldown on spawning obstacles so it isn't going constantly
        [SerializeField] private bool scaleWithSpeed;

        //used to set the y axis area the obstacles can spawn in
        [SerializeField] private float minYSpawn;
        [SerializeField] private float maxYSpawn;

        //[SerializeField] private ObstacleSpawnData[] obstacles;  //holds the obstacle prefabs
        [SerializeField] private SpawnBracket[] brackets;

        private static Queue<ObstacleController> inactiveObstacles = new();

        private bool isSpawning;

        #region Nested
        [System.Serializable]
        private class SpawnBracket
        {
            [SerializeField] internal ObstacleSpawnData[] spawnData;

            /// <summary>
            /// Run OnSelected for all spawn data objects in this bracket so they start with the correct weight.
            /// </summary>
            internal void Initialize()
            {
                // Reset all obstacles to their base weight
                foreach (var obstacle in spawnData)
                {
                    ObstacleSpawnData.OnSelected(obstacle);
                }
            }
        }
        [System.Serializable]
        private class ObstacleSpawnData
        {
            [SerializeField] private Obstacle obstacle;
            [SerializeField] private int baseWeight;
            [SerializeField] private int addedWeight;

            internal int weight;

            #region Properties
            internal Obstacle Obs => obstacle;
            internal float Size => obstacle.ObstacleSize;
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
            // Initialize all of our brackets with their starting weight.
            foreach(var bracket in brackets)
            {
                bracket.Initialize();
            }
            if (spawnOnStart)
            {
                StartCoroutine(SpawnObstacles(0));
            }
        }

        /// <summary>
        /// Starts spawning obstacles after an initial delay of some time.
        /// </summary>
        /// <param name="initialDelay"></param>
        public void StartSpawning(float initialDelay)
        {
            StartCoroutine(SpawnObstacles(initialDelay));
        }

        IEnumerator SpawnObstacles(float initialDelay)
        {
            yield return new WaitForSeconds(initialDelay);
            isSpawning = true;
            Obstacle obstacleData;
            SpawnBracket spawnBracket;
            ObstacleController spawnedController;
            while(isSpawning)
            {
                // Get the largest bracket we have obstacles set up to spawn in.
                spawnBracket = brackets[Mathf.Min(SizeBracket.Bracket, brackets.Length - 1)];
                for (int i = 0; i < obstacleSpawnAmount; i++)
                {
                    //Pick an obstacle to spawn
                    obstacleData = GetObstacleData(spawnBracket.spawnData);

                    // If no obstacle is valid to be spawned right now, then we should skip spawning.
                    if (obstacleData == null)
                    {
                        continue;
                    }
                    //Pick a random spawn point
                    float randomY = UnityEngine.Random.Range(minYSpawn, maxYSpawn);
                    Vector3 spawnArea = transform.position + (Vector3.up * randomY);

                    //Spawn obstacle and set it up with it's data
                    //Instantiate(obstacleSpawn, SpawnArea, Quaternion.identity, obstacleParent);
                    spawnedController = GetObstacleController();
                    spawnedController.transform.position = spawnArea;
                    spawnedController.SetObstacle(obstacleData);

                    
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
        private static Obstacle GetObstacleData(ObstacleSpawnData[] spawnData)
        {
            // Find the valid obstacles that can be spawned.
            //ObstacleSpawnData[] validObstacles = Array.FindAll(spawnData, CheckValidObstacle);
            ObstacleSpawnData[] validObstacles = spawnData;

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

                    return validObstacles[i].Obs;
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
        //private static bool CheckValidObstacle(ObstacleSpawnData obstacleSpawnData)
        //{
        //    // The obstacle's size must be within double or half of the snowball's size.
        //    return obstacleSpawnData.Size < SnowballSize.TargetValue * SnowballSize.OBSTACLE_RANGE_SCALE && 
        //        obstacleSpawnData.Size > SnowballSize.TargetValue / SnowballSize.OBSTACLE_RANGE_SCALE;
        //}

        /// <summary>
        /// Gets the weight of an obstacle, taking into account the size difference between it and the snowball.
        /// </summary>
        /// <returns></returns>
        private static int GetObstacleWeight(ObstacleSpawnData obstacle)
        {
            //Debug.Log(Mathf.RoundToInt(Mathf.Abs(obstacle.Size - SnowballSize.TargetValue)));
            //int effectiveWeight = Mathf.Max(obstacle.weight - 
            //    Mathf.RoundToInt(Mathf.Abs(obstacle.Size - SnowballSize.TargetValue)), 1);
            //Debug.Log($"Obstacle {obstacle.gameObject.name} has efffective weight of {effectiveWeight}");
            return Mathf.Max(obstacle.weight -
                Mathf.RoundToInt(Mathf.Abs(obstacle.Size - SnowballSize.TargetValue)), 1);
        }

        #region Object Pooling
        /// <summary>
        /// Gets an obstacle GameObject to use for an obstacle to be spawned.
        /// </summary>
        /// <returns>The found obstacle</returns>
        private ObstacleController GetObstacleController()
        {
            ObstacleController toGet = inactiveObstacles.Count > 0 ? inactiveObstacles.Dequeue() : 
                Instantiate(obstaclePrefab, obstacleParent);
            toGet.gameObject.SetActive(true);
            return toGet;
        }

        /// <summary>
        /// Returns an obstacle to be re-used.
        /// </summary>
        /// <param name="obstacle">The obstacle to be returned.</param>
        public static void ReturnObstacle(ObstacleController obstacle)
        {
            obstacle.gameObject.SetActive(false);
            inactiveObstacles.Enqueue(obstacle);
        }
        #endregion
    }
}
