/*****************************************************************************
// File Name : HoldButtonTimer.cs
// Author : Jack Fisher
// Creation Date : October 3, 2025
// Last Modified : October 3, 2025
//
// Brief Description : Script that activates the on click event of buttons after the mouse is held over the buttons for a set amount of time. 
Useful since the trackball cannot click on buttons. 
*****************************************************************************/
using Snowmentum;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HoldButtonTimer : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler 
{
    [SerializeField] private float requiredHoldTime = 1f; // The time in seconds the mouse must be on the button, currently 1 second
    [SerializeField] private Button usedButton; //We can reuse this script, placing it on each button, so I gave it a vague name

    private float currentHoldTime = 0f;
    private bool isPointerOver = false;

    /// <summary>
    /// Subscribe the CleanUp function to the button automatically
    /// </summary>
    private void Awake()
    {
        usedButton.onClick.AddListener(CleanUp);
    }
    private void OnDestroy()
    {
        usedButton.onClick.RemoveListener(CleanUp);
    }

    void Update()
    {
        if (isPointerOver)
        {
            currentHoldTime += Time.unscaledDeltaTime;
            HoldProgress.UpdateProgressFill(currentHoldTime, requiredHoldTime);
            if (currentHoldTime >= requiredHoldTime)
            {
                //activates the on click event of the button, which will be different depending on what each button does
                    usedButton.onClick.Invoke();
            }
        }
    }

    //checks for when mouse moves over and exits button
    public void OnPointerEnter(PointerEventData eventData)
    {
        isPointerOver = true;
        HoldProgress.SetProgressVisibility(true);
        currentHoldTime = 0f;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isPointerOver = false;
        HoldProgress.SetProgressVisibility(false);
        currentHoldTime = 0f; 
    }

    /// <summary>
    /// Separate function called by the button's event so that it gets called when the button is pressed normally as well.
    /// </summary>
    public void CleanUp()
    {
        // Remove the indicator.
        OnPointerExit(null);

        // Reset the timer to prevent repeated activation and things breaking
        currentHoldTime = 0f;
        isPointerOver = false;
    }
}