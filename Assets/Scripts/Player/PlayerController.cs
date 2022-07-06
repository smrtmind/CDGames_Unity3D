using Scripts.Utils;
using System.Collections;
using UnityEngine;

namespace Scripts.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Rigidbody _body;
        [SerializeField] private float _runSpeed = 5f;
        [SerializeField] private float _movementSpeed = 5f;
        [SerializeField] private float _gravityMultiplier = 1f;
        [SerializeField] private SpawnComponent _boardSpawner;
        [SerializeField] private Animator _animator;

        [Space]
        [SerializeField] private bool _isGrounded;
        [SerializeField] private float _groundBeemLength = 2f;

        private static readonly int IsGrounded = Animator.StringToHash("is-grounded");

        public bool _slideUp { get; set; }
        public bool _slideDown { get; set; }

        private bool _boardsIsUsing = false;

        private void Update()
        {
            _isGrounded = Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), _groundBeemLength);
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * _groundBeemLength, Color.red);

            _body.velocity = new Vector3(0f, 0f, _runSpeed);

            if (_slideUp || _slideDown)
            {
                if (!_boardsIsUsing)
                {
                    _boardsIsUsing = true;

                    StartCoroutine(BuildFloor());
                }

                if (_slideUp)
                    _body.AddForce(new Vector3(0f, _movementSpeed, 0f));
                else if (_slideDown)
                    _body.AddForce(new Vector3(0f, -_movementSpeed, 0f));
            }
            else
                _body.AddForce(new Vector3(0f, -_gravityMultiplier, 0f));

            UpdateAnimation();
        }

        private void UpdateAnimation()
        {
            _animator.SetBool(IsGrounded, _isGrounded ? true : false);  
        }

        private IEnumerator BuildFloor()
        {
            _boardSpawner.Spawn();

            yield return new WaitForSeconds(0.05f);

            _boardsIsUsing = false;
        }
    }
}
