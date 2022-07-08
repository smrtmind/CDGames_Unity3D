using UnityEngine;

namespace Scripts.Objects
{
    public class BoardComponent : MonoBehaviour
    {
        [SerializeField] float speed = 8f;

        void Update()
        {
            transform.position += Vector3.back * Time.deltaTime * speed;
            if (transform.position.z < -20)
                Destroy(gameObject);
        }
    }
}
