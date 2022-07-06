using Scripts.Player;
using System.Collections;
using UnityEngine;

namespace Scripts.Environment
{
    public class LevelComponent : MonoBehaviour
    {
        [SerializeField] private GameObject[] _sections;
        [SerializeField] private float _sectionLong = 40f;
        [SerializeField] private float _spawnDelay = 3f;
        [SerializeField] private PlayerController _player;

        private float _zPosition;
        private bool _creatingSection = false;

        private void Start()
        {
            //spawn first random section on start in the middle of the scene, to make every start of game unique
            Instantiate(_sections[GetRandomIndex()], Vector3.zero, Quaternion.identity);
        }

        private void Update()
        {
            if (!_creatingSection)
            {
                _creatingSection = true;

                _zPosition += _sectionLong;
                StartCoroutine(SpawnSection(_zPosition));
            }
        }

        private IEnumerator SpawnSection(float positionZ)
        {
            //choose random section from the array of ready sections
            Instantiate(_sections[GetRandomIndex()], new Vector3(0f, 0f, positionZ), Quaternion.identity);

            yield return new WaitForSeconds(_spawnDelay);

            _creatingSection = false;
        }

        private int GetRandomIndex() => Random.Range(0, _sections.Length);
    }
}
