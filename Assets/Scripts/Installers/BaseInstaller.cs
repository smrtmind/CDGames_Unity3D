using Scripts.Characters;
using Scripts.Managers;
using Scripts.Pooling;
using Scripts.Service;
using UnityEngine;
using Zenject;

namespace Unavinar.Installers
{
    public class BaseInstaller : MonoInstaller
    {
        [SerializeField] private PlayerController _playerController;
        [SerializeField] private GameManager _gameManager;
        [SerializeField] private AudioManager _audioManager;
        [SerializeField] private ObjectPool _objectPool;
        [SerializeField] private CameraController _cameraController;

        public override void InstallBindings()
        {
            InstallPlayerController();
            InstallGameSession();
            InstallAudioManager();
            InstallObjectPool();
            InstallCameraController();
        }

        private void InstallPlayerController()
        {
            Container.Bind<PlayerController>().FromInstance(_playerController).AsSingle().NonLazy();
            Container.QueueForInject(_playerController);
        }

        private void InstallGameSession()
        {
            Container.Bind<GameManager>().FromInstance(_gameManager).AsSingle().NonLazy();
            Container.QueueForInject(_gameManager);
        }

        private void InstallAudioManager()
        {
            Container.Bind<AudioManager>().FromInstance(_audioManager).AsSingle().NonLazy();
            Container.QueueForInject(_audioManager);
        }

        private void InstallObjectPool()
        {
            Container.Bind<ObjectPool>().FromInstance(_objectPool).AsSingle().NonLazy();
            Container.QueueForInject(_objectPool);
        }

        private void InstallCameraController()
        {
            Container.Bind<CameraController>().FromInstance(_cameraController).AsSingle().NonLazy();
            Container.QueueForInject(_cameraController);
        }
    }
}
