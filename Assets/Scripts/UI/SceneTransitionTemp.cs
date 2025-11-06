/*****************************************************************************
// File Name : SceneTransitionTemp.cs
// Author :
// Creation Date : 
// Last Modified : 
//
// Brief Description : 
*****************************************************************************/
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Snowmentum
{
    public class SceneTransitionTemp : MonoBehaviour
    {
        public void TransitionToScene(string targetScene)
        {
            TransitionManager.LoadScene(targetScene);
        }
    }
}
