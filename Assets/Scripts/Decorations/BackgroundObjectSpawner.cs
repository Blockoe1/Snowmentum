/*****************************************************************************
// File Name : BackgroundObjectsPooler.cs
// Author : Jack Fisher
// Creation Date : October 5, 2025
// Last Modified : October 5, 2025
//
// Brief Description : This script chooses a random object from the list of pooled background objects 
after random amounts of time, and spawns them to move across the background.
*****************************************************************************/
using System.Collections;
using UnityEngine;

namespace Snowmentum
{
    public class BackgroundObjectSpawner : MonoBehaviour
    {
        public string[] spawnTags = { "Hill", "Tree", "House" }; //Add tags for each background object here
        [SerializeField] private float minDelay = 2f;
        [SerializeField] private float maxDelay = 5f;
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private bool spawnOnStart;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            if (spawnOnStart)
            {
                StartSpawning();
            }
        }

        public void StartSpawning()
        {
            StartCoroutine(SpawnLoop());
        }

        IEnumerator SpawnLoop()
        {
            float Delay = Random.Range(minDelay, maxDelay);
            yield return new WaitForSeconds(Delay);

            string randomTag = spawnTags[Random.Range(0, spawnTags.Length)]; //picks a random object
            BackgroundObjectsPooler.Instance.SpawnFromPool(randomTag, spawnPoint.position, spawnPoint.rotation);
            StartCoroutine(SpawnLoop());
        }
    }
}
