using Scripts.Managers;
using System;
using UnityEngine;
using Zenject;
using IPoolable = Scripts.Pooling.IPoolable;

namespace Scripts.Objects
{
    public class Board : MonoBehaviour, IPoolable
    {
        [SerializeField, Min(1f)] float speed = 20f;
        [SerializeField] private float _distanceToReleaseByZ = -10f;

        public event Action<IPoolable> Destroyed;

        public GameObject GameObject => gameObject;

        private GameManager _gameManager;

        [Inject]
        private void Construct(GameManager gameManager)
        {
            _gameManager = gameManager;
        }

        private void Update()
        {
            if (_gameManager.State != GameState.Gameplay) return;

            if (transform.position.z < _distanceToReleaseByZ)
                Release();

            Move();
        }

        private void Move() => transform.position += Vector3.back * Time.deltaTime * speed;

        public void Release() => Destroyed?.Invoke(this);
    }
}
