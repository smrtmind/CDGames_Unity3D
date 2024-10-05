using DG.Tweening;
using Scripts.Managers;
using System;
using UnityEngine;
using Zenject;
using IPoolable = Scripts.Pooling.IPoolable;

namespace Scripts.Objects
{
    public class Section : MonoBehaviour, IPoolable
    {
        [SerializeField] private BoxCollider _collider;
        [SerializeField] private float _movingSpeed = 0.1f;
        [SerializeField, Min(0.1f)] private float _verticalSpeed = 0.25f;
        [SerializeField] private float _distanceToReleaseByZ = -10f;

        public event Action<IPoolable> Destroyed;

        public GameObject GameObject => gameObject;
        public float SizeZ => _collider.size.z;

        private MatchManager _matchManager;
        private GameManager _gameManager;
        private Tween _moveTween;

        [Inject]
        private void Construct(MatchManager matchManager, GameManager gameManager)
        {
            _matchManager = matchManager;
            _gameManager = gameManager;
        }

        private void OnEnable()
        {
            VerticalMove();
        }

        private void Update()
        {
            if (!_matchManager.IsStarted || _gameManager.State != GameState.Gameplay) return;

            if (transform.position.z < _distanceToReleaseByZ)
                Release();

            Move();
        }

        private void Move() => transform.position = new Vector3(0f, transform.position.y, transform.position.z + _movingSpeed);

        private void VerticalMove()
        {
            _moveTween?.Kill();
            _moveTween = transform.DOMoveY(0f, _verticalSpeed)
                .SetEase(Ease.OutBack)
                .SetLink(gameObject);
        }

        public void Release() => Destroyed?.Invoke(this);

        private void OnDisable()
        {
            _moveTween?.Kill();
        }
    }
}
