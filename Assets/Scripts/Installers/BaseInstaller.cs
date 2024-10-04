using Scripts.Characters;
using Scripts.Managers;
using Scripts.Pooling;
using Scripts.Service;
using Scripts.UI;
using UnityEngine;
using Zenject;

namespace Unavinar.Installers
{
    public class BaseInstaller : MonoInstaller
    {
        [SerializeField] private GameManager _gameManager;
        [SerializeField] private MatchManager _matchManager;
        [SerializeField] private AudioManager _audioManager;
        [SerializeField] private ObjectPool _objectPool;
        [SerializeField] private CameraController _cameraController;
        [SerializeField] private Player _player;
        [SerializeField] private BoardControlSlider _boardControlSlider;

        public override void InstallBindings()
        {
            InstallManagers();
            InstallObjectPool();
            InstallCameraController();
            InstallPlayer();
            InstallBoardControlSlider();
        }

        private void InstallManagers()
        {
            Container.Bind<GameManager>().FromInstance(_gameManager).AsSingle().NonLazy();
            Container.QueueForInject(_gameManager);

            Container.Bind<MatchManager>().FromInstance(_matchManager).AsSingle().NonLazy();
            Container.QueueForInject(_matchManager);

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

        private void InstallPlayer()
        {
            Container.Bind<Player>().FromInstance(_player).AsSingle().NonLazy();
            Container.QueueForInject(_player);
        }

        private void InstallBoardControlSlider()
        {
            Container.Bind<BoardControlSlider>().FromInstance(_boardControlSlider).AsSingle().NonLazy();
            Container.QueueForInject(_boardControlSlider);
        }
    }
}
