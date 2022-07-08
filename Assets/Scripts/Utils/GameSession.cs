using Scripts.Player;
using UnityEngine;

namespace Scripts.Utils
{
    public class GameSession : MonoBehaviour
    {
        [SerializeField] private GameObject _gameOverLayout;
        [SerializeField] private GameObject _victoryLayout;
        [SerializeField] private FollowCamera _followCamera;
        [SerializeField] private int _boards;
        [SerializeField] private int _coins;
        [SerializeField] private int _coinsToWin = 100;
        [SerializeField] private float _onStartDelay = 3f;
        [SerializeField] private PlayerController _player;
        [SerializeField] private SliderComponent _boardSlider;

        public int CoinsToWin => _coinsToWin;

        public int Boards
        {
            get => _boards;
            set => _boards = value;
        }

        public int Coins => _coins;

        private bool _gameIsStarted;
        public bool GameIsStarted
        {
            get => _gameIsStarted;
            set => _gameIsStarted = value;
        }

        public float OnStartDelay => _onStartDelay;

        public void ModifyBoards(int value) => _boards += value;

        public void ModifyCoins(int value) => _coins += value;

        private void Update()
        {
            if (!_gameIsStarted)
            {
                if (_onStartDelay > 0)
                {
                    _onStartDelay -= Time.deltaTime;
                }
                else
                {
                    _onStartDelay = 0;

                    _player.IsRunning = true;
                    _gameIsStarted = true;
                }
            }

            if (_player.IsDead || _player.gameObject.transform.position.y < -15f)
                StopGame();

            if (_coins == _coinsToWin)
            {
                StopGame(win: true);
                _player.IsWin = true;
            }
        }

        public void StopGame(bool win = false)
        {
            if (!win)
                _gameOverLayout.SetActive(true);
            else
                _victoryLayout.SetActive(true);

            _followCamera.enabled = false;

            _boardSlider.gameObject.SetActive(false);
            _gameIsStarted = false;
        }
    }
}
