using Scripts.Managers;
using Scripts.Objects;
using UnityEngine;
using Zenject;
using static Scripts.Utils.Enums;

namespace Scripts.Characters
{
    public class ItemsCollector : MonoBehaviour
    {
        [SerializeField] private LayerMask _target;

        private AudioManager _audioManager;
        private GameManager _gameSession;

        [Inject]
        private void Construct(AudioManager audioManager, GameManager gameSession)
        {
            _audioManager = audioManager;
            _gameSession = gameSession;
        }

        private void OnTriggerEnter(Collider other)
        {
            if ((_target & (1 << other.gameObject.layer)) != 0)
            {
                other.TryGetComponent(out CollectableComponent item);
                if (item != null)
                {
                    switch (item.Type)
                    {
                        case CollectableItemType.Coin:
                            _audioManager.PlaySfx("coin");
                            _gameSession.AddCoins(item.Value);
                            break;

                        case CollectableItemType.Board:
                            _audioManager.PlaySfx("board");
                            _gameSession.AddBoards(item.Value);
                            break;
                    }

                    Destroy(item.gameObject);
                }
            }
        }
    }
}
