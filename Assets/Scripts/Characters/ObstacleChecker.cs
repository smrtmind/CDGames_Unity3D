using UnityEngine;

namespace Scripts.Characters
{
    public class ObstacleChecker : MonoBehaviour
    {
        [SerializeField] private Player _player;
        [SerializeField] private LayerMask _target;

        private void OnCollisionEnter(Collision collision)
        {
            if ((_target & (1 << collision.gameObject.layer)) != 0)
            {
                _player.Die();
            }
        }
    }
}
