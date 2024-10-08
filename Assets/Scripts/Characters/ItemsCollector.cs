using Scripts.Objects.Items;
using UnityEngine;

namespace Scripts.Characters
{
    public class ItemsCollector : MonoBehaviour
    {
        [SerializeField] private LayerMask _target;

        private void OnTriggerEnter(Collider other)
        {
            if ((_target & (1 << other.gameObject.layer)) != 0)
            {
                other.TryGetComponent(out CollectableItem item);
                if (item != null)
                    item.Collect();
            }
        }
    }
}
