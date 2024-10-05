using Scripts.Characters;
using Scripts.Utils;
using System;
using UnityEngine;
using Zenject;

namespace Scripts.Managers
{
    public class MatchManager : MonoBehaviour
    {
        #region Variables
        [field: Header("Parameters")]
        [field: SerializeField] public int CoinsToWin { get; private set; } = 100;
        [field: SerializeField] public float TimerOnstart { get; private set; } = 3f;
        [SerializeField, Min(0f)] private float _delayOnLevelComplete = 2f;
        [SerializeField, Min(0)] private int _boardsOnStart;

        public static Action OnMatchStarted;
        public static Action OnMatchEnded;
        public static Action<int> OnCoinsAmountChanged;
        public static Action<int> OnBoardsAmountChanged;

        public int Coins { get; private set; }
        public int Boards { get; private set; }
        public bool IsStarted { get; private set; }

        private AudioManager _audioManager;
        private GameManager _gameManager;
        #endregion

        [Inject]
        private void Construct(AudioManager audioManager, GameManager gameManager)
        {
            _audioManager = audioManager;
            _gameManager = gameManager;
        }

        private void OnEnable()
        {
            Subscribe();

            IsStarted = false;

            Coins = 0;
            Boards = _boardsOnStart;
        }

        private void Subscribe()
        {
            GameManager.OnAfterStateChanged += OnAfterStateChanged;
            Player.OnPlayerLost += OnPlayerLost;
        }

        private void Unsubscribe()
        {
            GameManager.OnAfterStateChanged -= OnAfterStateChanged;
            Player.OnPlayerLost -= OnPlayerLost;
        }

        private void OnAfterStateChanged(GameState state)
        {
            switch (state)
            {
                case GameState.StartScreen:
                    _audioManager.SetMusicTrack("startScreen");
                    break;

                case GameState.Gameplay:
                    _audioManager.SetMusicTrack("gameplay");

                    this.WaitForSeconds(TimerOnstart, () =>
                    {
                        IsStarted = true;
                        OnMatchStarted?.Invoke();
                    });
                    break;
            }
        }

        private void OnPlayerLost()
        {
            Boards = 0;
            EndMatchWithDelay(win: false);
        }

        public void AddCoins(int value)
        {
            Coins += value;
            OnCoinsAmountChanged?.Invoke(Coins);

            if (Coins >= CoinsToWin)
            {
                Coins = CoinsToWin;
                Boards = 0;

                EndMatchWithDelay(win: true);
                OnMatchEnded?.Invoke();
            }
        }

        public void AddBoards(int value)
        {
            Boards += value;
            OnBoardsAmountChanged?.Invoke(Boards);
        }

        public void RemoveBoard()
        {
            Boards--;
            if (Boards <= 0)
                Boards = 0;

            OnBoardsAmountChanged?.Invoke(Boards);
        }

        private void EndMatchWithDelay(bool win)
        {
            IsStarted = false;

            var state = win ? GameState.Victory : GameState.Defeat;
            this.WaitForSeconds(_delayOnLevelComplete, () => _gameManager.ChangeState(state));
        }

        private void OnDisable()
        {
            Unsubscribe();
        }
    }
}
