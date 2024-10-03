using Scripts.Characters;
using Scripts.Utils;
using System;
using UnityEngine;
using Zenject;

namespace Scripts.Managers
{
    public class GameManager : MonoBehaviour
    {

        [field: SerializeField] public int CoinsToWin { get; private set; } = 100;
        [field: SerializeField] public float TimerOnstart { get; private set; } = 3f;
        [SerializeField] private SliderComponent _boardSlider;

        public static Action OnMatchStarted;
        public static Action OnMatchEnded;

        public static Action<int> OnCoinsAmountChanged;
        public static Action<int> OnBoardsAmountChanged;

        private int _coins;

        public bool MatchIsStarted { get; private set; }
        public int Boards { get; private set; }

        private Player _player;

        [Inject]
        private void Construct(Player player)
        {
            _player = player;
        }

        public void AddCoins(int value)
        {
            _coins += value;
            OnCoinsAmountChanged?.Invoke(_coins);
        }

        public void AddBoards(int value)
        {
            Boards += value;
            OnBoardsAmountChanged?.Invoke(Boards);
        }

        public void RemoveBoard()
        {
            Boards--;
            OnBoardsAmountChanged?.Invoke(Boards);
        }

        private void OnEnable()
        {
            this.WaitForSeconds(TimerOnstart, () =>
            {
                MatchIsStarted = true;
                OnMatchStarted?.Invoke();
            });
        }

        private void Update()
        {
            //if (!_gameIsStarted)
            //{
            //    if (_onStartDelay > 0)
            //    {
            //        _onStartDelay -= Time.deltaTime;
            //    }
            //    else
            //    {
            //        _onStartDelay = 0;

            //        OnMatchStarted?.Invoke();
            //        _gameIsStarted = true;
            //    }
            //}

            if (_player.IsDead || _player.gameObject.transform.position.y < -15f)
                StopGame();

            if (_coins == CoinsToWin)
            {
                //StopGame(win: true);
                //_player.IsWin = true;
            }
        }

        public void StopGame()
        {
            //_gameOverLayout.SetActive(true);

            //_followCamera.enabled = false;
            _boardSlider.gameObject.SetActive(false);
        }
    }
}
