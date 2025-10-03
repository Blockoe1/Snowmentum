/*****************************************************************************
// File Name : TitleScript.cs
// Author : Jack Fisher
// Creation Date : October 1, 2025
// Last Modified : October 3, 2025
//
// Brief Description : This script has the function the start game button uses to load the gameplay scene
*****************************************************************************/
using Snowmentum.Score;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Snowmentum
{
    public class TitleScript : MonoBehaviour
    {
        
        public void PlayGame()
        {
            //We can make the title scene 0 in the build and the gameplay scene 1
            SceneManager.LoadScene(1);
        }

        /// <summary>
        /// Quits the game.
        /// </summary>
        public void Quit()
        {
            Application.Quit();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
    }
}
