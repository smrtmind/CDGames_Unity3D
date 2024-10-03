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
        [SerializeField] private Slider _coinsBar;
        [SerializeField] private TMP_Text _timer;
        [SerializeField] private GameObject _tutorialHint;

        private GameSession _gameSession;
        private Coroutine _timerRoutine;

        [Inject]
        private void Construct(GameSession gameSession)
        {
            _gameSession = gameSession;
        }

        private void OnEnable()
        {
            GameSession.OnMatchStarted += OnMatchStarted;

            InitCoinsBar();
        }

        private void OnDisable()
        {
            GameSession.OnMatchStarted -= OnMatchStarted;

            this.StopCoroutine(ref _timerRoutine);
        }

        private void OnMatchStarted()
        {
            _timer.gameObject.SetActive(false);
            _tutorialHint.SetActive(false);
        }

        private void Start()
        {
            if (_timerRoutine == null)
                _timerRoutine = StartCoroutine(Timer());
        }

        private IEnumerator Timer()
        {
            var timer = _gameSession.TimerOnstart;

            while (timer > 0)
            {
                _timer.text = $"{Mathf.RoundToInt(timer)}";
                yield return timer -= Time.deltaTime;
            }

            _timer.color = Color.green;
            _timer.text = "GO";

            _timerRoutine = null;
        }

        private void InitCoinsBar()
        {
            _coinsBar.maxValue = _gameSession.CoinsToWin;
            _coinsBar.value = 0f;
        }

        private void RefreshCoinsBar(int coins) => _coinsBar.value = coins;
    }
}
