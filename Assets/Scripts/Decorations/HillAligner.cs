/*****************************************************************************
// File Name : HillAligner.cs
// Author : Brandon Koederitz
// Creation Date : 10/2/2025
// Last Modified : 10/2/2025
//
// Brief Description : Aligns all the hills along a specific horizon line.
*****************************************************************************/
using UnityEngine;

namespace Snowmentum
{

    public class HillAligner : MonoBehaviour
    {
        #region CONSTS
        public const float HORIZON_LINE = 1.375f;
        #endregion

        /// <summary>
        /// Align the hill with the horizon line on awake.
        /// </summary>
        private void Awake()
        {
            transform.position =  new Vector3(transform.position.x, HORIZON_LINE, transform.position.z);
        }
    }
}
