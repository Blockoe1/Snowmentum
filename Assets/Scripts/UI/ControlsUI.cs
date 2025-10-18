/*****************************************************************************
// File Name : ControlsUI.cs
// Author : Brandon Koederitz
// Creation Date : 10/18/2025
// Last Modified : 10/18/2025
//
// Brief Description : Controls the animations of the UI object that displays the controls to the player.
*****************************************************************************/
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

namespace Snowmentum.UI
{
    public class ControlsUI : MonoBehaviour
    {
        #region CONSTS
        private const int MAX_DIRECTION_INT = 15;
        private const string ANIM_HORIZONTAL_NAME = "Horizontal";
        private const string ANIM_VERTICAL_NAME = "Vertical";
        #endregion

        [SerializeField] private ControlsDirection direction;

        private Vector2Int previousDirection;

        #region Component References
        [Header("Components")]
        [SerializeReference] protected Animator anim;

        /// <summary>
        /// Get components on reset.
        /// </summary>
        [ContextMenu("Get Component References")]
        private void Reset()
        {
            anim = GetComponent<Animator>();
        }
        #endregion

        #region Properties
        public ControlsDirection Direction
        {
            get { return direction; }
            set { direction = value; }
        }
        #endregion

        /// <summary>
        /// Set a random animation direction on awake so we skip the default left animation.
        /// </summary>
        private void Awake()
        {
            SetRandomDirection();
        }


        /// <summary>
        /// Sets the value of this enum based on an integer value
        /// </summary>
        /// <param name="value">
        /// The integer value to set the direction flags with.  Follows this key:
        /// 0 = None.
        /// 1 = Left
        /// 2 = Right
        /// 3 = Up
        /// 4 = Down
        /// To add multiple directions, add the numbers together.
        /// </param>
        public void SetDirection(int value)
        {
            value = Mathf.Clamp(value, 0, MAX_DIRECTION_INT);
            this.direction = (ControlsDirection)value;

            // Hide this object if we have no direction.
            if (direction == ControlsDirection.None)
            {
                gameObject.SetActive(false);
            }
            else
            {
                gameObject.SetActive(true);
                // Update the animator's direction.
                SetRandomDirection();
            }
        }

        /// <summary>
        /// Sets the animator to play the animation for a random direction.
        /// </summary>
        public void SetRandomDirection()
        {
            Vector2Int[] directions = GetDirections(direction);
            // If directions is invalid or there is only one valid direction, skip updating the animations.
            if (directions == null || directions.Length == 0)
            {
                return;
            }
            // Get a random valid direction.
            Vector2Int dir = directions[Random.Range(0, directions.Length)];
            previousDirection = dir;
            Debug.Log(previousDirection);

            anim.SetInteger(ANIM_HORIZONTAL_NAME, dir.x);
            anim.SetInteger(ANIM_VERTICAL_NAME, dir.y);
        }

        /// <summary>
        /// Converts a ControlsDirection enum into an array of all Vector2Ints representing the directions stored
        /// in that enum.
        /// </summary>
        /// <param name="directions"></param>
        /// <returns></returns>
        private Vector2Int[] GetDirections(ControlsDirection directions)
        {
            List<Vector2Int> outputList = new List<Vector2Int>();
            // Manually check and add each direction flag.
            // Exclude the previously used direction.
            if (direction.HasFlag(ControlsDirection.Left) && previousDirection != Vector2Int.left)
            {
                outputList.Add(Vector2Int.left);
            }
            if (direction.HasFlag(ControlsDirection.Right) && previousDirection != Vector2Int.right)
            {
                outputList.Add(Vector2Int.right);
            }
            if (direction.HasFlag(ControlsDirection.Up) && previousDirection != Vector2Int.up)
            {
                outputList.Add(Vector2Int.up);
            }
            if (direction.HasFlag(ControlsDirection.Down) && previousDirection != Vector2Int.down)
            {
                outputList.Add(Vector2Int.down);
            }
            return outputList.ToArray();
        }
    }
}
