/*****************************************************************************
// File Name : Obstacle Spawner.cs
// Author : Jack Fisher
// Creation Date : 9/22/2025
// Last Modified : 9/23/2025
//
// Brief Description : Continually spawns obstacles.
*****************************************************************************/
using Snowmentum.Size;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Snowmentum
{

    public delegate void ObstacleReturnFunction(ObstacleController toReturn);
    public class ObstacleSpawner : MonoBehaviour
    {
        [SerializeField] private Transform obstacleParent;
        [SerializeField] private ObstacleController obstaclePrefab;
        [SerializeField] private bool spawnOnStart = true;
        [Header("Spawn Parameters")]
        [SerializeField, Tooltip("The number of obstacles that spawn after the cooldown.")] 
        private int obstacleSpawnAmount = 1;  //amount of obstacles that will be spawned
        [SerializeField, Tooltip("The amount of time to wait between spawning obstacles.")]
        private float spawnCooldown;  //cooldown on spawning obstacles so it isn't going constantly
        [SerializeField, Tooltip("If set to true, the rate obstacles spawn at will be adjusted based on the " +
            "current speed of the snowball")]
        private bool scaleWithSpeed;
        [SerializeField, Tooltip("Controls how much spawned obstacles skew towards being close in size to " +
            "the snowball.")] 
        private float maxBonusWeight;

        //used to set the y axis area the obstacles can spawn in
        [SerializeField] private float minYSpawn;
        [SerializeField] private float maxYSpawn;

        //this will make the obstacles spawn a set distance apart to stop walls of obstacles forming
        [SerializeField] private float minimumYDistance = 2f;

        //check for last Y spawn also needed to make sure obstacles are a certain distance apart. Set to infinity so first spawn is always valid. 
        private float lastYSpawn = Mathf.Infinity;

        //[SerializeField] private ObstacleSpawnData[] obstacles;  //holds the obstacle prefabs
        [Header("Brackets")]
        //[SerializeField] private SpawnBracket[] brackets;
        //[SerializeField] private SpawnBracket infiniteBracket;
        [SerializeField] private ObstacleBracket[] brackets;
        [SerializeField] private ObstacleBracket infiniteBracket;


        private readonly Queue<ObstacleController> inactiveObstacles = new();

        private float bracketTime;
        private bool isSpawning;
        private bool isPaused;


        #region Properties
        public bool IsPaused
        {
            get { return isPaused; }
            set { isPaused = value; }
        }
        #endregion


        //#region Nested
        //[System.Serializable]
        //private class SpawnBracket
        //{
        //    [SerializeField] internal ObstacleSpawnData[] spawnData;

        //    /// <summary>
        //    /// Run OnSelected for all spawn data objects in this bracket so they start with the correct weight.
        //    /// </summary>
        //    internal void Initialize()
        //    {
        //        // Reset all obstacles to their base weight
        //        foreach (var obstacle in spawnData)
        //        {
        //            ObstacleSpawnData.OnSelected(obstacle);
        //        }
        //    }
        //}
        //[System.Serializable]
        //private class ObstacleSpawnData
        //{
        //    [SerializeField] private Obstacle obstacle;
        //    [SerializeField] private int baseWeight;
        //    [SerializeField] private int addedWeight;

        //    internal int weight;

        //    #region Properties
        //    internal Obstacle Obs => obstacle;
        //    internal float Size => obstacle.ObstacleSize;
        //    #endregion

        //    #region Weight Updaters
        //    /// <summary>
        //    /// Increments the spawn data's weight when it isnt selected.
        //    /// </summary>
        //    /// <param name="data"></param>
        //    internal static void OnNotSelected(ObstacleSpawnData data)
        //    {
        //        data.weight += data.addedWeight;
        //    }

        //    /// <summary>
        //    /// Resets the spawn data's weight back to the default when it is selected.
        //    /// </summary>
        //    /// <param name="data"></param>
        //    internal static void OnSelected(ObstacleSpawnData data)
        //    {
        //        data.weight = data.baseWeight;
        //    }
        //    #endregion
        //}
        //#endregion

        /// <summary>
        /// Subscribe/Unsubscribe to OnBracketChanged event to reset bracket time every time we enter a new bracket.
        /// </summary>
        private void Awake()
        {
            SizeBracket.OnBracketChanged += ResetBracketTime;
        }
        private void OnDestroy()
        {
            SizeBracket.OnBracketChanged -= ResetBracketTime;
        }

        /// <summary>
        /// Resets bracket time when we enter a new bracket.
        /// </summary>
        /// <param name="newBracket"></param>
        /// <param name="oldBracket"></param>
        private void ResetBracketTime(int newBracket, int oldBracket)
        {
            // Only reset bracket time if we went up a bracket.
            if (newBracket > oldBracket)
            {
                bracketTime = 0;
                Debug.Log("Reset bracket time.");
            }
        }


        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            //// Initialize all of our brackets with their starting weight.
            //foreach(var bracket in brackets)
            //{
            //    bracket.Initialize();
            //}
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
            ObstacleBracket spawnBracket;
            ObstacleController spawnedController;
            while(isSpawning)
            {
                // Get the largest bracket we have obstacles set up to spawn in.
                // Decrement bracket by 1 so that it's an index.
                //spawnBracket = brackets[Mathf.Min(SizeBracket.Bracket - 1, brackets.Length - 1)];

                // If our current bracket exceeds the brackets we've set, use the infinite bracket.
                spawnBracket = SizeBracket.Bracket > brackets.Length ? 
                    infiniteBracket : brackets[SizeBracket.Bracket - 1];
                for (int i = 0; i < obstacleSpawnAmount; i++)
                {
                    //Pick an obstacle to spawn
                    obstacleData = GetObstacleData(spawnBracket.SpawnData, maxBonusWeight, bracketTime);

                    // If no obstacle is valid to be spawned right now, then we should skip spawning.
                    if (obstacleData == null)
                    {
                        //Debug.Log("No obstacle");
                        continue;
                    }

                    //Repeatedly pick certain spawnpoints until one is the minimumYdistance away from the previous spawn
                    float randomY = UnityEngine.Random.Range(minYSpawn, maxYSpawn);

                    //I am realizing that this system doesn't work super well with negative numbers
                    //it shouldn't be game breaking but is worth fixing probably
                    while (Mathf.Abs(randomY - lastYSpawn) < minimumYDistance)
                    {
                        randomY = UnityEngine.Random.Range(minYSpawn, maxYSpawn);
                        
                    }
                    
                    lastYSpawn = randomY;

                    
                    
                    Vector3 spawnArea = transform.position + (Vector3.up * randomY);

                    //Spawn obstacle and set it up with it's data
                    //Instantiate(obstacleSpawn, SpawnArea, Quaternion.identity, obstacleParent);
                    spawnedController = GetObstacleController();
                    spawnedController.Reset(spawnArea);
                    spawnedController.SetObstacle(obstacleData);
                    spawnedController.ReturnFunction = ReturnObstacle;

                    //Debug.Log("Spawned obstacle " + obstacleData);
                    
                    //StartCoroutine(SpawnObstacles());
                }

                float timer = spawnCooldown;
                while (timer > 0)
                {
                    // Prevent the timer from decreasing if spawning is paused.
                    if (!isPaused)
                    {
                        float timerDecay = scaleWithSpeed ? Time.deltaTime * SnowballSpeed.Value : Time.deltaTime;
                        timer -= timerDecay;
                        // Track the amount of time that we've been in a certain bracket.
                        bracketTime += Time.deltaTime;
                    }
                    yield return null;
                }              
            }
        }


        /// <summary>
        /// Gets a random obstacle from a list to spawn.  Uses weighted randomness.
        /// </summary>
        /// <param name="spawnData">The list of obstacles to pick from.</param>
        /// <returns></returns>
        private static Obstacle GetObstacleData(ObstacleSpawnData[] spawnData, float maxBonusWeight, float bracketTime)
        {
            // Return null if there are no valid obstacles to spawn at the moment.
            if (spawnData.Length == 0) { return null; }

            // Items should never be null since spawnData is set in the editor.
            // Filter out invalid obstacles that are outside the bracket time
            spawnData = Array.FindAll(spawnData, item => item.RequiredBracketTime < bracketTime);

            // Calculate the total weight of the valid objects
            int totalWeight = 0;
            foreach(ObstacleSpawnData obs in spawnData)
            {
                totalWeight += GetObstacleWeight(obs, maxBonusWeight);
            }

            // Get a random weight value
            int randomWeight = UnityEngine.Random.Range(0, totalWeight);

            // Subtract item weight from random weight until it hits 0.
            for (int i = 0; i < spawnData.Length; i++)
            {
                randomWeight -= GetObstacleWeight(spawnData[i], maxBonusWeight);
                if (randomWeight <= 0)
                {
                    // Before we return our found obstacle, we need to update the weight value of all the
                    // non-selected obstacles.
                    for(int j = i + 1; j < spawnData.Length; j++)
                    {
                        spawnData[j].OnNotSelected();
                    }

                    spawnData[i].OnSelected();

                    return spawnData[i].Obs;
                }

                spawnData[i].OnNotSelected();
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
        private static int GetObstacleWeight(ObstacleSpawnData obstacle, float maxBonusWeight)
        {
            // Divide by EnvironmentSize so that the comparison isn't affected by the current bracket.
            float sizeComparison = (obstacle.Size - SnowballSize.TargetValue) / EnvironmentSize.Value;

            // Uses the formula
            // 2^-sizeComparison^2 * maxBonusWeight
            // to calculate the bonus weight
            int bonusWeight = Mathf.RoundToInt(Mathf.Pow(2, -Mathf.Pow(sizeComparison, 2)) * maxBonusWeight);


            //Debug.Log("Obstacle " + obstacle.Obs + " has a bonus weight of " + bonusWeight);
            return Mathf.Max(obstacle.Weight + bonusWeight, 1);
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
        public void ReturnObstacle(ObstacleController obstacle)
        {
            obstacle.gameObject.SetActive(false);
            inactiveObstacles.Enqueue(obstacle);
        }
        #endregion
    }
}
