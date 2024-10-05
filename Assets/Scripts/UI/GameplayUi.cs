using Scripts.Characters;
using Scripts.Managers;
using Scripts.Utils;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Scripts.UI
{
    public class GameplayUi : MonoBehaviour
    {
        #region Variables
        [Header("Components")]
        [SerializeField] private Image _progressFiller;
        [SerializeField] private TMP_Text _countdown;
        [SerializeField] private TMP_Text _timer;
        [SerializeField] private GameObject _tutorialHand;

        [Header("Parameters")]
        [SerializeField] private Color _timerStartColor;
        [SerializeField] private Color _timerEndColor;

        public string FormattedTimer { get; private set; }

        private MatchManager _matchManager;
        private Coroutine _countdownRoutine;
        private Coroutine _timerRoutine;
        private WaitForEndOfFrame _waitForEndOfFrame = new();

        private float _progressStep;
        #endregion

        [Inject]
        private void Construct(MatchManager matchManager)
        {
            _matchManager = matchManager;
        }

        private void OnEnable()
        {
            Subscribe();
            InitProgressBar();
        }

        private void Subscribe()
        {
            GameManager.OnAfterStateChanged += OnAfterStateChanged;
            MatchManager.OnMatchStarted += OnMatchStarted;
            MatchManager.OnMatchEnded += StopTimer;
            MatchManager.OnCoinsAmountChanged += OnCoinsAmountChanged;
            Player.OnPlayerLost += StopTimer;
        }

        private void Unsubscribe()
        {
            GameManager.OnAfterStateChanged -= OnAfterStateChanged;
            MatchManager.OnMatchStarted -= OnMatchStarted;
            MatchManager.OnMatchEnded -= StopTimer;
            MatchManager.OnCoinsAmountChanged -= OnCoinsAmountChanged;
            Player.OnPlayerLost -= StopTimer;
        }

        private void OnAfterStateChanged(GameState state)
        {
            if (state != GameState.Gameplay) return;

            if (_countdownRoutine == null)
                _countdownRoutine = StartCoroutine(Countdown());
        }

        private void OnMatchStarted()
        {
            _countdown.gameObject.SetActive(false);
            _tutorialHand.SetActive(false);
            _timer.gameObject.SetActive(true);

            if (_timerRoutine == null)
                _timerRoutine = StartCoroutine(Timer());
        }

        private void StopTimer() => this.StopCoroutine(ref _timerRoutine);

        private void OnCoinsAmountChanged(int coins) => _progressFiller.fillAmount = _progressStep * coins;

        private IEnumerator Countdown()
        {
            var countdown = _matchManager.TimerOnstart;

            while (countdown > 0f)
            {
                var roundedSeconds = Mathf.RoundToInt(countdown);
                var actualText = roundedSeconds >= 1f ? $"{roundedSeconds}" : "GO";
                var actualColor = roundedSeconds >= 1f ? _timerStartColor : _timerEndColor;

                _countdown.text = actualText;
                _countdown.color = actualColor;

                yield return countdown -= Time.deltaTime;
            }

            _countdownRoutine = null;
        }

        private IEnumerator Timer()
        {
            var timeElapsed = 0f;

            while (true)
            {
                timeElapsed += Time.deltaTime;

                var minutes = Mathf.FloorToInt(timeElapsed / 60);
                var seconds = Mathf.FloorToInt(timeElapsed % 60);

                FormattedTimer = string.Format("{0:00}:{1:00}", minutes, seconds);
                _timer.text = FormattedTimer;

                yield return _waitForEndOfFrame;
            }
        }

        private void InitProgressBar()
        {
            _progressFiller.fillAmount = 0f;
            _progressStep = 1f / _matchManager.CoinsToWin;

            _timer.gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            Unsubscribe();

            StopTimer();
            this.StopCoroutine(ref _countdownRoutine);
        }
    }
}
