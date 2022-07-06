using UnityEngine;

namespace Scripts.Player
{
    public class PlayerInput : MonoBehaviour
    {
        [SerializeField] private PlayerController _player;
        [SerializeField] private Joystick _joystick;

        private void Update()
        {
            _player._slideUp = _joystick.Vertical >= 0.2f;
            _player._slideDown = _joystick.Vertical <= -0.2f;
        }
    }
}
