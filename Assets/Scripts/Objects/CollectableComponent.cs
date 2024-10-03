using Scripts.Utils;
using UnityEngine;
using static Scripts.Utils.Enums;

namespace Scripts.Objects
{
    public class CollectableComponent : MonoBehaviour
    {
        [field: SerializeField] public CollectableItemType Type { get; private set; }
        [field: SerializeField, Min(1)] public int Value { get; private set; } = 1;

        //private GameSession _gameSession;
        //private AudioComponent _audio;

        //private void Awake()
        //{
        //    _gameSession = FindObjectOfType<GameSession>();
        //    _audio = FindObjectOfType<AudioComponent>();
        //}

        //private void OnTriggerEnter(Collider other)
        //{
        //    var player = other.gameObject.tag == "Player";
        //    if (player)
        //    {
        //        if (_item == Item.Board)
        //        {
        //            _audio.PlaySfx("board");
        //            _gameSession.ModifyBoards(_amount);
        //        }
                    
        //        if (_item == Item.Coin)
        //        {
        //            _audio.PlaySfx("coin");
        //            _gameSession.ModifyCoins(_amount);
        //        }  

        //        Destroy(gameObject);
        //    }
        //}
    }
}
