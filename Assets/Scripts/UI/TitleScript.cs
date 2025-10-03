/*****************************************************************************
// File Name : TimerDisplayer.cs
// Author : Jack Fisher
// Creation Date : October 1, 2025
// Last Modified : October 1, 2025
//
// Brief Description : This script has the function the start game button uses to load the gameplay scene
*****************************************************************************/
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
    }
}
