/*****************************************************************************
// File Name : CameraRotater.cs
// Author : Jack Fisher
// Creation Date : October 6, 2025
// Last Modified : October 6, 2025
//
// Brief Description : Rotates the camera and background objects to help the illusion of the snowball speeding up.
*****************************************************************************/
using UnityEngine;

namespace Snowmentum
{
    public class CameraRotater : MonoBehaviour
    {
        [SerializeField] GameObject sceneCamera;
        [SerializeField] GameObject backgroundLayer1;
        [SerializeField] GameObject backgroundLayer2;
        [SerializeField] Rigidbody2D snowballRigidbody;

        //used to track change in snowballSpeed
        public float previousSpeed;

        private float snowballSpeed;
        private float maxRotateAngle = 8f;
        void Start()
        {
            previousSpeed = SnowballSpeed.Value;
        }

        // Update is called once per frame
        void Update()
        {
            
            
            

            //Clamp the camera from rotating beyond a certain point
            Vector3 currentEulerAngles = transform.eulerAngles;
            float zAngle = currentEulerAngles.z;
            zAngle = Mathf.Clamp(zAngle, 0, maxRotateAngle);
            transform.eulerAngles = new Vector3(currentEulerAngles.x, currentEulerAngles.y, zAngle);

        }

        private void FixedUpdate()
        {
            //tracks the change in SnowballSpeed
            if(previousSpeed != SnowballSpeed.Value)
            {
                float deltavalue = SnowballSpeed.Value - previousSpeed;

                Debug.Log($"Speed changed by {deltavalue}");

                transform.Rotate(0, 0, deltavalue);
            }
            previousSpeed = SnowballSpeed.Value;

        }
    }
}
