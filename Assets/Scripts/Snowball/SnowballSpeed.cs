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
using static UnityEditor.Rendering.InspectorCurveEditor;

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

        /// <summary>
        /// The formula
        /// -(curveSteepness ^ (-snowballSize + obstacleSize)) * curveScale,
        /// which is used for determining changes to speed.
        /// </summary>
        /// <param name="obstacleSize"></param>
        /// <param name="snowballSize"></param>
        /// <param name="curveScale"></param>
        /// <param name="curveSteepness"></param>
        /// <returns></returns>
        private static float SpeedCollisionCurve(float obstacleSize, float snowballSize, float curveScale, 
            float curveSteepness)
        {
            return -Mathf.Pow(curveSteepness, (-snowballSize + obstacleSize)) * curveScale;
        }
    }
}
