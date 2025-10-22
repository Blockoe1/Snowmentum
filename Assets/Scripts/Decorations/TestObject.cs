using UnityEngine;

namespace Snowmentum
{
    public class NewMonoBehaviourScript : MonoBehaviour
    {
        float speed = 5;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            speed = speed * Random.Range(0.9f, 1.6f); //added random speed modifier to make some stuff faster or slower than others -Zaden
        }

        // Update is called once per frame
        void Update()
        {
            transform.Translate(Vector2.left * speed * Time.deltaTime);
        }
    }
}
