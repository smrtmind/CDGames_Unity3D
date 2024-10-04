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
        [SerializeField] private Slider _progressBar;
        [SerializeField] private TMP_Text _timer;
        [SerializeField] private TMP_Text _counter;
        [SerializeField] private GameObject _tutorialHint;

        [Space]
        [SerializeField] private Color _timerStartColor;
        [SerializeField] private Color _timerEndColor;

        private MatchManager _matchManager;
        private Coroutine _timerRoutine;

        [Inject]
        private void Construct(MatchManager matchManager)
        {
            _matchManager = matchManager;
        }

        private void OnEnable()
        {
            Subscribe();
            InitCoinsBar();
            RefreshCoinsCounter();
        }

        private void Subscribe()
        {
            GameManager.OnAfterStateChanged += OnAfterStateChanged;
            MatchManager.OnMatchStarted += OnMatchStarted;
            MatchManager.OnCoinsAmountChanged += OnCoinsAmountChanged;
        }

        private void Unsubscribe()
        {
            GameManager.OnAfterStateChanged -= OnAfterStateChanged;
            MatchManager.OnMatchStarted -= OnMatchStarted;
            MatchManager.OnCoinsAmountChanged -= OnCoinsAmountChanged;
        }

        private void OnAfterStateChanged(GameState state)
        {
            if (state != GameState.Gameplay) return;

            if (_timerRoutine == null)
                _timerRoutine = StartCoroutine(Timer());
        }

        private void OnMatchStarted()
        {
            _timer.gameObject.SetActive(false);
            _tutorialHint.SetActive(false);
        }

        private void OnCoinsAmountChanged(int coins)
        {
            _progressBar.value = coins;
            RefreshCoinsCounter();
        }

        private IEnumerator Timer()
        {
            var timer = _matchManager.TimerOnstart;

            while (timer > 0f)
            {
                var roundedTimer = Mathf.RoundToInt(timer);
                var actualText = roundedTimer >= 1f ? $"{roundedTimer}" : "GO";
                var actualColor = roundedTimer >= 1f ? _timerStartColor : _timerEndColor;

                _timer.text = actualText;
                _timer.color = actualColor;

                yield return timer -= Time.deltaTime;
            }

            _timerRoutine = null;
        }

        private void InitCoinsBar()
        {
            _progressBar.maxValue = _matchManager.CoinsToWin;
            _progressBar.value = 0f;
        }

        private void RefreshCoinsCounter() => _counter.text = $"<color=yellow>{_matchManager.Coins}</color>/{_matchManager.CoinsToWin}";

        private void OnDisable()
        {
            Unsubscribe();

            this.StopCoroutine(ref _timerRoutine);
        }
    }
}
