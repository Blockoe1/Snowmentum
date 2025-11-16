/*****************************************************************************
// File Name : CelestialBody.cs
// Author : Brandon Koederitz
// Creation Date : 11/3/2025
// Last Modified : 11/3/2025
//
// Brief Description : Controls the movement of a celestial body around the center of the screen based on the time.
*****************************************************************************/
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Snowmentum
{
    public class CelestialBody : MonoBehaviour
    {
        [SerializeField] private float angleOffset;
        [SerializeField] private float radialOffset;
        [SerializeField] private Light2D bodyLight;
        [SerializeField] private TimeKeyframe[] keyframes;

        private int currentKeyframe;
        private int targetKeyframe;
        private bool hasKeyframes;

        #region Nested
        // Stores a "keyframe" in time that celestial body lerps towards based on the current time.
        [System.Serializable]
        private struct TimeKeyframe
        {
            [SerializeField, Range(0f, 1f)] internal float time;
            [Header("Light Settings")]
            [SerializeField] internal Color lightColor;
            [SerializeField] internal float lightIntensity;
            [SerializeField] internal float innerRadius;
            [SerializeField] internal float outerRadius;
        }
        #endregion

        /// <summary>
        /// Order the keyframes list based on their time.
        /// </summary>
        private void Awake()
        {
            keyframes.OrderBy(item => item.time).ToArray();
            if (keyframes.Length > 1)
            {
                hasKeyframes = true;
                currentKeyframe = 0;
                targetKeyframe = 1;
            }
        }

        /// <summary>
        /// Updates the celestial body's position and light settings based on the current elapsed daylight time.
        /// </summary>
        /// <param name="normalizedTime"></param>
        public void TimeUpdate(float normalizedTime)
        {
            // Update position.
            float currentAngle = Mathf.Lerp(0, 360, normalizedTime) + angleOffset;
            transform.localPosition = PolarToCartesian(currentAngle, radialOffset);

            // Update light settings based on the current time keyframe.

            if (hasKeyframes)
            {
                // If this is the last keyframe, don't update until normalizedTime has looped.
                if (targetKeyframe != 0 || normalizedTime < keyframes[currentKeyframe].time)
                {
                    // Update next and current keyframes.
                    if (normalizedTime > keyframes[targetKeyframe].time)
                    {
                        currentKeyframe = targetKeyframe;
                        targetKeyframe++;
                        CollectionHelpers.LoopIndex(keyframes.Length, ref targetKeyframe);
                    }
                    else if (normalizedTime < keyframes[currentKeyframe].time)
                    {
                        targetKeyframe = currentKeyframe;
                        currentKeyframe--;
                        CollectionHelpers.LoopIndex(keyframes.Length, ref currentKeyframe);
                    }
                }

                Debug.Log(currentKeyframe + " " + targetKeyframe);
                // Calculate the distance between them.
                float toTarget = normalizedTime - keyframes[currentKeyframe].time;
                float keyframeDistance = keyframes[targetKeyframe].time - keyframes[currentKeyframe].time;
                // Calculate the normalized progress between the two keyframes.
                float normalizedKeyframeDistance = toTarget / keyframeDistance;

                // lerp between the two keyframes.
                SetTimeData(LerpKeyframe(keyframes[currentKeyframe], keyframes[targetKeyframe], 
                    normalizedKeyframeDistance));
            }
        }

        /// <summary>
        /// Creates a keyframe that is lerped between two target keyframes.
        /// </summary>
        /// <param name="current"></param>
        /// <param name="target"></param>
        /// <param name="t"></param>
        private static TimeKeyframe LerpKeyframe(TimeKeyframe current, TimeKeyframe target, float t)
        {
            TimeKeyframe frame = new TimeKeyframe();
            frame.time = Mathf.Lerp(current.time, target.time, t);
            frame.lightIntensity = Mathf.Lerp(current.lightIntensity, target.lightIntensity, t);
            frame.innerRadius = Mathf.Lerp(current.innerRadius, target.innerRadius, t);
            frame.outerRadius = Mathf.Lerp(current.outerRadius, target.outerRadius, t);
            frame.lightColor = Color.Lerp(current.lightColor, target.lightColor, t);
            return frame;
        }

        /// <summary>
        /// Sets the data of this celestial body's light based on a TimeKeyframe.
        /// </summary>
        /// <param name="keyframe"></param>
        private void SetTimeData(TimeKeyframe keyframe)
        {
            if (bodyLight != null)
            {
                bodyLight.color = keyframe.lightColor;
                bodyLight.intensity = keyframe.lightIntensity;
                bodyLight.pointLightInnerRadius = keyframe.innerRadius;
                bodyLight.pointLightOuterRadius = keyframe.outerRadius;
            }
        }

        /// <summary>
        /// Calculates the position of the celestial body based on it's angle offset.
        /// </summary>
        /// <param name="angle">The angle that the celestial body is at.</param>
        /// <param name="radius">The radis of the polar circle the celestial body lies on.</param>
        /// <returns></returns>
        private static Vector2 PolarToCartesian(float angle, float radius)
        {
            return MathHelpers.DegAngleToUnitVector(angle) * radius;
        }
    }
}
