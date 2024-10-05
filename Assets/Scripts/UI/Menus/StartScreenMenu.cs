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
        private GameManager _gameManager;

        [Inject]
        private void Construct(AudioManager audioManager, GameManager gameManager)
        {
            _audioManager = audioManager;
            _gameManager = gameManager;
        }

        private void OnEnable()
        {
            Subscribe();
        }

        private void Subscribe()
        {
            _startButton.onClick.AddListener(StartGame);
            _exitButton.onClick.AddListener(ExitGame);
        }

        private void Unsubscribe()
        {
            _startButton.onClick.RemoveListener(StartGame);
            _exitButton.onClick.RemoveListener(ExitGame);
        }

        private void StartGame()
        {
            _gameManager.ChangeState(GameState.Gameplay);
            _audioManager.PlaySfx("button");

            DisableButtonsAvailability();
        }

        private void ExitGame() => Application.Quit();

        private void DisableButtonsAvailability()
        {
            _startButton.image.raycastTarget = false;
            _exitButton.image.raycastTarget = false;
        }

        private void OnDisable()
        {
            Unsubscribe();
        }
    }
}
