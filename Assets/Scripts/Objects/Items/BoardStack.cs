using Scripts.Pooling;
using Scripts.Utils;
using System;
using System.Collections;
using UnityEngine;
using Zenject;

namespace Scripts.Objects.Items
{
    public class BoardStack : Item
    {
        [SerializeField] private CollectableBoard _boardPrefab;
        [SerializeField, Min(1)] private int _amountOfBoards = 3;
        [SerializeField, Min(0.1f)] private float _delayBetweenSpawn = 0.1f;

        private ObjectPool _objectPool;
        private Coroutine _spawnRoutine;
        private WaitForSeconds _waitForDelayBetweenSpawn;

        [Inject]
        private void Construct(ObjectPool objectPool)
        {
            _objectPool = objectPool;
        }

        private void Awake()
        {
            _waitForDelayBetweenSpawn = new WaitForSeconds(_delayBetweenSpawn);
        }

        public void Collect()
        {
            if (_spawnRoutine == null)
                _spawnRoutine = StartCoroutine(StartSpawn(Release));
        }

        private IEnumerator StartSpawn(Action onComplete)
        {
            for (int i = 0; i < _amountOfBoards; i++)
            {
                var board = _objectPool.Get(_boardPrefab);
                board.transform.position = transform.position;
                board.Collect();

                yield return _waitForDelayBetweenSpawn;
            }

            onComplete?.Invoke();
        }

        private void OnDisable()
        {
            this.StopCoroutine(ref _spawnRoutine);
        }
    }
}
