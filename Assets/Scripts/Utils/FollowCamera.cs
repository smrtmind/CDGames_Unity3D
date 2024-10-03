using Scripts.Characters;
using Scripts.Service;
using UnityEngine;
using Zenject;

namespace Scripts.Utils
{
    public class FollowCamera : MonoBehaviour
    {
        [SerializeField] private Vector3 _offset;

        private Transform _playerTransform;
        private Camera _mainCamera;

        [Inject]
        private void Construct(PlayerController player, CameraController cameraController)
        {
            _playerTransform = player.transform;
            _mainCamera = cameraController.GetMainCamera();
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
