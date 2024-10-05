using Scripts.Pooling;
using System;
using UnityEngine;
using static Scripts.Utils.Enums;

namespace Scripts.Objects
{
    public abstract class Item : MonoBehaviour, IPoolable
    {
        [field: SerializeField] public ItemType Type { get; private set; }

        public event Action<IPoolable> Destroyed;

        public GameObject GameObject => gameObject;

        public void Release() => Destroyed?.Invoke(this);
    }
}
