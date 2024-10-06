using DG.Tweening;
using Scripts.Characters;
using Scripts.Managers;
using System;
using UnityEngine;
using Zenject;
using IPoolable = Scripts.Pooling.IPoolable;


namespace Scripts.Objects
{
    public class CollectableBoard : MonoBehaviour, IPoolable
    {
        [SerializeField, Min(1)] private int _value = 10;
        [SerializeField, Min(1f)] private float _jumpHeight = 4f;
        [SerializeField, Min(1f)] private float _jumpDuration = 0.35f;
        [SerializeField] private Vector3 _endScale;

        public event Action<IPoolable> Destroyed;

        public GameObject GameObject => gameObject;

        private AudioManager _audioManager;
        private MatchManager _matchManager;
        private Player _player;
        private Tween _jumpTween;
        private Tween _scaleTween;

        [Inject]
        private void Construct(AudioManager audioManager, MatchManager matchManager, Player player)
        {
            _audioManager = audioManager;
            _matchManager = matchManager;
            _player = player;
        }

        public void Collect()
        {
            transform.localScale = Vector3.one;

            Jump();
            Scale();
        }

        public void Jump()
        {
            _jumpTween?.Kill();
            _jumpTween = transform.DOJump(_player.transform.position, _jumpHeight, 1, _jumpDuration)
                .SetEase(Ease.Linear)
                .SetLink(gameObject)
                .OnComplete(() =>
                {
                    _audioManager.PlaySfx("board");
                    _matchManager.AddBoards(_value);

                    Release();
                });
        }

        private void Scale()
        {
            _scaleTween?.Kill();
            _scaleTween = transform.DOScale(_endScale, _jumpDuration)
                .SetEase(Ease.Linear)
                .SetLink(gameObject);
        }

        public void Release() => Destroyed?.Invoke(this);

        private void OnDisable()
        {
            _jumpTween?.Kill();
            _scaleTween?.Kill();
        }
    }
}
