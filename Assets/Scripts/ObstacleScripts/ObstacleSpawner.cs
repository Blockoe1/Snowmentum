using System.Collections;
using UnityEngine;

namespace Snowmentum
{

    
    public class ObstacleSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject[] obstaclePrefabs;  //holds the obstacle prefabs
        
        [SerializeField] private int obstacleSpawnAmount = 1;  //amount of obstacles that will be spawned
        [SerializeField] private float spawnCooldown;  //cooldown on spawning obstacles so it isn't going constantly

        //used to set the y axis area the obstacles can spawn in
        [SerializeField] private float minYSpawn;
        [SerializeField] private float maxYSpawn;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
        StartCoroutine(SpawnObstacles());
        }

        IEnumerator SpawnObstacles()
        {

            for (int i = 0; i < obstacleSpawnAmount; i++)
            {
                float randomY = Random.Range(minYSpawn, maxYSpawn);
                //Pick an obstacle prefab
                GameObject obstacleSpawn = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];
                //Pick a random spawn point
                Vector3 SpawnArea = new Vector3(5, randomY, 0);
                //Spawn obstacle
                Instantiate(obstacleSpawn, SpawnArea, Quaternion.identity);
                yield return new WaitForSeconds(spawnCooldown);
                StartCoroutine(SpawnObstacles());
            }
        }
    }
}
