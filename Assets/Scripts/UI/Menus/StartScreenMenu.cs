using Scripts.Managers;
using Scripts.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Scripts.UI.Menus
{
    public class StartScreenMenu : MonoBehaviour
    {
        [SerializeField] private TMP_Text _bestScore;
        [SerializeField] private Button _startButton;
        [SerializeField] private Button _exitButton;

        private AudioManager _audioManager;
        private GameManager _gameManager;
        private SaveManager _saveManager;

        [Inject]
        private void Construct(AudioManager audioManager, GameManager gameManager, SaveManager saveManager)
        {
            _audioManager = audioManager;
            _gameManager = gameManager;
            _saveManager = saveManager;
        }

        private void OnEnable()
        {
            Subscribe();
            this.WaitEndOfFrame(RefreshBestScore);
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

        private void RefreshBestScore() => _bestScore.text = $"Best score: {_saveManager.BestScore}";

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
