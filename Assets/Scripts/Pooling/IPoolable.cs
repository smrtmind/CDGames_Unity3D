using System;
using UnityEngine;

namespace Scripts.Pooling
{
    public interface IPoolable
    {
        public event Action<IPoolable> Destroyed;

        public GameObject GameObject { get; }

        public void Release();
    }
}
