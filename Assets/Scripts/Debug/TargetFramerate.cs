using UnityEngine;

namespace Snowmentum
{
    public class TargetFramerate : MonoBehaviour
    {
        [SerializeField] private int targetFramerate;
        [SerializeField] private Material scrollMaterial;

        private float speed = 1;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            Application.targetFrameRate = targetFramerate;
        }

        // Update is called once per frame
        void Update()
        {
            speed += 0.1f * Time.deltaTime;
            scrollMaterial.SetFloat("_Speed", speed);
        }
    }
}
