using Scripts.Characters;
using System.Collections;
using UnityEngine;
using Zenject;
using Scripts.Utils;
using Scripts.Objects;
using System.Collections.Generic;
using Scripts.Pooling;

namespace Scripts.Service
{
    public class SectionsSpawner : MonoBehaviour
    {
        [SerializeField] private Section[] _sections;
        [SerializeField, Min(1f)] private float _sectionLength = 40f;
        [SerializeField, Min(0.1f)] private float _spawnDelay = 3f;

        private HashSet<Section> _activeSections = new();
        private PlayerController _player;
        private Coroutine _spawnRoutine;
        private WaitForSeconds _waitForSpawnDelay;
        private ObjectPool _objectPool;

        private float _positionByZ;
        private bool _creatingSection = false;

        [Inject]
        private void Construct(PlayerController player, ObjectPool objectPool)
        {
            _player = player;
            _objectPool = objectPool;
        }

        private void Awake()
        {
            _waitForSpawnDelay = new WaitForSeconds(_spawnDelay);
        }

        private void OnEnable()
        {
            if (_spawnRoutine == null)
                _spawnRoutine = StartCoroutine(SpawnLoop());
        }

        private IEnumerator SpawnLoop()
        {
            while (true)
            {
                _positionByZ += _sectionLength;
                SpawnNewSection();

                yield return _waitForSpawnDelay;
            }
        }

        //private void Update()
        //{
        //    if (!_player.IsDead)
        //    {
        //        if (!_creatingSection)
        //        {
        //            _creatingSection = true;

        //            _positionByZ += _sectionLength;
        //        }
        //    }
        //}

        private void SpawnNewSection()
        {
            var randomSection = _sections[Random.Range(0, _sections.Length)];
            var section = _objectPool.Get(randomSection);
            section.transform.position = new Vector3(0f, 0f, _positionByZ);
            section.transform.rotation = Quaternion.identity;

            ////choose random section from the array of ready sections
            //Instantiate(_sections[GetRandomIndex()], new Vector3(0f, 0f, _positionByZ), Quaternion.identity);

            //yield return new WaitForSeconds(_spawnDelay);

            //_creatingSection = false;
        }

        //private int GetRandomIndex() => Random.Range(0, _sections.Length);

        private void OnDisable()
        {
            this.StopCoroutine(ref _spawnRoutine);
        }
    }
}
