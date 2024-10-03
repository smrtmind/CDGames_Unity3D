using Scripts.Characters;
using UnityEngine;
using Zenject;

namespace Scripts.Utils
{
    public class FollowCamera : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private Vector3 _offset;

        private Transform _playerTransform;

        [Inject]
        private void Construct(PlayerController player)
        {
            _playerTransform = player.transform;
        }

        public Vector3 Offset
        {
            get => _offset;
            set => _offset = value;
        }

        private void Update()
        {
            transform.position = _playerTransform.position + _offset;
        }
    }
}
