using Scripts.Characters;
using Scripts.Service;
using UnityEngine;
using Zenject;

namespace Scripts.Objects
{
    public class ObstacleComponent : MonoBehaviour
    {
        [SerializeField] private LayerMask _target;

        private PlayerController _player;

        [Inject]
        private void Construct(PlayerController player)
        {
            _player = player;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if ((_target & (1 << collision.gameObject.layer)) != 0)
            {
                FindObjectOfType<AudioComponent>().PlaySfx("hit");
                _player.Die();
            }
        }

        //private void OnCollisionEnter(Collision collision)
        //{
        //    var player = collision.gameObject.tag == "Player";
        //    if (player)
        //    {
        //        FindObjectOfType<AudioComponent>().PlaySfx("hit");
        //        _player.IsDead = true;

        //        _session.StopGame();
        //    }
        //}
    }
}
