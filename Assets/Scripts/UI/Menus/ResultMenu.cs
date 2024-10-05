using Scripts.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

namespace Scripts.UI.Menus
{
    public class ResultMenu : MonoBehaviour
    {
        [SerializeField] private Button _returnButton;
        [SerializeField] private Button _exitButton;
        [SerializeField] private TMP_Text _title;
        [SerializeField] private TMP_Text _timer;

        private GameplayUi _gameplayUi;

        [Inject]
        private void Construct(GameplayUi gameplayUi)
        {
            _gameplayUi = gameplayUi;
        }

        private void OnEnable()
        {
            Subscribe();
        }

        private void Subscribe()
        {
            GameManager.OnAfterStateChanged += OnAfterStateChanged;
            _returnButton.onClick.AddListener(ReturnToMenu);
            _exitButton.onClick.AddListener(ExitGame);
        }

        private void Unsubscribe()
        {
            GameManager.OnAfterStateChanged -= OnAfterStateChanged;
            _returnButton.onClick.RemoveListener(ReturnToMenu);
            _exitButton.onClick.RemoveListener(ExitGame);
        }

        private void OnAfterStateChanged(GameState state)
        {
            if (state == GameState.Victory || state == GameState.Defeat)
            {
                _title.text = state == GameState.Victory ? "Victory" : "Defeat";
                _title.color = state == GameState.Victory ? Color.green : Color.red;

                _timer.text = $"Time: {_gameplayUi.FormattedTimer}";
            }
        }

        private void ReturnToMenu()
        {
            var currentScene = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(currentScene);
        }

        private void ExitGame() => Application.Quit();

        private void OnDisable()
        {
            Unsubscribe();
        }
    }
}
