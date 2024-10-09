using DG.Tweening;
using Scripts.Managers;
using Scripts.Objects.Items;
using System;
using UnityEngine;
using Zenject;
using IPoolable = Scripts.Pooling.IPoolable;

namespace Scripts.Objects
{
    public class Section : MonoBehaviour, IPoolable
    {
        #region Variables
        [Header("Components")]
        [SerializeField] private BoxCollider _collider;
        [SerializeField] private Transform _spawnPoint;

        [Header("Parameters")]
        [SerializeField, Min(0.1f)] private float _verticalSpeed = 0.25f;
        [SerializeField] private float _distanceToReleaseByZ = -10f;

        public event Action<IPoolable> Destroyed;

        public GameObject GameObject => gameObject;
        public Vector3 Size => _collider.size;

        private MatchManager _matchManager;
        private GameManager _gameManager;
        private Tween _moveTween;
        private Tween _rotateTween;
        #endregion

        [Inject]
        private void Construct(MatchManager matchManager, GameManager gameManager)
        {
            _matchManager = matchManager;
            _gameManager = gameManager;
        }

        private void OnEnable()
        {
            VerticalMove();
            Rotate();
        }

        private void Update()
        {
            if (!_matchManager.IsStarted || _gameManager.State != GameState.Gameplay) return;

            if (transform.position.z < _distanceToReleaseByZ)
            {
                _matchManager.IncreaseScore();

                TryReleaseItem();
                Release();
            }

            Move();
        }

        private void Move()
        {
            transform.position = new Vector3(
                transform.position.x,
                transform.position.y,
                transform.position.z + (-_matchManager.LevelSpeed));
        }

        private void VerticalMove()
        {
            _moveTween?.Kill();
            _moveTween = transform.DOMoveY(0f, _verticalSpeed)
                .SetEase(Ease.OutBack)
                .SetLink(gameObject);
        }

        private void Rotate()
        {
            transform.rotation = Quaternion.identity;

            _rotateTween?.Kill();
            _rotateTween = transform.DORotate(new Vector3(0f, 360f, 0f), _verticalSpeed, RotateMode.FastBeyond360)
                .SetLink(gameObject);
        }

        public void Release() => Destroyed?.Invoke(this);

        public void SetCurrentItem(Item item)
        {
            item.transform.SetParent(_spawnPoint);
            item.transform.position = _spawnPoint.position;
        }

        private void TryReleaseItem()
        {
            if (_spawnPoint.childCount == 0) return;

            _spawnPoint.GetChild(0).TryGetComponent(out Item item);
            if (item != null)
                item.Release();
        }

        private void OnDisable()
        {
            _moveTween?.Kill();
            _rotateTween?.Kill();
        }
    }
}
