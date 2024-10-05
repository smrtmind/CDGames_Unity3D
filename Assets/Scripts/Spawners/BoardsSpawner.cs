using Scripts.Managers;
using Scripts.Objects;
using Scripts.Pooling;
using Scripts.UI;
using UnityEngine;
using Zenject;

namespace Scripts.Spawners
{
    public class BoardsSpawner : MonoBehaviour
    {
        #region Variables
        [Header("Components")]
        [SerializeField] private Board _board;

        [Header("Parameters")]
        [SerializeField] private float _spawnDelay;
        [SerializeField] private float _rotateMultiplier;
        [SerializeField] private float _smoothPower;

        private ObjectPool _objectPool;
        private MatchManager _matchManager;
        private BoardControlSlider _boardControlSlider;

        private float _time;
        private float _nextBoardPosition;
        private float _nextBoardRotation;
        private bool _canSpawn;
        #endregion

        [Inject]
        private void Construct(ObjectPool objectPool, MatchManager matchManager, BoardControlSlider boardControlSlider)
        {
            _objectPool = objectPool;
            _matchManager = matchManager;
            _boardControlSlider = boardControlSlider;
        }

        private void OnEnable()
        {
            Subscribe();
        }

        private void Subscribe()
        {
            BoardControlSlider.OnTouched += StartSpawn;
            BoardControlSlider.OnReleased += StopSpawn;
        }

        private void Unsubscribe()
        {
            BoardControlSlider.OnTouched -= StartSpawn;
            BoardControlSlider.OnReleased -= StopSpawn;
        }

        private void StartSpawn() => _canSpawn = true;

        private void StopSpawn() => _canSpawn = false;

        private void Update()
        {
            if (_canSpawn && _matchManager.Boards > 0)
            {
                _time += Time.deltaTime;

                if (_time > _spawnDelay)
                {
                    _time = 0;

                    _matchManager.RemoveBoard();

                    var boardTransform = GetNewBoard();
                    var sliderValue = _boardControlSlider.GetValue();

                    _nextBoardPosition = Mathf.Lerp(boardTransform.position.y, boardTransform.position.y + sliderValue, Time.deltaTime * _smoothPower);
                    _nextBoardRotation = Mathf.Lerp(boardTransform.rotation.x, -sliderValue * _rotateMultiplier, Time.deltaTime * _smoothPower);
                }
            }
            else
            {
                _nextBoardPosition = 0f;
            }
        }

        private Transform GetNewBoard()
        {
            var board = _objectPool.Get(_board);
            board.transform.position = new Vector3(0, _nextBoardPosition, transform.position.z);
            board.transform.rotation = Quaternion.Euler(_nextBoardRotation, 0f, 0f);

            return board.transform;
        }

        private void OnDisable()
        {
            Unsubscribe();
        }
    }
}
