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
        private float rotationSpeed = 1.3f;
        private float maxZRotation = 8f;
        private float minZRotation;
        private Quaternion targetRotation;

        void Start()
        {
            previousSpeed = SnowballSpeed.Value;
            targetRotation = transform.rotation;
        }

        // Update is called once per frame
        void Update()
        {
            //If there is a better way to check when the minigame ends that doesn't happen every frame forever that would be good but it works for now
            //check if snowball is active. basically checks for when the packing minigame ends and snowball becomes active
            if (snowballGameObject.activeInHierarchy)
            {
                StartCoroutine(BeginRotation());
            }
            
            

            //Clamp the camera from rotating beyond a certain point
            Vector3 currentEulerAngles = transform.eulerAngles;
            float zAngle = currentEulerAngles.z;
            zAngle = Mathf.Clamp(zAngle, 0, maxZRotation);
            transform.eulerAngles = new Vector3(currentEulerAngles.x, currentEulerAngles.y, zAngle);

        }

        IEnumerator BeginRotation()
        {
            
            yield return new WaitForSeconds(postMinigameDelay);
            isRotating = true;
        }

        //I might end up not using this lol. 
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
                // Calculate the target Quaternion based on Euler angles
                targetRotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, maxZRotation);

                // Smoothly interpolate towards the target rotation
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            }
        }
    }
}
