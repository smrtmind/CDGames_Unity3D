using Scripts.Player;
using Scripts.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Hud
{
    public class HudController : MonoBehaviour
    {
        [SerializeField] private Text _countdown;
        [SerializeField] private GameSession _session;
        [SerializeField] private PlayerController _player;
        [SerializeField] private Text _boardsValue;
        [SerializeField] private Text _coinsValue;
        [SerializeField] private Slider _coinsBar;
        [SerializeField] private GameObject _gestures;

        private void Awake()
        {
            _coinsBar.maxValue = _session.CoinsToWin;
        }

        private void Update()
        {
            _boardsValue.text = $"{_session.Boards}";
            _coinsValue.text = $"{_session.Coins}";
            _coinsBar.value = _session.Coins;

            if ((int)_session.OnStartDelay > 0)
            {
                _countdown.text = $"{(int)_session.OnStartDelay}";
            }
            else
            {
                _countdown.color = Color.green;
                _countdown.text = "GO";
            }

            if (_player.IsRunning)
            {
                _countdown.gameObject.SetActive(false);
                _gestures.SetActive(false);
            }
        }
    }
}
