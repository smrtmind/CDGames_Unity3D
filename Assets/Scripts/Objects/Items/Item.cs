using Scripts.Pooling;
using System;
using UnityEngine;

namespace Scripts.Objects.Items
{
    public abstract class Item : MonoBehaviour, IPoolable
    {
        public event Action<IPoolable> Destroyed;

        public GameObject GameObject => gameObject;

        public void Release() => Destroyed?.Invoke(this);
    }
}
