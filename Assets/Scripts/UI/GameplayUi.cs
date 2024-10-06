using Scripts.Managers;
using Scripts.Utils;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

namespace Scripts.UI
{
    public class GameplayUi : MonoBehaviour
    {
        #region Variables
        [Header("Components")]
        [SerializeField] private TMP_Text _countdown;
        [SerializeField] private TMP_Text _score;
        [SerializeField] private Button _returnButton;
        [SerializeField] private GameObject _tutorialHand;

        [Header("Parameters")]
        [SerializeField] private Color _timerStartColor;
        [SerializeField] private Color _timerEndColor;

        private MatchManager _matchManager;
        private Coroutine _countdownRoutine;
        #endregion

        [Inject]
        private void Construct(MatchManager matchManager)
        {
            _matchManager = matchManager;
        }

        private void OnEnable()
        {
            Subscribe();
        }

        private void Subscribe()
        {
            GameManager.OnAfterStateChanged += OnAfterStateChanged;
            MatchManager.OnMatchStarted += OnMatchStarted;
            MatchManager.OnScoreAmountChanged += OnScoreAmountChanged;
            _returnButton.onClick.AddListener(ReturnToMenu);
        }

        private void Unsubscribe()
        {
            GameManager.OnAfterStateChanged -= OnAfterStateChanged;
            MatchManager.OnMatchStarted -= OnMatchStarted;
            MatchManager.OnScoreAmountChanged -= OnScoreAmountChanged;
            _returnButton.onClick.RemoveListener(ReturnToMenu);
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
        }

        private void OnScoreAmountChanged(int score) => _score.text = $"Score: {score}";

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

        private void ReturnToMenu()
        {
            var currentScene = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(currentScene);
        }

        private void OnDisable()
        {
            Unsubscribe();
            this.StopCoroutine(ref _countdownRoutine);
        }
    }
}
