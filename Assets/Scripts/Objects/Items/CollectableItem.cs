using Scripts.Managers;
using UnityEngine;
using Zenject;

namespace Scripts.Objects.Items
{
    public abstract class CollectableItem : Item
    {
        [field: SerializeField, Min(1)] public int Value { get; private set; } = 30;

        protected AudioManager _audioManager;
        protected MatchManager _matchManager;

        [Inject]
        private void Construct(AudioManager audioManager, MatchManager matchManager)
        {
            _audioManager = audioManager;
            _matchManager = matchManager;
        }

        public abstract void Collect();
    }
}
