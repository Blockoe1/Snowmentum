/*****************************************************************************
// File Name : ObstacleSettings.cs
// Author : Brandon Koederitz
// Creation Date : 9/13/2025
// Last Modified : 9/13/2025
//
// Brief Description : Stores settings that are universal for all obstacle types.
*****************************************************************************/
using UnityEngine;

namespace Snowmentum
{
    [CreateAssetMenu(fileName = "ObstacleSettings", menuName = "ScriptableObjects/ObstacleSettings")]
    public class ObstacleSettings : ScriptableObject
    {
        [SerializeField, Tooltip("The angle that the snowball moves at, based on the approximate angle of " +
        "the hillside.  Should be based on 0 degrees being to the right.")]
        private float moveAngle;
        [SerializeField, Tooltip("If true, then the obstacle's position in the hill will automatically be adjusted" +
    " to add to the illusion of the snowball getting bigger.")]
        private bool scalePerspective;
        [SerializeField, Tooltip("When set to true, the object will scale it's position based on it's starting size" +
    " when it spawns.  This results in obstacles spawning at varied locations based on their size.")]
        private bool scaleOnSpawn;

        [SerializeField, HideInInspector] private Vector2 moveVector;

        #region Properties
        public bool ScalePerspective => scalePerspective;
        public bool ScaleOnSpawn => scaleOnSpawn;
        public float MoveAngle => moveAngle;
        public Vector2 MoveVector => moveVector;
        #endregion

        /// <summary>
        /// When the value of MoveAngle is changed, we should update our value of MoveVector automatically.
        /// </summary>
        private void OnValidate()
        {
            moveVector = MathHelpers.DegAngleToUnitVector(moveAngle);
        }
    }
}
