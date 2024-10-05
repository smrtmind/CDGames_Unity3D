using Scripts.Managers;
using Scripts.Objects;
using Scripts.Pooling;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Scripts.Spawners
{
    public class ItemsSpawner : MonoBehaviour
    {
        [SerializeField] private ItemData[] _itemDatas;

        private ObjectPool _objectPool;
        private MatchManager _matchManager;

        [Inject]
        private void Construct(ObjectPool objectPool, MatchManager matchManager)
        {
            _objectPool = objectPool;
            _matchManager = matchManager;
        }

        private void OnEnable()
        {
            SectionsSpawner.OnSectionSpawned += SpawnRandomItem;
        }

        private void OnDisable()
        {
            SectionsSpawner.OnSectionSpawned -= SpawnRandomItem;
        }

        private void SpawnRandomItem(Section section)
        {
            if (!_matchManager.IsStarted) return;

            var itemPrefab = GenerateRandomItemPrefab();
            if (itemPrefab != null)
            {
                var item = _objectPool.Get(itemPrefab);
                item.transform.SetParent(section.SpawnPoint);
                item.transform.position = section.SpawnPoint.position;
            }
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
