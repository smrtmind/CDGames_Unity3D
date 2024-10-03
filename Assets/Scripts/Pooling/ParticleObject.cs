using Scripts.Utils;
using System;
using UnityEngine;

namespace Scripts.Pooling
{
    public class ParticleObject : MonoBehaviour, IPoolable
    {
        [SerializeField] private ParticleSystem _particleSystem;

        public event Action<IPoolable> Destroyed;

        public GameObject GameObject => gameObject;

        private void OnEnable()
        {
            this.WaitForSecondsUnclamped(_particleSystem.main.duration, Release);
        }

        public void Release() => Destroyed?.Invoke(this);
    }
}
