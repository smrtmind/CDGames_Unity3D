using Scripts.Player;
using Scripts.Utils;
using UnityEngine;

namespace Scripts.Objects
{
    public class SectionComponent : MonoBehaviour
    {
        [SerializeField] private float _movingSpeed = 0.1f;
        [SerializeField] private Transform _section;

        private GameSession _session;
        private PlayerController _player;

        private void Awake()
        {
            _session = FindObjectOfType<GameSession>();
            _player = FindObjectOfType<PlayerController>();
        }

        private void Update()
        {
            if (_session.GameIsStarted)
                _section.transform.position = new Vector3(0f, 0f, transform.position.z + _movingSpeed);

            if (_player.IsDead)
                _session.GameIsStarted = false;
        }
    }
}
