using Scripts.Managers;
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

        private MatchManager _matchManager;
        private GameManager _gameManager;

        [Inject]
        private void Construct(MatchManager matchManager, GameManager gameManager)
        {
            _matchManager = matchManager;
            _gameManager = gameManager;
        }

        private void Update()
        {
            if (!_matchManager.IsStarted || _gameManager.State != GameState.Gameplay) return;

            Move();
        }

        private void Move() => transform.position = new Vector3(0f, 0f, transform.position.z + _movingSpeed);

        public void Release() => Destroyed?.Invoke(this);
    }
}
