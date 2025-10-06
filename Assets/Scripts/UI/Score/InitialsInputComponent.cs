/*****************************************************************************
// File Name : ScoreInputComponent.cs
// Author : Brandon Koederitz
// Creation Date : 10/5/2025
// Last Modified : 10/5/2025
//
// Brief Description : Controls parts of players inputting data to save a high score.
*****************************************************************************/
using UnityEngine;

namespace Snowmentum
{
    public class InitialsInputComponent : MonoBehaviour
    {
        /// <summary>
        /// Called when this component is selected.
        /// </summary>
        public virtual void OnSelect() { }

        /// <summary>
        /// Called when this component is deselected.
        /// </summary>
        public virtual void OnDeselect() { }

        /// <summary>
        /// Called when the player inputs vertically on the trackball.
        /// </summary>
        /// <param name="inpupAmount">The amount of input that the ScoreInputMenu passes.</param>
        public virtual void OnVerticalInput(int inpupAmount) { }
    }
}
