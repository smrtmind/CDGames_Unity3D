using Scripts.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Scripts.UI
{
    public class UiController : MonoBehaviour
    {
        [SerializeField] private UiElement _mainMenu;
        [SerializeField] private UiElement _gameplayUi;
        [SerializeField] private UiElement _victoryScreen;
        [SerializeField] private UiElement _defeatScreen;

        private AudioManager _audioManager;

        [Inject]
        private void Construct(AudioManager audioManager)
        {
            _audioManager = audioManager;
        }

        public void OnStart()
        {
            SceneManager.LoadScene("Level");
        }

        public void OnReplay()
        {
            var scene = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(scene);
        }

        public void OnExit()
        {
            Application.Quit();
        }

        public void OnButtonPressed()
        {
            _audioManager.PlaySfx("button");
        }
    }
}
