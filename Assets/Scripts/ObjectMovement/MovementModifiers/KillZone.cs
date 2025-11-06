/*****************************************************************************
// File Name : KillZone.cs
// Author : Brandon Koederitz
// Creation Date : 9/23/2025
// Last Modified : 9/23/2025
//
// Brief Description : Destroys a moving object when it goes outside of a given bounds.
*****************************************************************************/
using UnityEngine;
using UnityEngine.Events;

namespace Snowmentum
{
    public class KillZone : MonoBehaviour, IMovementModifier
    {
        [SerializeField] private KillZoneDimension x;
        [SerializeField] private KillZoneDimension y;
        [SerializeField] private UnityEvent OnEnterKillzone;

        #region Nested
        [System.Serializable]
        private struct KillZoneDimension
        {
            [SerializeField] internal float killZone;
            [SerializeField] internal bool checkNegative;
            [SerializeField] internal bool checkPositive;
        }

        #endregion

        /// <summary>
        /// If the object is beyond the kill zone, then mark it for destruction.
        /// </summary>
        /// <param name="targetPos"></param>
        /// <param name="moveVector"></param>
        /// <returns></returns>
        public Vector2 MoveUpdate(Transform movedObject, Vector2 targetPos, Vector2 moveVector)
        {
            // Check for exceeding the positive and negative kill zones separately.
            if (CheckOutOfBounds(targetPos.x, x) || CheckOutOfBounds(targetPos.y, y))
            {
                Debug.Log("Hit kill zone at position " + targetPos);
                OnEnterKillzone?.Invoke();
                //Destroy(movedObject.gameObject);
            }
            return targetPos;
        }

        /// <summary>
        /// Checks if this object has exceeded a kill zone.
        /// </summary>
        /// <param name="targetPos"></param>
        /// <param name="dim"></param>
        /// <returns></returns>
        private static bool CheckOutOfBounds(float targetPos, KillZoneDimension dim)
        {
            return (dim.checkPositive && targetPos > dim.killZone) || (dim.checkNegative && targetPos < -dim.killZone);
        }
    }
}
