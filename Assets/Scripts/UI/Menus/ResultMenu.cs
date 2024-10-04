using Scripts.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Scripts.UI.Menus
{
    public class ResultMenu : MonoBehaviour
    {
        [SerializeField] private Button _replayButton;
        [SerializeField] private Button _exitButton;
        [SerializeField] private TMP_Text _title;

        private void OnEnable()
        {
            Subscribe();
        }

        private void Subscribe()
        {
            GameManager.OnAfterStateChanged += OnAfterStateChanged;
            _replayButton.onClick.AddListener(ReplayGame);
            _exitButton.onClick.AddListener(ExitGame);
        }

        private void Unsubscribe()
        {
            GameManager.OnAfterStateChanged -= OnAfterStateChanged;
            _replayButton.onClick.RemoveListener(ReplayGame);
            _exitButton.onClick.RemoveListener(ExitGame);
        }

        private void OnAfterStateChanged(GameState state)
        {
            switch (state)
            {
                case GameState.Victory:
                    _title.text = "Victory";
                    _title.color = Color.green;
                    break;

                case GameState.Defeat:
                    _title.text = "Defeat";
                    _title.color = Color.red;
                    break;
            }
        }

        private void ReplayGame()
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
