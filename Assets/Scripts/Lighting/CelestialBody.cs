/*****************************************************************************
// File Name : CelestialBody.cs
// Author : Brandon Koederitz
// Creation Date : 11/3/2025
// Last Modified : 11/3/2025
//
// Brief Description : Controls the movement of a celestial body around the center of the screen based on the time.
*****************************************************************************/
using System.Linq;
using Unity.VisualScripting;
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
            if (keyframes.Length > 1)
            {
                keyframes = keyframes.OrderBy(item => item.time).ToArray();
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
                void IncrementKeyframe()
                {
                    currentKeyframe = targetKeyframe;
                    targetKeyframe++;
                    CollectionHelpers.LoopIndex(keyframes.Length, ref targetKeyframe);
                }
                void DecrementKeyframe()
                {
                    targetKeyframe = currentKeyframe;
                    currentKeyframe--;
                    CollectionHelpers.LoopIndex(keyframes.Length, ref currentKeyframe);
                }

                // Update next and current keyframes.
                if (targetKeyframe == 0)
                {
                    // Need to do extra checks when looping to ensure things move properly.
                    if (normalizedTime >= keyframes[targetKeyframe].time && 
                        normalizedTime < keyframes[targetKeyframe + 1].time)
                    {
                        IncrementKeyframe();
                    }
                    else if (normalizedTime < keyframes[currentKeyframe].time && 
                        normalizedTime > keyframes[currentKeyframe - 1].time)
                    {
                        DecrementKeyframe();
                    }
                }
                else
                {
                    if (normalizedTime >= keyframes[targetKeyframe].time)
                    {
                        IncrementKeyframe();
                    }
                    else if (normalizedTime < keyframes[currentKeyframe].time)
                    {
                        DecrementKeyframe();   
                    }
                }

                    Debug.Log(name + ": " + currentKeyframe + " " + targetKeyframe);
                float normalizedKeyframeDistance = 
                    GetNomalizedKeyDistance(keyframes[currentKeyframe], keyframes[targetKeyframe], normalizedTime, 
                    targetKeyframe == 0);

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
        /// Calculates the normalized progress between two keyframes given a certain normalized time.
        /// </summary>
        /// <param name="currentIndex"></param>
        /// <param name="targetIndex"></param>
        /// <param name="normalizedTime"></param>
        /// <returns></returns>
        private static float GetNomalizedKeyDistance(TimeKeyframe currentIndex, TimeKeyframe targetIndex, 
            float normalizedTime, bool isLooped)
        {
            // Calculate the distance between them.
            float toTarget = normalizedTime - currentIndex.time;
            float keyframeDistance = targetIndex.time - currentIndex.time;
            // If this is a looped keyframe, we need to increment keyframeDistance by 1.
            if (isLooped)
            {
                keyframeDistance += 1;
            }
            // Calculate the normalized progress between the two keyframes.
            float normalizedKeyframeDistance = toTarget / keyframeDistance;
            return normalizedKeyframeDistance;
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
