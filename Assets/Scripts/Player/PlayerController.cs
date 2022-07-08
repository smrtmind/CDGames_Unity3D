using Scripts.Utils;
using UnityEngine;

namespace Scripts.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Rigidbody _body;
        [SerializeField] private float _gravityMultiplier = 1f;
        [SerializeField] private Animator _animator;

        [Space]
        [SerializeField] private bool _isGrounded;
        [SerializeField] private float _groundBeemLength = 2f;

        private static readonly int IsFallingKey = Animator.StringToHash("is-falling");
        private static readonly int IsRunningKey = Animator.StringToHash("is-running");
        private static readonly int IsDeadKey = Animator.StringToHash("is-dead");
        private static readonly int IsWinKey = Animator.StringToHash("is-win");

        private SliderComponent _boardSlider;
        private bool _isRunning;
        private bool _isDead;
        private bool _isWin;

        public bool IsRunning
        {
            get => _isRunning;
            set => _isRunning = value;
        }

        public bool IsDead
        {
            get => _isDead;
            set => _isDead = value;
        }

        public bool IsWin
        {
            get => _isWin;
            set => _isWin = value;
        }

        public bool _slideUp { get; set; }
        public bool _slideDown { get; set; }

        private void Awake()
        {
            _boardSlider = FindObjectOfType<SliderComponent>();
        }

        private void Update()
        {
            _isGrounded = Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), _groundBeemLength);
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * _groundBeemLength, Color.red);

            if (_isRunning)
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

            _boardSlider.slider.enabled = _isRunning ? true : false;

            if (!_isGrounded)
                _body.AddForce(new Vector3(0f, -_gravityMultiplier, 0f));

            UpdateAnimation();
        }

        private void UpdateAnimation()
        {
            _animator.SetBool(IsRunningKey, _isRunning);
            _animator.SetBool(IsDeadKey, _isDead);
            _animator.SetBool(IsWinKey, _isWin);

            //_animator.SetBool(IsFallingKey, !_isGrounded);  
        }
    }
}
