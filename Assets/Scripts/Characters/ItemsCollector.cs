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
        private MatchManager _matchManager;

        [Inject]
        private void Construct(AudioManager audioManager, MatchManager matchManager)
        {
            _audioManager = audioManager;
            _matchManager = matchManager;
        }

        private void OnTriggerEnter(Collider other)
        {
            if ((_target & (1 << other.gameObject.layer)) != 0)
            {
                other.TryGetComponent(out CollectableItem item);
                if (item != null)
                {
                    switch (item.Type)
                    {
                        case ItemType.Coin:
                            _audioManager.PlaySfx("coin");
                            _matchManager.AddCoins(item.Value);
                            break;

                        case ItemType.Board:
                            _audioManager.PlaySfx("board");
                            _matchManager.AddBoards(item.Value);
                            break;
                    }

                    item.Release();
                }
            }
        }
    }
}
