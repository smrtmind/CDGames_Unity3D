using UnityEngine;

namespace Scripts.Objects
{
    public class CollectableItem : Item
    {
        [field: SerializeField, Min(1)] public int Value { get; private set; } = 1;
    }
}
