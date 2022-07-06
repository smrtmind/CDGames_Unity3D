using UnityEngine;

namespace Scripts.Utils
{
    public class SpawnComponent : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private GameObject _prefab;

        [ContextMenu("Spawn")]
        public void Spawn()
        {
            Instantiate(_prefab, _target.position, Quaternion.identity);
        }
    }
}