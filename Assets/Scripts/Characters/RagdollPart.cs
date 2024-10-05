using UnityEngine;

namespace Scripts.Characters
{
    public class RagdollPart : MonoBehaviour
    {
        public Rigidbody Rigidbody { get; private set; }
        public Collider Collider { get; private set; }

        private void Awake()
        {
            Rigidbody = GetComponent<Rigidbody>();
            Collider = GetComponent<Collider>();
        }
    }
}
