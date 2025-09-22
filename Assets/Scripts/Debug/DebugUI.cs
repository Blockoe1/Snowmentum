/*****************************************************************************
// File Name : DebugUI.cs
// Author :
// Creation Date : 
// Last Modified : 
//
// Brief Description : 
*****************************************************************************/
using UnityEngine;

namespace Snowmentum
{
    public class DebugUI : MonoBehaviour
    {
        private void OnGUI()
        {
            GUIStyle _guiStyle = new GUIStyle();
            _guiStyle.fontSize = 20;
            _guiStyle.fontStyle = FontStyle.Normal;
            _guiStyle.normal.textColor = Color.white;

            GUI.Box(new Rect(8f, 10f, 200f, 200f), "");
            GUI.Label(new Rect(12f, 10f, 10f, 75f), $"HP: ", _guiStyle);
            GUI.Label(new Rect(12f, 40f, 10f, 75f), $"Score: ", _guiStyle);
            GUI.Label(new Rect(12f, 70f, 10f, 75f), $"Highscore: ", _guiStyle);
            //GUI.Label(new Rect(10f, 210f, 10f, 10f), $"Gameover: {this.gameOver}", _guiStyle);

        }
    }
}
