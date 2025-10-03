/*****************************************************************************
// File Name : ButtonHovering.cs
// Author : Brandon Koederitz
// Creation Date : 10/2/2025
// Last Modified : 10/2/2025
//
// Brief Description : Allows for auto-seleecting a button when hovering over it for a certain amount of time.
*****************************************************************************/
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Snowmentum
{
    public class ButtonHovering : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public void OnPointerEnter(PointerEventData eventData)
        {
            Debug.Log("Pointer Enter");
            Cursor.visible = false;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Debug.Log("Pointer Exit");
            Cursor.visible = true;
        }

        public void ClickButton()
        {

        }
    }
}
