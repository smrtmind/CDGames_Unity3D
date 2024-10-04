using UnityEngine;
using static Scripts.Utils.Enums;

namespace Scripts.Objects
{
    public class CollectableItem : MonoBehaviour
    {
        [field: SerializeField] public CollectableItemType Type { get; private set; }
        [field: SerializeField, Min(1)] public int Value { get; private set; } = 1;
    }
}
