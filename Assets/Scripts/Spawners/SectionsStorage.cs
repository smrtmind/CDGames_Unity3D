using Scripts.Objects;
using UnityEngine;

namespace Scripts.Spawners
{
    [CreateAssetMenu(fileName = nameof(SectionsStorage), menuName = "ScriptableObjects/" + nameof(SectionsStorage))]
    public class SectionsStorage : ScriptableObject
    {
        [SerializeField] private Section _section;
        [SerializeField] private Section _sectionWithPillar;

        public float GetSectionLengthByZ() => _section.SizeZ;

        public Section GetSection() => _section;

        public Section GetSectionWithPillar() => _sectionWithPillar;
    }
}
