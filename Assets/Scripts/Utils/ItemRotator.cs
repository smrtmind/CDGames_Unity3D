using UnityEngine;

namespace Scripts.Utils
{
    public class ItemRotator : MonoBehaviour
    {
        [SerializeField] private Vector3 _rotationAngle;
        [SerializeField, Min(1f)] private float _rotationSpeed;

        private void Update()
        {
            transform.Rotate(_rotationAngle * _rotationSpeed * Time.deltaTime);
        }
    }
}
