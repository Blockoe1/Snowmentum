/*****************************************************************************
// File Name : ObstacleCollision.cs
// Author : Brandon Koederitz
// Creation Date : 9/18/2025
// Last Modified : 9/18/2025
//
// Brief Description : Allows obstacles to have collisions with the snowball and acts as a middle-man for the snowball
// collisions getting obstacle values.
*****************************************************************************/
using UnityEngine;
using UnityEngine.Events;

namespace Snowmentum
{
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(ObjectScaler))]
    public class ObstacleCollision : MonoBehaviour
    {
        [Header("Events")]
        [SerializeField] private UnityEvent OnDestroyEvent;

        #region Component References
        [Header("Components")]
        [SerializeReference] protected ObjectScaler scaler;

        /// <summary>
        /// Get components on reset.
        /// </summary>
        [ContextMenu("Get Component References")]
        private void Reset()
        {
            scaler = GetComponent<ObjectScaler>();
        }
        #endregion

        #region Properties
        public float ObstacleSize => scaler.Size;
        #endregion

        /// <summary>
        /// Destroys this obstacle.
        /// </summary>
        public void DestroyObstacle()
        {
            OnDestroyEvent?.Invoke();
            Destroy(gameObject);
        }
    }
}
