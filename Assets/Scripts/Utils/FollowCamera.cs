using Scripts.Player;
using UnityEngine;

namespace Scripts.Utils
{
    public class FollowCamera : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private Transform _player;
        [SerializeField] private Vector3 _offset;

        public Vector3 Offset
        {
            get => _offset;
            set => _offset = value;
        }

        private void Update()
        {
            transform.position = _player.position + _offset;
        }
    }
}
