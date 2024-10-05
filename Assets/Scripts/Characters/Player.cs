using Scripts.Managers;
using System;
using UnityEngine;
using Zenject;

namespace Scripts.Characters
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private AnimationsController _animationsController;
        [SerializeField] private Rigidbody _rigidBody;
        [SerializeField] private Collider _collider;

        [Space]
        [SerializeField] private LayerMask _targetGround;
        [SerializeField, Min(0.5f)] private float _raycastLength = 2f;
        [SerializeField] private float _losePositionByY = -15f;
        [SerializeField] private float _gravityMultiplier;

        public static Action OnPlayerLost;

        public bool IsDead { get; private set; }

        private AudioManager _audioManager;

        [Inject]
        private void Construct(AudioManager audioManager)
        {
            _audioManager = audioManager;
        }

        private void OnEnable()
        {
            Subscribe();

            _rigidBody.useGravity = false;
        }

        private void Subscribe()
        {
            GameManager.OnAfterStateChanged += OnAfterStateChanged;
            MatchManager.OnMatchStarted += OnMatchStarted;
            MatchManager.OnMatchEnded += OnMatchEnded;
        }

        private void Unsubscribe()
        {
            GameManager.OnAfterStateChanged -= OnAfterStateChanged;
            MatchManager.OnMatchStarted -= OnMatchStarted;
            MatchManager.OnMatchEnded -= OnMatchEnded;
        }

        private void OnAfterStateChanged(GameState state)
        {
            if (state != GameState.Gameplay) return;

            _rigidBody.useGravity = true;
        }

        private void OnMatchStarted() => _animationsController.Run();

        private void OnMatchEnded() => _animationsController.Win();

        private void Update()
        {
            if (transform.position.y < _losePositionByY)
            {
                OnPlayerLost?.Invoke();
                Destroy(gameObject);
            }

            if (!IsGrounded())
                _rigidBody.AddForce(new Vector3(0f, _gravityMultiplier, 0f));
        }

        private bool IsGrounded()
        {
            var isGrounded = Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), _raycastLength, _targetGround);
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * _raycastLength, Color.red);

            return isGrounded;
        }

        public void Die()
        {
            IsDead = true;

            _animationsController.Lose();
            _audioManager.PlaySfx("hit");

            OnPlayerLost?.Invoke();
        }

        private void OnDisable()
        {
            Unsubscribe();
        }
    }
}
