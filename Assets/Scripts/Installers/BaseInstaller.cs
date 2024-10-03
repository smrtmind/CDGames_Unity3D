using Scripts.Characters;
using Scripts.Pooling;
using Scripts.Service;
using Scripts.Utils;
using UnityEngine;
using Zenject;

namespace Unavinar.Installers
{
    public class BaseInstaller : MonoInstaller
    {
        [SerializeField] private PlayerController _playerController;
        [SerializeField] private GameSession _gameSession;
        [SerializeField] private AudioComponent _audioManager;
        [SerializeField] private ObjectPool _objectPool;

        public override void InstallBindings()
        {
            InstallPlayerController();
            InstallGameSession();
            InstallAudioManager();
            InstallObjectPool();
        }

        private void InstallPlayerController()
        {
            Container.Bind<PlayerController>().FromInstance(_playerController).AsSingle().NonLazy();
            Container.QueueForInject(_playerController);
        }

        private void InstallGameSession()
        {
            Container.Bind<GameSession>().FromInstance(_gameSession).AsSingle().NonLazy();
            Container.QueueForInject(_gameSession);
        }

        private void InstallAudioManager()
        {
            Container.Bind<AudioComponent>().FromInstance(_audioManager).AsSingle().NonLazy();
            Container.QueueForInject(_audioManager);
        }

        private void InstallObjectPool()
        {
            Container.Bind<ObjectPool>().FromInstance(_objectPool).AsSingle().NonLazy();
            Container.QueueForInject(_objectPool);
        }
    }
}
