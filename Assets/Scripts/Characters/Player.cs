using Scripts.Managers;
using Scripts.Utils;
using System;
using UnityEngine;

namespace Scripts.Characters
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private AnimationsController _animationsController;
        [SerializeField] private Rigidbody _rigidBody;
        [SerializeField] private float _gravityMultiplier = 1f;



        [SerializeField] private float _groundBeemLength = 2f;

        private SliderComponent _boardSlider;

        public bool IsDead { get; private set; }

        public void Die()
        {
            IsDead = true;
            MatchManager.OnMatchEnded?.Invoke();
        }

        //private bool _isGrounded;


        //public bool IsRunning
        //{
        //    get => _isRunning;
        //    set => _isRunning = value;
        //}

        //public bool IsDead
        //{
        //    get => _isDead;
        //    set => _isDead = value;
        //}

        //public bool IsWin
        //{
        //    get => _isWin;
        //    set => _isWin = value;
        //}

        private bool _canMove;

        private void OnEnable()
        {
            MatchManager.OnMatchStarted += OnMatchStarted;

            _canMove = false;
        }

        private void OnDisable()
        {
            MatchManager.OnMatchStarted -= OnMatchStarted;
        }

        private void OnMatchStarted()
        {
            _canMove = true;

        }

        public bool _slideUp { get; set; }
        public bool _slideDown { get; set; }

        private void Awake()
        {
            _boardSlider = FindObjectOfType<SliderComponent>();
        }

        private void Update()
        {
            //_isGrounded = Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), _groundBeemLength);
            //Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * _groundBeemLength, Color.red);

            if (_canMove)
            {
                if (_boardSlider.SliderIsTouching)
                {
                    _boardSlider.slider.interactable = true;
                    _boardSlider.SpawnBoards = true;
                }
                else
                {
                    _boardSlider.slider.value = 0;

                    _boardSlider.slider.interactable = false;
                    _boardSlider.SpawnBoards = false;
                }
            }

            _boardSlider.slider.enabled = _canMove;

            if (!IsGrounded())
                _rigidBody.AddForce(new Vector3(0f, -_gravityMultiplier, 0f));

            //UpdateAnimation();
        }

        private bool IsGrounded()
        {
            var isGrounded = Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), _groundBeemLength);
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * _groundBeemLength, Color.red);

            return isGrounded;
        }

        //private void UpdateAnimation()
        //{
        //    _animator.SetBool(IsRunningKey, _isRunning);
        //    _animator.SetBool(IsDeadKey, _isDead);
        //    _animator.SetBool(IsWinKey, _isWin);

        //    //_animator.SetBool(IsFallingKey, !_isGrounded);  
        //}
    }
}
