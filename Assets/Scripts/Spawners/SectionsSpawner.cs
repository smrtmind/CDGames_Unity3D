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
        [Header("Components")]
        [SerializeField] private Section _section;
        [SerializeField] private Section _sectionWithPillar;
        [SerializeField] private Section _sectionSide;
        [SerializeField] private Section _sectionSideWithPillar;

        [Header("Parameters")]
        [SerializeField] private float _spawnDistance = 10f;
        [SerializeField, Min(0.1f)] private float _distanceAheadOfPlayer = 30f;
        [SerializeField, Min(1)] private int _spawnPillarSkipCounter = 5;
        [SerializeField, Range(0f, 100f)] private float _skipChance = 40f;

        [Space]
        [SerializeField, Min(2)] private int _sideSectionsMin;
        [SerializeField, Min(2)] private int _sideSectionsMax;

        [Space]
        [SerializeField, Min(2)] private int _frontSectionsMin;
        [SerializeField, Min(2)] private int _frontSectionsMax;

        private HashSet<Section> _activeSections = new();
        private ObjectPool _objectPool;
        private Player _player;
        private Section _lastSpawnedSection;
        private Coroutine _spawnRoutine;
        private WaitForEndOfFrame _waitForEndOfFrame = new();

        private int _platformSpawnedCount = 0;
        private float _accumulatedSkipDistance;
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
            SpawnFirstSection();
        }

        private void Subscribe()
        {
            GameManager.OnAfterStateChanged += OnAfterStateChanged;
            Player.OnPlayerLost += StopSpawn;
        }

        private void Unsubscribe()
        {
            GameManager.OnAfterStateChanged -= OnAfterStateChanged;
            Player.OnPlayerLost -= StopSpawn;
        }

        private void OnAfterStateChanged(GameState state)
        {
            switch (state)
            {
                case GameState.Gameplay:
                    StartSpawn();
                    break;

                case GameState.GameOver:
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
            if (Random.value > _skipChance / 100f)
            {
                _platformSpawnedCount++;

                var currentSection = _platformSpawnedCount < _spawnPillarSkipCounter ? _section : _sectionWithPillar;

                if (_platformSpawnedCount >= _spawnPillarSkipCounter)
                    _platformSpawnedCount = 0;

                var newPosisitonByZ = _lastSpawnedSection.transform.position.z + _lastSpawnedSection.Size.z + _accumulatedSkipDistance;
                _accumulatedSkipDistance = 0f;

                _lastSpawnedSection = GetNewSection(currentSection);
                _lastSpawnedSection.transform.position = new Vector3(0f, Random.value > 0.5f ? _spawnDistance : -_spawnDistance, newPosisitonByZ);

                if (currentSection == _sectionWithPillar)
                    SpawnSideSections(newPosisitonByZ, _section.Size.x, Random.Range(_sideSectionsMin, _sideSectionsMax));
            }
            else
            {
                _accumulatedSkipDistance += _section.Size.z;
            }
        }

        private void SpawnSideSections(float basePositionZ, float offsetX, int sideSections)
        {
            float lastLeftPositionX = 0f;
            float lastRightPositionX = 0f;

            for (int i = 1; i <= sideSections + (sideSections / 2); i++)
            {
                var section = GetNewSection(GetRandomSideSection());
                var side = i <= sideSections ? -1 : 1;
                var positionX = offsetX * (i <= sideSections ? i : i - sideSections);

                section.transform.position = new Vector3(side * positionX, Random.value > 0.5f ? _spawnDistance : -_spawnDistance, basePositionZ);

                if (side == -1)
                    lastLeftPositionX = side * positionX;
                else
                    lastRightPositionX = side * positionX;
            }

            var randomFrontSectionsAmount = Random.Range(_frontSectionsMin, _frontSectionsMax);
            for (int j = 1; j <= randomFrontSectionsAmount; j++)
            {
                SpawnNewSectionInFront(lastLeftPositionX, basePositionZ, j);
                SpawnNewSectionInFront(lastRightPositionX, basePositionZ, j);
            }
        }

        private void SpawnNewSectionInFront(float lastPositionX, float basePositionZ, int offset)
        {
            var section = GetNewSection(GetRandomSideSection());
            var newPositionZ = basePositionZ + offset * _section.Size.z;

            section.transform.position = new Vector3(lastPositionX, Random.value > 0.5f ? _spawnDistance : -_spawnDistance, newPositionZ);
        }

        private void StopSpawn() => this.StopCoroutine(ref _spawnRoutine);

        private Section GetNewSection(Section sectionPrefab)
        {
            var section = _objectPool.Get(sectionPrefab);
            _activeSections.Add(_section);

            return section;
        }

        private void SpawnFirstSection()
        {
            _lastSpawnedSection = GetNewSection(_sectionWithPillar);
            _lastSpawnedSection.transform.position = Vector3.zero;
        }

        private void ReleaseAllSections()
        {
            foreach (var section in _activeSections)
            {
                if (section != null)
                    section.Release();
            }

            _activeSections.Clear();
        }

        private Section GetRandomSideSection() => Random.value > 0.1f ? _sectionSide : _sectionSideWithPillar;

        private void OnDisable()
        {
            Unsubscribe();
            StopSpawn();
            ReleaseAllSections();
        }
    }
}
