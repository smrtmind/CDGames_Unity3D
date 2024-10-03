using Scripts.Managers;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Scripts.Utils
{
    public class SliderComponent : MonoBehaviour
    {
        [SerializeField] private GameObject _boardPrefab;
        [SerializeField] private Slider _slider;
        [SerializeField] private float _spawnDelay;
        [SerializeField] private float _rotateMultiplier;
        [SerializeField] private float _smoothPower;



        private bool _sliderIsTouching;
        private bool _spawnBoards;

        public bool SpawnBoards
        {
            get => _spawnBoards;
            set => _spawnBoards = value;
        }

        public Slider slider => _slider;
        public bool SliderIsTouching => _sliderIsTouching;



        private void Start()
        {
            Application.targetFrameRate = 60;
        }



        public void SliderIsActive(bool state) => _sliderIsTouching = state;
    }
}
