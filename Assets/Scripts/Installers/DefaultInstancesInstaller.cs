using Levels.Common;
using Player;
using Services.Factory;
using Services.Progress;
using Services.SceneServices;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class DefaultInstancesInstaller : MonoInstaller
    {
        public Transform PlayerStartPoint;
        public Transform BlockPosition;
        public Transform MonitorPosition;
        public PlayerController PlayerPrefab;
        public BlockHandler BlockPrefab;
        public Monitor MonitorPrefab;

        [Range(-360, 360)] public int MonitorYRotation;

        private IGameFactory _gameFactory;
        private IPersistentProgressService _progressService;

        public override void InstallBindings()
        {
            _gameFactory = Container.Resolve<IGameFactory>();
            _progressService = Container.Resolve<IPersistentProgressService>();

            PlayerController player = _gameFactory.CreateAndBind(PlayerStartPoint.position, PlayerPrefab);
            player.LoadProgress(_progressService.PlayerProgress);

            _gameFactory.CreateAndBind(BlockPosition.position, BlockPrefab);
            Monitor monitor = _gameFactory.Create(MonitorPosition.position, MonitorPrefab);
            monitor.transform.rotation = Quaternion.Euler(0, MonitorYRotation, 0);
        }
    }
}