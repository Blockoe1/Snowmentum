/*****************************************************************************
// File Name : KillZone.cs
// Author : Brandon Koederitz
// Creation Date : 9/23/2025
// Last Modified : 9/23/2025
//
// Brief Description : Destroys a moving object when it goes outside of a given bounds.
*****************************************************************************/
using UnityEngine;

namespace Snowmentum
{
    public class KillZone : MonoBehaviour, IMovementModifier
    {
        [SerializeField] private float killZone;

        /// <summary>
        /// If the object is beyond the kill zone, then mark it for destruction.
        /// </summary>
        /// <param name="targetPos"></param>
        /// <param name="moveVector"></param>
        /// <returns></returns>
        public Vector2 MoveUpdate(Transform movedObject, Vector2 targetPos, Vector2 moveVector)
        {
            if (Mathf.Abs(targetPos.x) > killZone)
            {
                Destroy(movedObject.gameObject);
            }
            return targetPos;
        }
    }
}
