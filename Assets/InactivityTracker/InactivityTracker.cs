using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public class InactivityTracker : MonoBehaviour
{
    private float Timer;
    public TMP_Text timerText; 
    public static InactivityTracker Instance { get; private set; }
    public GameObject ResetPanel;
    private int timerMax = 120; 
    private void Start()
    {
        if(Instance == null)
        {
            Instance = this; 
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void OnEnable()
    {
        InputSystem.onEvent += OnInputEvent;
    }
    private void OnDisable()
    {
        InputSystem.onEvent -= OnInputEvent;
    }

    private void OnInputEvent(InputEventPtr eventPtr, InputDevice device)
    {
        // Reset the timer whenever an input event is detected
        Timer = 0f; 
        if (ResetPanel.activeSelf == true)
        {
            ResetPanel.SetActive(false);
        }
    }

    private void Update()
    {
        Timer += Time.unscaledDeltaTime; 
        if(Timer > timerMax)
        {

            if (ResetPanel.activeSelf == false)
            {
                ResetPanel.SetActive(true);

            }

            if (Timer > timerMax + 5)
            {
                Application.Quit(); 
                print("application quit called from inactivity");

            }
            int value = timerMax + 5 - Mathf.RoundToInt(Timer); 
            if(value < 0)
            {
                value = 0;
            }
            timerText.text = "Returning to main menu in " + value.ToString() + " seconds";

        }

    }
}
