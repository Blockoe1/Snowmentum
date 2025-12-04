/*****************************************************************************
// File Name : GamePauser.cs
// Author :
// Creation Date : 
// Last Modified : 
//
// Brief Description : 
*****************************************************************************/
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Snowmentum
{
    public class SubmitInput : MonoBehaviour
    {
        [SerializeField] private UnityEvent onSubmit;
        private InputAction submitAction;

        private void Awake()
        {
            submitAction = InputSystem.actions.FindAction("Submit");
            submitAction.performed += SubmitAction_performed;
        }

        private void OnDestroy()
        {
            submitAction.performed -= SubmitAction_performed;
        }

        private void SubmitAction_performed(InputAction.CallbackContext obj)
        {
            onSubmit?.Invoke();
            //TransitionManager.LoadScene("TransitionFootageScene", TransitionType.Snowy);
            //isPaused = !isPaused;
            //if (isPaused)
            //{
            //    Time.timeScale = 1.0f;
            //}
            //else
            //{
            //    Time.timeScale = 0;
            //}
        }
    }
}
