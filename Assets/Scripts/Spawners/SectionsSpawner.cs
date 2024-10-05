using Scripts.Characters;
using Scripts.Managers;
using Scripts.Objects;
using Scripts.Pooling;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Scripts.Utils;

namespace Scripts.Spawners
{
    public class SectionsSpawner : MonoBehaviour
    {
        [Header("Storages")]
        [SerializeField] private SectionsStorage _sectionsStorage;

        [Header("Parameters")]
        [SerializeField] private float _spawnHeight = 10f;

        [Header("Gap Settings")]
        [SerializeField, Min(1)] private int _platformsBeforeSkip = 3;
        [SerializeField, Min(0)] private int _skipCount = 1;

        [Header("Spawn Distance Settings")]
        [SerializeField, Min(0.1f)] private float _distanceAheadOfPlayer = 30f;

        private HashSet<Section> _activeSections = new();
        private ObjectPool _objectPool;
        private Player _player;
        private Section _lastSpawnedSection;
        private Coroutine _spawnRoutine;
        private WaitForEndOfFrame _waitForEndOfFrame = new();

        private int _platformSpawnedCount = 0;

        [Inject]
        private void Construct(ObjectPool objectPool, Player player)
        {
            _objectPool = objectPool;
            _player = player;
        }

        private void OnEnable()
        {
            GameManager.OnAfterStateChanged += OnAfterStateChanged;
        }

        private void OnDisable()
        {
            GameManager.OnAfterStateChanged -= OnAfterStateChanged;

            StopSpawn();
            ReleaseAllSections();
        }

        private void OnAfterStateChanged(GameState state)
        {
            switch (state)
            {
                case GameState.Gameplay:
                    if (_spawnRoutine == null)
                        _spawnRoutine = StartCoroutine(SpawnLoop());
                    break;

                case GameState.Victory:
                case GameState.Defeat:
                    StopSpawn();
                    break;
            }
        }

        private IEnumerator SpawnLoop()
        {
            while (true)
            {
                if (ShouldSpawnNewSection())
                    SpawnNewSection();

                yield return _waitForEndOfFrame;
            }
        }

        private bool ShouldSpawnNewSection()
        {
            if (_lastSpawnedSection == null)
                return true;

            var distanceToLastSection = _lastSpawnedSection.transform.position.z - _player.transform.position.z;

            return distanceToLastSection <= _distanceAheadOfPlayer;
        }

        private void SpawnNewSection()
        {
            if (_platformSpawnedCount >= _platformsBeforeSkip)
            {
                SkipSections(_skipCount);
                _platformSpawnedCount = 0;
            }

            var section = _objectPool.Get(_sectionsStorage.GetRandomSectionPrefab());

            float newPositionZ = (_lastSpawnedSection != null)
                ? _lastSpawnedSection.transform.position.z + _sectionsStorage.GetSectionLengthByZ()
                : 0f;

            section.transform.position = new Vector3(0f, _spawnHeight, newPositionZ);
            section.transform.rotation = Quaternion.identity;

            _activeSections.Add(section);

            _lastSpawnedSection = section;
            _platformSpawnedCount++;
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
    }
}
