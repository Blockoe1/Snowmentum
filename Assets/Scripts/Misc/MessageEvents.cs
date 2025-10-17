/*****************************************************************************
// File Name : StartEvent.cs
// Author : Brandon Koederitz
// Creation Date : 10/17/2025
// Last Modified : 10/17/2025
//
// Brief Description : Script for exposing unity messages 
*****************************************************************************/
using UnityEngine;
using UnityEngine.Events;

namespace Snowmentum
{
    public class MessageEvents : MonoBehaviour
    {
        [SerializeField] private UnityEvent OnStartEvent;
        [SerializeField] private UnityEvent OnDestroyEvent;

        private void Start()
        {
            OnStartEvent?.Invoke();
        }
        private void OnDestroy()
        {
            OnDestroyEvent?.Invoke();
        }
    }
}
