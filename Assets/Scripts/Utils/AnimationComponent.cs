using UnityEngine;

namespace Scripts.Utils
{
    public class AnimationComponent : MonoBehaviour
    {
        [SerializeField] private bool _isRotating = false;
        [SerializeField] private Vector3 _rotationAngle;
        [SerializeField] private float _rotationSpeed;

        private void Update()
        {
            if (_isRotating)
                transform.Rotate(_rotationAngle * _rotationSpeed * Time.deltaTime);
        }
    }
}
