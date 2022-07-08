using Scripts.Player;
using Scripts.Utils;
using UnityEngine;

namespace Scripts.Objects
{
    public class ObstacleComponent : MonoBehaviour
    {
        private PlayerController _player;
        private GameSession _session;

        private void Awake()
        {
            _player = FindObjectOfType<PlayerController>();
            _session = FindObjectOfType<GameSession>();
        }

        private void OnCollisionEnter(Collision collision)
        {
            var player = collision.gameObject.tag == "Player";
            if (player)
            {
                FindObjectOfType<AudioComponent>().PlaySfx("hit");
                _player.IsDead = true;

                _session.StopGame();
            }
        }
    }
}
