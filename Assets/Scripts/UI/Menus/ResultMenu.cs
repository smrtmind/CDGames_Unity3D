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
        [SerializeField] private TMP_Text _score;

        private MatchManager _matchManager;

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
            if (state != GameState.GameOver) return;

            _score.text = $"Score: {_matchManager.Score}";
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
