using System.Collections;
using UnityEngine;

namespace Snowmentum
{

    
    public class ObstacleSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject[] obstaclePrefabs;  //holds the obstacle prefabs
        [SerializeField] private Transform[] spawnPoints;  //array holds the obstacle spawnpoints
        [SerializeField] private int obstacleSpawnAmount = 1;  //amount of obstacles that will be spawned
        [SerializeField] private float spawnCooldown;  //cooldown on spawning obstacles so it isn't going constantly


        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
        StartCoroutine(SpawnEnemies());
        }

        IEnumerator SpawnEnemies()
        {

            for (int i = 0; i < obstacleSpawnAmount; i++)
            {
                //Pick an obstacle prefab
                GameObject obstacleSpawn = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];
                //Pick a random spawn point
                Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
                //Spawn obstacle
                Instantiate(obstacleSpawn, spawnPoint.position, spawnPoint.rotation);
                yield return new WaitForSeconds(spawnCooldown);
                StartCoroutine(SpawnEnemies());
            }
        }
    }
}
