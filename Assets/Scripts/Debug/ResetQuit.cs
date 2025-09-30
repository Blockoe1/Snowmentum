using Snowmentum.Score;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Snowmentum
{
    public class ResetQuit : MonoBehaviour
    {
        private InputAction restartAction;
        private InputAction quitAction;

        private void Awake()
        {
            restartAction = InputSystem.actions.FindAction("Restart");
            quitAction = InputSystem.actions.FindAction("Quit");

            restartAction.performed += Restart;
            quitAction.performed += Quit;
        }

        private void OnDestroy()
        {
            restartAction.performed -= Restart;
            quitAction.performed -= Quit;
        }

        private void Restart(InputAction.CallbackContext obj)
        {
            // Reset score on restart temporarily until we get a proper scene movement system in.
            ScoreStatic.Score = 0;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        private void Quit(InputAction.CallbackContext obj)
        {
            Application.Quit();
        }
    }
}
