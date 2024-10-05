using Scripts.Objects;
using Scripts.Pooling;
using System;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Scripts.Spawners
{
    public class ItemsSpawner : MonoBehaviour
    {
        [SerializeField] private ItemData[] _itemDatas;

        public static Func<Item> OnItemRequested;

        private ObjectPool _objectPool;

        [Inject]
        private void Construct(ObjectPool objectPool)
        {
            _objectPool = objectPool;
        }

        private void OnEnable()
        {
            OnItemRequested += SpawnRandomItem;
        }

        private void OnDisable()
        {
            OnItemRequested -= SpawnRandomItem;
        }

        private Item SpawnRandomItem()
        {
            var item = GenerateRandomItemPrefab();
            return item != null ? _objectPool.Get(item) : null;
        }

        private Item GenerateRandomItemPrefab()
        {
            float randomValue = Random.value;
            float cumulativeProbability = 0f;

            foreach (var itemData in _itemDatas)
            {
                cumulativeProbability = (cumulativeProbability + itemData.ChanceToSpawn / 100f);

                if (randomValue <= cumulativeProbability)
                    return itemData.Item;
            }

            return null;
        }
    }
}
