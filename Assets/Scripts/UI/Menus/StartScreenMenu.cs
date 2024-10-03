using Scripts.Managers;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Scripts.UI.Menus
{
    public class StartScreenMenu : MonoBehaviour
    {
        [SerializeField] private Button _startButton;
        [SerializeField] private Button _exitButton;

        private AudioManager _audioManager;

        [Inject]
        private void Construct(AudioManager audioManager)
        {
            _audioManager = audioManager;
        }

        private void OnEnable()
        {
            _startButton.onClick.AddListener(StartGame);
            _exitButton.onClick.AddListener(ExitGame);
        }

        private void OnDisable()
        {
            _startButton.onClick.RemoveListener(StartGame);
            _exitButton.onClick.RemoveListener(ExitGame);
        }

        private void StartGame()
        {
            GameManager.Instance.ChangeState(GameState.Gameplay);
            _audioManager.PlaySfx("button");
        }

        private void ExitGame() => Application.Quit();
    }
}
