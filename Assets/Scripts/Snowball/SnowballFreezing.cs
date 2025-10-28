/*****************************************************************************
// File Name : SnowballFreezing.cs
// Author : Brandon Koederitz
// Creation Date : 9/23/2025
// Last Modified : 9/23/2025
//
// Brief Description : Allows the snowball to freeze over for a limited time, making it invincible.
*****************************************************************************/
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Snowmentum
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class SnowballFreezing : MonoBehaviour
    {
        #region CONSTS
        private const string WATER_TAG = "Water";
        private const float FREEZE_THRESHOLD = 1;
        #endregion

        [SerializeField, Tooltip("The time the snowball needs to spend in water to freeze over.")] 
        private float freezeTime;
        [SerializeField, Tooltip("The base amount of time that the snowball will stay frozen without moving " +
            "through water.")] 
        private float frozenTime;
        [SerializeField, Range(0, FREEZE_THRESHOLD)] private float warningThreshold = 0.25f;
        [SerializeField] private float flashDelay = 0.1f;
        [Header("Events")]
        [SerializeField] private UnityEvent OnFreezeEvent;
        [SerializeField] private UnityEvent OnThawEvent;
        [SerializeField] private UnityEvent<bool> OnToggleVisualsEvent;
        [SerializeField] private UnityEvent OnEnterWaterEvent;
        [SerializeField] private UnityEvent OnExitWaterEvent;

        private static float freezeAmount;
        public static event Action<float> OnFreezeAmountChanged;

        private static bool isFrozen;
        private byte waterNumber;
        private bool showingVisuals;

        #region Properties
        public static bool IsFrozen => isFrozen;
        private bool IsFrozen_Internal
        {
            get { return isFrozen; }
            set
            {
                // Only allow changes if we are moving to a new state.
                if (isFrozen != value)
                {
                    isFrozen = value;
                    if (isFrozen)
                    {
                        OnFreezeEvent?.Invoke();
                        // Make sure to enable/disable visuals when isFrozen changes.
                        ShowingVisuals = true;
                    }
                    else
                    {
                        OnThawEvent?.Invoke();
                        ShowingVisuals = false;
                    }
                }
            }
        }

        private bool ShowingVisuals
        {
            get { return showingVisuals; }
            set
            {
                showingVisuals = value;
                OnToggleVisualsEvent?.Invoke(value);
            }
        }
        public static float FreezeAmount
        {
            get { return freezeAmount; }
            private set 
            { 
                freezeAmount = Mathf.Clamp(value, 0, FREEZE_THRESHOLD); 
                OnFreezeAmountChanged?.Invoke(freezeAmount);
            }
        }
        #endregion

        #region Nested
        private delegate float RateGetter();
        private delegate bool StateGetter();
        #endregion

        #region Getters
        // Need to make getters instead of properties so that I can pass them as delegates.
        private float GetFreezeRate() { return FREEZE_THRESHOLD / freezeTime; }
        private float GetThawRate() { return -FREEZE_THRESHOLD / frozenTime; }
        #endregion

        /// <summary>
        /// Reset values when the snowball is destroyed.
        /// </summary>
        private void OnDestroy()
        {
            // Modify the values directly instead of through the property since running events on destroy causes
            // erorrs
            freezeAmount = 0;
        }

        #region Freeze Accumulation
        /// <summary>
        /// When the snowball enters a trigger marked as "water", they will begin to accumulate freezeAmount;
        /// </summary>
        /// <param name="collision"></param>
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag(WATER_TAG))
            {
                //isInWater = true;
                waterNumber++;
                // If this is the first puddle we've entered, need to start code that triggers on entering water.
                if (waterNumber == 1)
                {
                    OnEnterWaterEvent?.Invoke();
                    // Continually increases the freezeAmount by freezeRate while the snowball is in water.
                    StartCoroutine(FreezeChangeRoutine());
                }
            }
        }

        /// <summary>
        /// Stops freezeAmount gain when the snowball exits water.
        /// </summary>
        /// <param name="collision"></param>
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag(WATER_TAG))
            {
                waterNumber--;
                if (waterNumber <= 0)
                {
                    OnExitWaterEvent?.Invoke();
                }
            }
        }

        /// <summary>
        /// Causes the snowball to gain freezeAmount while it's in water
        /// </summary>
        /// <returns></returns>
        private IEnumerator FreezeChangeRoutine()
        {
            while (waterNumber > 0)
            {
                FreezeAmount += GetFreezeRate() * Time.deltaTime;
                UpdateFreezeStatus(FreezeAmount);
                yield return null;
            }
        }
        #endregion

        /// <summary>
        /// Updates the current state of the snowball between frozen and thawed.
        /// </summary>
        /// <param name="freezeAmount">The current freezeAmount of the snowball.</param>
        private void UpdateFreezeStatus(float freezeAmount)
        {
            // Switches the snowball to the frozen state.
            if (!IsFrozen_Internal && freezeAmount >= FREEZE_THRESHOLD)
            {
                //Debug.Log("Frozen");
                IsFrozen_Internal = true;
                // Need to decrement our freezeAmount while the snowball is frozen.
                StartCoroutine(WhileFrozenRoutine());
            }
            // Switches the snowball back to it's normal state.
            else if (IsFrozen_Internal && freezeAmount <= 0)
            {
                IsFrozen_Internal = false;
            }
        }

        /// <summary>
        /// Causes the snowball to gain freezeAmount while it's in water
        /// </summary>
        /// <returns></returns>
        private IEnumerator WhileFrozenRoutine()
        {
            float flashTimer = 0;
            while (isFrozen)
            {
                FreezeAmount += GetThawRate() * Time.deltaTime;
                UpdateFreezeStatus(FreezeAmount);

                // If our freeze amount is too low, then we do timed blinks of the frozen snowball sprite.
                if (FreezeAmount < warningThreshold)
                {
                    flashTimer -= Time.deltaTime;
                    if (flashTimer < 0)
                    {
                        ShowingVisuals = !ShowingVisuals;
                        flashTimer = flashDelay;
                    }
                }
                // If we go above our threshold, we need to make sure we reset visuals.
                else if (!ShowingVisuals)
                {
                    ShowingVisuals = true;
                }

                yield return null;
            }
        }

        ///// <summary>
        ///// Continually causes the snowball to thaw while it is frozen.
        ///// </summary>
        ///// <returns></returns>
        //private IEnumerator FreezeRoutine()
        //{
        //    while (IsFrozen)
        //    {
        //        FreezeAmount -= thawRate * Time.deltaTime;
        //        UpdateFreezeStatus(FreezeAmount);
        //        yield return null;
        //    }
        //}

        #region Debug
        //private void OnGUI()
        //{
        //    GUI.TextArea(new Rect(10, 220, 100, 100), "Freeze Amount: " + FreezeAmount.ToString());
        //}
        #endregion
    }
}
