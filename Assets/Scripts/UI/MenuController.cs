using Scripts.Service;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts.Hud
{
    public class MenuController : MonoBehaviour
    {
        private AudioComponent _audio;

        private void Awake()
        {
            _audio = FindObjectOfType<AudioComponent>();
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
            _audio.PlaySfx("button");
        }
    }
}
