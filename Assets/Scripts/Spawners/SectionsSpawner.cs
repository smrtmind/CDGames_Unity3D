using Scripts.Characters;
using Scripts.Managers;
using Scripts.Objects;
using Scripts.Pooling;
using Scripts.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Scripts.Spawners
{
    public class SectionsSpawner : MonoBehaviour
    {
        #region Variables
        [Header("Storages")]
        [SerializeField] private SectionsStorage _sectionsStorage;

        [Header("Parameters")]
        [SerializeField] private float _spawnHeight = 10f;
        [SerializeField, Min(0.1f)] private float _distanceAheadOfPlayer = 30f;
        [SerializeField, Min(1)] private int _spawnPillarSkipCounter = 5;

        [Space]
        [SerializeField, Min(1)] private int _platformsBeforeSkipMin = 1;
        [SerializeField, Min(2)] private int _platformsBeforeSkipMax = 10;

        [Space]
        [SerializeField, Min(1)] private int _skipCountMin = 1;
        [SerializeField, Min(2)] private int _skipCountMax = 10;

        public static Action<Section> OnSectionSpawned;

        private HashSet<Section> _activeSections = new();
        private ObjectPool _objectPool;
        private Player _player;
        private Section _lastSpawnedSection;
        private Coroutine _spawnRoutine;
        private WaitForEndOfFrame _waitForEndOfFrame = new();

        private int _platformSpawnedCount = 0;
        private int _platformsWithoutPillarCounter = 0;
        #endregion

        [Inject]
        private void Construct(ObjectPool objectPool, Player player)
        {
            _objectPool = objectPool;
            _player = player;
        }

        private void OnEnable()
        {
            Subscribe();
        }

        private void Subscribe()
        {
            GameManager.OnAfterStateChanged += OnAfterStateChanged;
            MatchManager.OnMatchEnded += StopSpawn;
            Player.OnPlayerLost += StopSpawn;
        }

        private void Unsubscribe()
        {
            GameManager.OnAfterStateChanged -= OnAfterStateChanged;
            MatchManager.OnMatchEnded -= StopSpawn;
            Player.OnPlayerLost -= StopSpawn;
        }

        private void OnAfterStateChanged(GameState state)
        {
            switch (state)
            {
                case GameState.Gameplay:
                    StartSpawn();
                    break;

                case GameState.Victory:
                case GameState.Defeat:
                    StopSpawn();
                    break;
            }
        }

        private void StartSpawn()
        {
            if (_spawnRoutine == null)
                _spawnRoutine = StartCoroutine(SpawnLoop());
        }

        private IEnumerator SpawnLoop()
        {
            _lastSpawnedSection = _objectPool.Get(_sectionsStorage.GetSectionWithPillar());
            _lastSpawnedSection.transform.position = Vector3.zero;

            while (true)
            {
                if (ShouldSpawnNewSection())
                    SpawnNewSection();

                yield return _waitForEndOfFrame;
            }
        }

        private bool ShouldSpawnNewSection()
        {
            var distanceToLastSection = _lastSpawnedSection.transform.position.z - _player.transform.position.z;
            return distanceToLastSection <= _distanceAheadOfPlayer;
        }

        private void SpawnNewSection()
        {
            if (_platformSpawnedCount >= Random.Range(_platformsBeforeSkipMin, _platformsBeforeSkipMax))
            {
                SkipSections(Random.Range(_skipCountMin, _skipCountMax));
                _platformSpawnedCount = 0;
            }

            _platformsWithoutPillarCounter++;
            var currentSectionType = _platformsWithoutPillarCounter < _spawnPillarSkipCounter ? 
                _sectionsStorage.GetSection() : 
                _sectionsStorage.GetSectionWithPillar();

            if (_platformsWithoutPillarCounter >= _spawnPillarSkipCounter)
                _platformsWithoutPillarCounter = 0;

            var section = _objectPool.Get(currentSectionType);

            float newPositionZ = (_lastSpawnedSection != null)
                ? _lastSpawnedSection.transform.position.z + _sectionsStorage.GetSectionLengthByZ()
                : 0f;

            section.transform.position = new Vector3(0f, Random.value > 0.5f ? _spawnHeight : -_spawnHeight, newPositionZ);

            _activeSections.Add(section);

            _lastSpawnedSection = section;
            _platformSpawnedCount++;

            OnSectionSpawned?.Invoke(_lastSpawnedSection);
        }

        private void SkipSections(int skipCount)
        {
            if (_lastSpawnedSection != null)
            {
                float skipDistance = _sectionsStorage.GetSectionLengthByZ() * skipCount;
                float newPositionZ = _lastSpawnedSection.transform.position.z + skipDistance;

                _lastSpawnedSection.transform.position = new Vector3(
                    _lastSpawnedSection.transform.position.x,
                    _lastSpawnedSection.transform.position.y,
                    newPositionZ);
            }
        }

        private void StopSpawn() => this.StopCoroutine(ref _spawnRoutine);

        private void ReleaseAllSections()
        {
            foreach (var section in _activeSections)
            {
                if (section != null)
                    section.Release();
            }

            _activeSections.Clear();
        }

        private void OnDisable()
        {
            Unsubscribe();
            StopSpawn();
            ReleaseAllSections();
        }
    }
}
