/*****************************************************************************
// File Name : PoolReturner.cs
// Author : Brandon Koederitz
// Creation Date : 10/21/2025
// Last Modified : 10/21/2025
//
// Brief Description : Returns a spawned object to the object pool.
*****************************************************************************/
using UnityEngine;

namespace Snowmentum
{
    public class PoolReturner : MonoBehaviour
    {
        [SerializeField] private string poolTag;

        /// <summary>
        /// Returns this game object to it's specified pool.
        /// </summary>
        public void Return()
        {
            Debug.Log("Returned");
            BackgroundObjectsPooler.Instance.ReturnToPool(gameObject, poolTag);
        }
    }
}
