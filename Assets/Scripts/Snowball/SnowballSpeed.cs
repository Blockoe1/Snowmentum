/*****************************************************************************
// File Name : SnowballSpeed.cs
// Author : Brandon Koederitz
// Creation Date : 9/19/2025
// Last Modified : 9/19/2025
//
// Brief Description : From a gameplay standpoint, it controls the speed that the snowball moves down the hill.
// In actuality, it controls the speed that obstacles move towards the snowball.
*****************************************************************************/
using UnityEngine;

namespace Snowmentum
{
    [CreateAssetMenu(fileName = "SnowballSpeed", menuName = "ScriptableObjects/SnowballSpeed")]
    public class SnowballSpeed : ScriptableValue
    {
        [SerializeField, Tooltip("The minimum target speed that the snowball can be at.")]
        private float minSpeed;

        public override float Value
        {
            set
            {
                base.Value = Mathf.Max(value, minSpeed);
            }
        }

        public override void OnCollision(float obstacleSize, float snowballSize)
        {
            base.OnCollision(obstacleSize, snowballSize);
        }
    }
}
