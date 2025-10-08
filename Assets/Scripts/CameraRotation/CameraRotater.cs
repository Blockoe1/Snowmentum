/*****************************************************************************
// File Name : CameraRotater.cs
// Author : Jack Fisher
// Creation Date : October 6, 2025
// Last Modified : October 6, 2025
//
// Brief Description : Rotates the camera and background objects to help the illusion of the snowball speeding up.
*****************************************************************************/
using System.Collections;
using UnityEngine;

namespace Snowmentum
{
    public class CameraRotater : MonoBehaviour
    {
        [SerializeField] GameObject sceneCamera;
        [SerializeField] GameObject backgroundLayer1;
        [SerializeField] GameObject backgroundLayer2;
        [SerializeField] GameObject snowballGameObject;
        [SerializeField] Rigidbody2D snowballRigidbody;

        //used to track change in snowballSpeed
        public float previousSpeed;
        //usedd to wait until after the minigame is over to start rotating camera
        private float postMinigameDelay = 3f;

        private float deltaValue;

        //check for if camera should be rotating
        private bool isRotating;

        private float snowballSpeed;
        private float maxRotateAngle = 8f;
        void Start()
        {
            previousSpeed = SnowballSpeed.Value;
        }

        // Update is called once per frame
        void Update()
        {
            //check if snowball is active. basically checks for when the packing minigame ends and snowball becomes active
            if (snowballGameObject.activeSelf)
            {
                StartCoroutine(BeginRotation());
            }
            
            

            //Clamp the camera from rotating beyond a certain point
            Vector3 currentEulerAngles = transform.eulerAngles;
            float zAngle = currentEulerAngles.z;
            zAngle = Mathf.Clamp(zAngle, 0, maxRotateAngle);
            transform.eulerAngles = new Vector3(currentEulerAngles.x, currentEulerAngles.y, zAngle);

        }

        IEnumerator BeginRotation()
        { 
            yield return new WaitForSeconds(postMinigameDelay);
            isRotating = true;
        }

        private void FixedUpdate()
        {
            //tracks the change in SnowballSpeed
            //deltavalue is the change in snowballspeed
            if(previousSpeed != SnowballSpeed.Value)
            {
                float deltavalue = SnowballSpeed.Value - previousSpeed;

                Debug.Log($"Speed changed by {deltavalue}");
                deltavalue = deltaValue;
                RotateCamera();
                
            }
            previousSpeed = SnowballSpeed.Value;

        }

        void RotateCamera()
        {
            if (isRotating)
            {
                transform.Rotate(0, 0, deltaValue);
            }
        }
    }
}
