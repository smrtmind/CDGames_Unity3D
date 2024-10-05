using Scripts.Objects;
using System.Linq;
using UnityEngine;

namespace Scripts.Spawners
{
    [CreateAssetMenu(fileName = nameof(SectionsStorage), menuName = "ScriptableObjects/" + nameof(SectionsStorage))]
    public class SectionsStorage : ScriptableObject
    {
        [SerializeField] private Section[] _sections;

        public float GetSectionLengthByZ() => _sections.First().Collider.size.z;

        public Section GetRandomSectionPrefab() => _sections[Random.Range(0, _sections.Length)];
    }
}
