/*****************************************************************************
// File Name : PackingMinigame.cs
// Author : Brandon Koederitz
// Creation Date : 9/24/2025
// Last Modified : 9/24/2025
//
// Brief Description : Controls the progression of the packing minigame.
*****************************************************************************/
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Snowmentum
{
    public class PackingMinigame : MonoBehaviour
    {
        #region CONST
        private const string DELTA_ACTION_NAME = "MouseMovement";
        #endregion

        [SerializeField] private UnityEvent OnMinigameComplete;

        [Header("Packing")]
        [SerializeField] private float packingTime;
        [SerializeField, Tooltip("Packing quality is by default a very large value (Close to 1 million) whith good " +
            "input.  Divide it by this value to calculate the actual addition to size of the snowball.")] 
        private float packingQualityScaler = 1000000;
        [SerializeField] private UnityEvent<float> OnMinigamePack;
        [Header("Throwing")]
        [SerializeField] private float throwDelay;
        [SerializeField] private float minThrowForce;
        [SerializeField, Range(0f, 1f)] private float throwSampleTime;
        [SerializeField, Tooltip("Throw Strength is usually a very high value (close to 10 thousand).  " +
            "Divide it by this value to calculate the actual increase in snowball speed.")]
        private float throwStrengthScaler = 10000;
        [SerializeField, Tooltip("Multiplied by the scaled throwStrength to get the starting speed of the snowball")] 
        private float startingSpeedScaler;
        [SerializeField] private UnityEvent<float> OnMinigameThrow;
        [SerializeField] private UnityEvent<float> OnMultipliedMinigameThrow;

        private MinigameState minigameState;
        private InputAction deltaAction;

        #region States
        private abstract class MinigameState
        {
            internal abstract IEnumerator Timer(PackingMinigame minigameController);
            internal abstract void MouseUpdate(PackingMinigame minigameController, Vector2 mouseDelta);
        }

        private class PackingState : MinigameState
        {
            private Vector2 lastDelta;
            private float packingQuality;
            private float timer;

            private bool hasStarted;

            internal PackingState(PackingMinigame minigameController)
            {
                this.timer = minigameController.packingTime;
                Debug.Log("Now Packing");

                minigameController.StartCoroutine(Timer(minigameController));
            }

            /// <summary>
            /// When the player inputs with the trackball, track it and progress the minigame
            /// </summary>
            /// <param name="mouseDelta"></param>
            internal override void MouseUpdate(PackingMinigame minigameController, Vector2 mouseDelta)
            {
                // Skip any updates where our mouse didnt move.
                if (mouseDelta == Vector2.zero) { return; }
                // Increase the quality of the snowball based on how much the mouse delta changed since the
                // last update.
                packingQuality += Vector2.Distance(lastDelta, mouseDelta);

                lastDelta = mouseDelta;

                // Only allow the timer to update once the player has done at least one input.
                hasStarted |= true;

            }

            /// <summary>
            /// Limits the player's time during the packing stage of the minigame once the player starts inputting
            /// with the mouse.
            /// </summary>
            /// <param name="deltaTime"></param>
            internal override IEnumerator Timer(PackingMinigame minigameController)
            {
                // Wait until the player has started inputting to actually track anything.
                while (!hasStarted)
                {
                    yield return null;
                }

                while (timer > 0)
                {
                    timer -= Time.deltaTime;
                    yield return null;
                }
                // Move to the the throw state once our time is up.
                minigameController.minigameState =
                            new ThrowState(minigameController, packingQuality);
            }
        }

        private class ThrowState : MinigameState
        {
            private readonly float requiredThrowForce;
            private readonly float storedPackingQuality;
            private readonly float throwDelay;
            private float sampleTime;

            private float maxDelta;

            private bool canThrow;
            private bool isSampling;

            internal ThrowState(PackingMinigame minigameController,
                float storedPackingQuality)
            {
                this.requiredThrowForce = minigameController.minThrowForce;
                this.storedPackingQuality = storedPackingQuality;
                this.throwDelay = minigameController.throwDelay;
                this.sampleTime = minigameController.throwSampleTime;
                Debug.Log("Now Thow");

                minigameController.StartCoroutine(Timer(minigameController));
            }

            /// <summary>
            /// When the player inputs upwards, if it surpasses our required throw force, then the game begins.
            /// </summary>
            /// <param name="minigameController"></param>
            /// <param name="mouseDelta"></param>
            internal override void MouseUpdate(PackingMinigame minigameController, Vector2 mouseDelta)
            {
                // First, check if the trackball is moving in the desired direction.
                if (canThrow && mouseDelta.x > requiredThrowForce)
                {
                    isSampling = true;
                }

                // Then, sample mouse delta values for a small amount of time to ensure we capture the fastest the 
                // player moved the ball.
                if (isSampling)
                {
                    // Track only the largest delta value.
                    if (mouseDelta.x > maxDelta)
                    {
                        maxDelta = mouseDelta.x;
                    }

                    sampleTime -= Time.deltaTime;

                    if (sampleTime <= 0)
                    {
                        minigameController.CompleteMinigame(storedPackingQuality, maxDelta);
                    }
                }
            }

            /// <summary>
            /// Delay detecting a throw for a bit at the beginning of the throw state.
            /// </summary>
            /// <param name="minigameController"></param>
            /// <param name="deltaTime"></param>
            internal override IEnumerator Timer(PackingMinigame minigameController)
            {
                canThrow = false;
                yield return new WaitForSeconds(throwDelay);
                canThrow = true;
            }
        }
        #endregion

        /// <summary>
        /// When the game begins, we begin with the packing state
        /// </summary>
        private void Awake()
        {
            minigameState = new PackingState(this);
            //StartCoroutine(TimeUpdateRoutine());

            deltaAction = InputSystem.actions.FindAction(DELTA_ACTION_NAME);

            deltaAction.performed += DeltaAction_Performed;

            Application.targetFrameRate = 0;
        }

        /// <summary>
        /// Unsubscribe input events
        /// </summary>
        private void OnDestroy()
        {
            deltaAction.performed -= DeltaAction_Performed;
        }

        /// <summary>
        /// Notifies our current state when the player inputs with the trackball.
        /// </summary>
        /// <param name="obj"></param>
        private void DeltaAction_Performed(InputAction.CallbackContext obj)
        {
            //minigameState.OnMouseInput(this, obj.ReadValue<Vector2>());
        }

        /// <summary>
        /// Track the mouse delta in update and pass the frame independent value to our current state.
        /// </summary>
        private void Update()
        {
            minigameState.MouseUpdate(this, deltaAction.ReadValue<Vector2>() / Time.deltaTime);
        }

        /// <summary>
        /// Continually calls time update on the current minigame state.
        /// </summary>
        /// <returns></returns>
        //private IEnumerator TimeUpdateRoutine()
        //{
        //    while (true)
        //    {
        //        minigameState.OnTimeUpdate(this, Time.deltaTime);
        //        yield return null;
        //    }
        //}

        /// <summary>
        /// Called by a state when the minigame is completed.
        /// </summary>
        /// <param name="packinQuality">
        /// The packing quality based on how much the player moved the trackball during packing
        /// </param>
        /// <param name="throwStrength">
        /// Corresponds to the force the player rolled the trackball with during the throw minigame.
        /// </param>
        private void CompleteMinigame(float packingQuality, float throwStrength)
        {
            // Scale down packingQuality and ThrowStrength to useful values.
            packingQuality /= packingQualityScaler;
            throwStrength /= throwStrengthScaler;


            OnMinigameComplete?.Invoke();
            OnMinigamePack?.Invoke(packingQuality);
            OnMinigameThrow?.Invoke(throwStrength);
            Debug.Log($"Packing Quality: {packingQuality}.  Throw Strength: {throwStrength}");

            // Use this event to start the snowball moving at a speed that scales based on the throw strength
            OnMultipliedMinigameThrow?.Invoke(throwStrength * startingSpeedScaler);

            // The minigame is no longer relevant, so destroy it.
            Destroy(gameObject);
        }
    }
}
