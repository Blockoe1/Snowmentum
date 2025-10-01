/*****************************************************************************
// File Name : CameraZoom.cs
// Author : Brandon Koederitz
// Creation Date : 9/29/2025
// Last Modified : 9/29/2025
//
// Brief Description : Zooms the camera in and out based on the current size bracket of the snowball.
*****************************************************************************/
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Snowmentum.Size
{
    [RequireComponent(typeof(PixelPerfectCamera))]
    public class CameraZoom : MonoBehaviour
    {
        private int referencePPU;
        private float refSize;

        #region Component References
        [SerializeReference] private PixelPerfectCamera cam;
        /// <summary>
        /// Get components automatically on reset
        /// </summary>
        [ContextMenu("Get Component References")]
        private void Reset()
        {
            cam = GetComponent<PixelPerfectCamera>();
        }
        #endregion

        /// <summary>
        /// Sets up this component.
        /// </summary>
        private void Awake()
        {
            // Store our base camera size;
            //refSize = cam.orthographicSize;
            referencePPU = cam.assetsPPU;
            // Subscribe so we can call OnBracketChanged when our snowball's size bracket changes.
            SizeBracket.OnBracketChanged += OnBracketChanged;
        }
        /// <summary>
        /// Unsubscribe events.
        /// </summary>
        private void OnDestroy()
        {
            SizeBracket.OnBracketChanged -= OnBracketChanged;
        }

        /// <summary>
        /// Updates the size of the camera based on our snowball's current bracket so that is zooms out.
        /// </summary>
        /// <param name="sizeBracket">The current size bracket of the snowball.</param>
        private void OnBracketChanged(int sizeBracket)
        {
            Debug.Log("Bracket Changed");
            //cam.orthographicSize = refSize * sizeBracket;
            cam.assetsPPU = referencePPU / sizeBracket;
        }
    }
}
