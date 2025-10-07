/*****************************************************************************
// File Name : ThermometerToggle.cs
// Author :
// Creation Date : 
// Last Modified : 
//
// Brief Description : 
*****************************************************************************/
using UnityEngine;
using UnityEngine.InputSystem;

namespace Snowmentum
{
    public class ThermometerToggle : MonoBehaviour
    {
        [SerializeField] private GameObject meter1;
        [SerializeField] private GameObject meter2;
        private InputAction meterSwitchAction;

        private void Awake()
        {
            meterSwitchAction = InputSystem.actions.FindAction("Quit");
            meterSwitchAction.performed += MeterSwitchAction_performed;
        }

        private void OnDestroy()
        {
            meterSwitchAction.performed -= MeterSwitchAction_performed;
        }

        private void MeterSwitchAction_performed(InputAction.CallbackContext obj)
        {
            Debug.Log("Pressed");
            meter1.SetActive(!meter1.activeSelf);
            meter2.SetActive(!meter2.activeSelf);
        }
    }
}
