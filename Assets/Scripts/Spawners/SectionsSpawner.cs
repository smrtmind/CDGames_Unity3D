using Scripts.Managers;
using Scripts.Objects;
using Scripts.Pooling;
using Scripts.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Scripts.Spawners
{
    public class SectionsSpawner : MonoBehaviour
    {
        [Header("Storages")]
        [SerializeField] private SectionsStorage _sectionsStorage;

        [Header("Parameters")]
        [SerializeField, Min(1f)] private float _sectionLength = 40f;
        [SerializeField, Min(0.1f)] private float _spawnDelay = 3f;

        private HashSet<Section> _activeSections = new();
        private ObjectPool _objectPool;
        private Coroutine _spawnRoutine;
        private WaitForSeconds _waitForSpawnDelay;

        private float _positionByZ;

        [Inject]
        private void Construct(ObjectPool objectPool)
        {
            _objectPool = objectPool;
        }

        private void Awake()
        {
            _waitForSpawnDelay = new WaitForSeconds(_spawnDelay);
        }

        private void OnEnable()
        {
            GameManager.OnAfterStateChanged += OnAfterStateChanged;
        }

        private void OnDisable()
        {
            GameManager.OnAfterStateChanged -= OnAfterStateChanged;

            this.StopCoroutine(ref _spawnRoutine);
            ReleaseAllSections();
        }

        private void OnAfterStateChanged(GameState state)
        {
            if (state != GameState.Gameplay) return;

            if (_spawnRoutine == null)
                _spawnRoutine = StartCoroutine(SpawnLoop());
        }

        private IEnumerator SpawnLoop()
        {
            while (true)
            {
                SpawnNewSection();
                _positionByZ += _sectionLength;

                yield return _waitForSpawnDelay;
            }
        }

        private void SpawnNewSection()
        {
            var section = _objectPool.Get(_sectionsStorage.GetRandomSectionPrefab());
            section.transform.position = new Vector3(0f, 0f, _positionByZ);
            section.transform.rotation = Quaternion.identity;

            _activeSections.Add(section);
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
    }
}
