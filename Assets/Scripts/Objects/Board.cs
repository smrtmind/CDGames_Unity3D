using Scripts.Pooling;
using System;
using UnityEngine;

namespace Scripts.Objects
{
    public class Board : MonoBehaviour, IPoolable
    {
        [SerializeField, Min(1f)] float speed = 20f;

        public event Action<IPoolable> Destroyed;

        public GameObject GameObject => gameObject;

        private void Update()
        {
            transform.position += Vector3.back * Time.deltaTime * speed;

            if (transform.position.z < -20)
                Destroy(gameObject);
        }

        public void Release() => Destroyed?.Invoke(this);
    }
}
