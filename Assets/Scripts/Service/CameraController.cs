using UnityEngine;

namespace Scripts.Service
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Camera _mainCamera;

        public Camera GetMainCamera() => _mainCamera;
    }
}
