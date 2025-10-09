/*****************************************************************************
// File Name : TimerDisplayer.cs
// Author : Jack Fisher
// Creation Date : October 3, 2025
// Last Modified : October 3, 2025
//
// Brief Description : This script pools objects to be spawned in the background,
then will repeatedly choose from those objects at random to be spawned in random intervals.
*****************************************************************************/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Snowmentum
{


    public class BackgroundObjectsPooler : MonoBehaviour

    {
        [SerializeField] private Transform poolParent;
        public List<Pool> pools;
        public Dictionary<string, Queue<GameObject>> poolDictionary;


        public static BackgroundObjectsPooler Instance;

        private void Awake()
        {

            Instance = this;
        }


        private void Start()
        {

            poolDictionary = new Dictionary<string, Queue<GameObject>>();

            foreach (Pool pool in pools)
            {
                Queue<GameObject> objectPool = new Queue<GameObject>();

                for (int i = 0; i < pool.size; i++)
                {
                    GameObject obj = Instantiate(pool.backgroundPrefab, poolParent);
                    obj.SetActive(false);
                    objectPool.Enqueue(obj);

                }
                poolDictionary.Add(pool.tag, objectPool);
            }

        }
        public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
        {
            if (!poolDictionary.ContainsKey(tag))
            {
                Debug.LogWarning($"Pool with tag {tag} doesn’t exist.");
                return null;
            }

            GameObject obj = poolDictionary[tag].Dequeue();
            obj.SetActive(true);
            obj.transform.position = position;
            obj.transform.rotation = rotation;
            poolDictionary[tag].Enqueue(obj);

            return obj;
        }
        [System.Serializable]
        public class Pool
        {
            public string tag;
            public GameObject backgroundPrefab;
            public int size = 1; //amount of times one prefab will be pooled. 

        }
    }
}
