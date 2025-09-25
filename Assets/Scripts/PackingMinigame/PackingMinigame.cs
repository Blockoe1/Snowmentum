/*****************************************************************************
// File Name : PackingMinigame.cs
// Author : Brandon Koederitz
// Creation Date : 9/24/2025
// Last Modified : 9/24/2025
//
// Brief Description : Controls the progression of the packing minigame.
*****************************************************************************/
using NUnit.Framework.Constraints;
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

        [SerializeField] private float packingTime;
        [SerializeField] private float throwDelay;
        [SerializeField] private float minThrowForce;
        [SerializeField] private UnityEvent<float, float> OnMinigameComplete;

        private MinigameState minigameState;
        private InputAction deltaAction;

        #region States
        private abstract class MinigameState
        {
            internal abstract IEnumerator OnTimeUpdate(PackingMinigame minigameController);
            internal abstract void OnMouseInput(PackingMinigame minigameController, Vector2 mouseDelta);
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

                minigameController.StartCoroutine(OnTimeUpdate(minigameController));
            }

            /// <summary>
            /// When the player inputs with the trackball, track it and progress the minigame
            /// </summary>
            /// <param name="mouseDelta"></param>
            internal override void OnMouseInput(PackingMinigame minigameController, Vector2 mouseDelta)
            {
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
            internal override IEnumerator OnTimeUpdate(PackingMinigame minigameController)
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

            private bool canThrow;

            internal ThrowState(PackingMinigame minigameController,
                float storedPackingQuality)
            {
                this.requiredThrowForce = minigameController.minThrowForce;
                this.storedPackingQuality = storedPackingQuality;
                this.throwDelay = minigameController.throwDelay;
                Debug.Log("Now Thow");

                minigameController.StartCoroutine(OnTimeUpdate(minigameController));
            }

            /// <summary>
            /// When the player inputs upwards, if it surpasses our required throw force, then the game begins.
            /// </summary>
            /// <param name="minigameController"></param>
            /// <param name="mouseDelta"></param>
            internal override void OnMouseInput(PackingMinigame minigameController, Vector2 mouseDelta)
            {
                if (canThrow && mouseDelta.y > requiredThrowForce)
                {
                    minigameController.CompleteMinigame(storedPackingQuality, mouseDelta.y);
                }
            }

            /// <summary>
            /// Delay detecting a throw for a bit at the beginning of the throw state.
            /// </summary>
            /// <param name="minigameController"></param>
            /// <param name="deltaTime"></param>
            internal override IEnumerator OnTimeUpdate(PackingMinigame minigameController)
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

            Application.targetFrameRate = 30;
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
            minigameState.OnMouseInput(this, obj.ReadValue<Vector2>());
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
        private void CompleteMinigame(float packinQuality, float throwStrength)
        {
            OnMinigameComplete?.Invoke(packinQuality, throwStrength);
            Debug.Log($"Packing Quality: {packinQuality}.  Throw Strength: {throwStrength}");

            // The minigame is no longer relevant, so destroy it.
            Destroy(gameObject);
        }
    }
}
