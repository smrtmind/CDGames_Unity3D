using Scripts.Managers;
using Scripts.Utils;
using System;
using UnityEngine;
using Zenject;
using IPoolable = Scripts.Pooling.IPoolable;

namespace Scripts.Objects
{
    public class Section : MonoBehaviour, IPoolable
    {
        [SerializeField] private float _movingSpeed = 0.1f;

        public event Action<IPoolable> Destroyed;

        public GameObject GameObject => gameObject;

        private GameManager _gameSession;

        [Inject]
        private void Construct(GameManager gameSession)
        {
            _gameSession = gameSession;
        }

        private void Update()
        {
            if (!_gameSession.MatchIsStarted) return;

            transform.position = new Vector3(0f, 0f, transform.position.z + _movingSpeed);
        }

        public void Release() => Destroyed?.Invoke(this);
    }
}
