using Scripts.Objects.Items;
using System;
using UnityEngine;

namespace Scripts.Spawners
{
    [Serializable]
    public class ItemData
    {
        [field: SerializeField] public Item Item { get; private set; }
        [field: SerializeField, Range(0f, 100f)] public float ChanceToSpawn { get; private set; }
    }
}
