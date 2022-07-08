using Scripts.Utils;
using UnityEngine;

namespace Scripts.Objects
{
    public class CollectableComponent : MonoBehaviour
    {
        [SerializeField] private Item _item;
        [SerializeField] private int _amount = 1;

        private GameSession _gameSession;
        private AudioComponent _audio;

        private void Awake()
        {
            _gameSession = FindObjectOfType<GameSession>();
            _audio = FindObjectOfType<AudioComponent>();
        }

        private void OnTriggerEnter(Collider other)
        {
            var player = other.gameObject.tag == "Player";
            if (player)
            {
                if (_item == Item.Board)
                {
                    _audio.PlaySfx("board");
                    _gameSession.ModifyBoards(_amount);
                }
                    
                if (_item == Item.Coin)
                {
                    _audio.PlaySfx("coin");
                    _gameSession.ModifyCoins(_amount);
                }  

                Destroy(gameObject);
            }
        }

        private enum Item
        {
            Coin,
            Board
        }
    }
}
