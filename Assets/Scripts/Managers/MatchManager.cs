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
        [field: SerializeField] public float TimerOnStart { get; private set; } = 3f;
        [SerializeField, Min(0f)] private float _delayOnLevelComplete = 2f;
        [SerializeField, Min(0)] private int _boardsOnStart;

        [Space]
        [SerializeField, Min(0.1f)] private float _defaultLevelSpeed  = 0.3f;
        [SerializeField, Min(0.01f)] private float _levelSpeedStep = 0.05f;

        public static Action OnMatchStarted;
        public static Action<int> OnScoreAmountChanged;
        public static Action<int> OnBoardsAmountChanged;

        public int Score { get; private set; }
        public int Boards { get; private set; }
        public bool IsStarted { get; private set; }
        public float LevelSpeed { get; private set; }

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

            Score = 0;
            Boards = _boardsOnStart;
            LevelSpeed = _defaultLevelSpeed;
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

                    this.WaitForSeconds(TimerOnStart, () =>
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
            IsStarted = false;

            this.WaitForSeconds(_delayOnLevelComplete, () => _gameManager.ChangeState(GameState.GameOver));
        }

        public void IncreaseScore()
        {
            Score++;
            OnScoreAmountChanged?.Invoke(Score);
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

        public void IncreaseLevelSpeed() => LevelSpeed += _levelSpeedStep;

        private void OnDisable()
        {
            Unsubscribe();
        }
    }
}
