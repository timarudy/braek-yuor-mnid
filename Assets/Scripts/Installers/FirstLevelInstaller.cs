using Infrastructure.AssetManagement;
using Levels.Common;
using Levels.FirstLevel;
using Services.Factory;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class FirstLevelInstaller : MonoInstaller
    {
        public Transform FirstStandPosition;
        public Transform SecondStandPosition;
        public Transform ThirdStandPosition;
        
        private IGameFactory _gameFactory;
        private IAssetProvider _assetProvider;

        public override void InstallBindings()
        {
            _gameFactory = Container.Resolve<IGameFactory>();
            _assetProvider = Container.Resolve<IAssetProvider>();
            
            CreateInputStand(FirstStandPosition, 1);
            CreateInputStand(SecondStandPosition, 2);
            CreateInputStand(ThirdStandPosition, 3);
        }

        private void CreateInputStand(Transform at, int id)
        {
            CodeDecryptorStand inputStand = _gameFactory.CreateAndBind(at.position,
                _assetProvider.LoadResource<CodeDecryptorStand>(AssetPath.InputStandPrefabPath));
            inputStand.Id = id;
            inputStand.Encryption.text = inputStand.GetInputStandData().Encryption;
        }
    }
}