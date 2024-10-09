using Scripts.Managers;
using Scripts.Objects.Items;
using Scripts.Pooling;
using UnityEngine;
using Zenject;

namespace Scripts.Characters
{
    public class ObstacleChecker : MonoBehaviour
    {
        [SerializeField] private Player _player;
        [SerializeField] private LayerMask _target;

        [Space]
        [SerializeField] private ParticleObject _explosionParticle;
        [SerializeField] private ParticleObject _boomTextParticle;
        [SerializeField] private float _particleOffsetZ = 1f;

        private AudioManager _audioManager;
        private ObjectPool _objectPool;

        [Inject]
        private void Construct(AudioManager audioManager, ObjectPool objectPool)
        {
            _audioManager = audioManager;
            _objectPool = objectPool;
        }

        private void SpawnBoomText()
        {
            var boomText = _objectPool.Get(_boomTextParticle);

            var newPosition = transform.position;
            newPosition.z -= _particleOffsetZ;
            boomText.transform.position = newPosition;
        }

        private void SpawnExplosion(Collision collision)
        {
            collision.gameObject.TryGetComponent(out Obstacle obstacle);
            if (obstacle != null)
            {
                var explosion = _objectPool.Get(_explosionParticle);
                explosion.transform.position = obstacle.transform.position;

                obstacle.Release();
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if ((_target & (1 << collision.gameObject.layer)) != 0)
            {
                if (!_player.IsDead)
                {
                    _audioManager.PlaySfx("death");
                    _audioManager.StopMusic();

                    SpawnBoomText();
                    SpawnExplosion(collision);

                    _player.Die();
                }
            }
        }
    }
}
