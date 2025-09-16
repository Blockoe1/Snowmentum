/*****************************************************************************
// File Name : MovementSettings.cs
// Author : Brandon Koederitz
// Creation Date : 9/13/2025
// Last Modified : 9/13/2025
//
// Brief Description : Stores settings that determine how a specific type of objects (background objects for parralax,
// obstacles, etc.) move.
*****************************************************************************/
using UnityEngine;

namespace Snowmentum
{
    [CreateAssetMenu(fileName = "MovementSettings", menuName = "ScriptableObjects/MovementSettings")]
    public class MovementSettings : ScriptableObject
    {
        [Header("Movement")]
        [SerializeField, Tooltip("The angle that the snowball moves at, based on the approximate angle of " +
        "the hillside.  Should be based on 0 degrees being to the right.")]
        private float moveAngle;
        [SerializeField, Tooltip("How quickly this object should move in comparison to the snowball's speed.  " +
            "Obstacles should have this value set to 1.")]
        private float speedScale = 1;
        [Header("Scaling")]
        [SerializeField, Tooltip("If true, then the obstacle's position in the hill will automatically be adjusted" +
    " to add to the illusion of the snowball getting bigger.")]
        private bool scalePerspective;
        [SerializeField, Tooltip("When set to true, the object will scale it's position based on it's starting size" +
    " when it spawns.  This results in obstacles spawning at varied locations based on their size.")]
        private bool scaleOnSpawn;

        [SerializeField, HideInInspector] private Vector2 moveVector;

        #region Properties
        public bool ScalePerspective => scalePerspective;
        public float SpeedScale => speedScale;
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
