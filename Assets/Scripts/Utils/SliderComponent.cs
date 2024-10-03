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

        private GameSession _gameSession;

        private float _time;
        private float _nextBoardPosition;
        private float _nextBoardRotation;
        private bool _sliderIsTouching;
        private bool _spawnBoards;

        public bool SpawnBoards
        {
            get => _spawnBoards;
            set => _spawnBoards = value;
        }

        public Slider slider => _slider;
        public bool SliderIsTouching => _sliderIsTouching;

        [Inject]
        private void Construct(GameSession gameSession)
        {
            _gameSession = gameSession;
        }

        private void Start()
        {
            Application.targetFrameRate = 60;
        }

        private void Update()
        {
            if (_spawnBoards && _gameSession.Boards > 0)
            {
                _time += Time.deltaTime;

                if (_time > _spawnDelay)
                {
                    _time = 0;
                    _gameSession.RemoveBoard();

                    Transform board = Instantiate(_boardPrefab, new Vector3(0, _nextBoardPosition), Quaternion.Euler(_nextBoardRotation, 0, 0)).transform;

                    _nextBoardPosition = Mathf.Lerp(board.position.y, board.position.y + _slider.value, Time.deltaTime * _smoothPower);
                    _nextBoardRotation = Mathf.Lerp(board.rotation.x, -_slider.value * _rotateMultiplier, Time.deltaTime * _smoothPower);
                }
            }
            else
                _nextBoardPosition = 0f;
        }

        public void SliderIsActive(bool state) => _sliderIsTouching = state;
    }
}
