using Infrastructure;
using Infrastructure.AssetManagement;
using Infrastructure.GameStates;
using Inputs.Services;
using Services.Cursor;
using Services.Factory;
using Services.LevelAccess;
using Services.Progress;
using Services.Progress.SaveLoadService;
using Services.SceneServices;
using Services.StateMachine;
using Services.StaticData;
using SoundManagement;
using UI;
using UI.Factory;
using UI.Service;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class BootstrapInstaller : MonoInstaller, ICoroutineRunner
    {
        public LoadingCurtain LoadingCurtainPrefab;

        public override void InstallBindings()
        {
            BindCursorService();
            BindInputService();
            BindCurtain();
            BindPersistentProgressService();
            BindCodeAccessChecker();
            BindUIInputService();
            BindCoroutineRunner();
            BindSceneLoader();
            BindAssetProvider();
            BindSaveLoadService();
            BindStaticDataService();
            BindFactories();
            BindWindowService();
            BindSoundService();
            BindAccessoriesService();

            BindLevelDataService();

            BindStateMachine();
        }

        private void BindSoundService() =>
            Container
                .Bind<ISoundService>()
                .To<SoundService>()
                .AsSingle();

        private void BindInputService() =>
            Container
                .Bind<IInputService>()
                .To<InputService>()
                .AsSingle();

        private void BindCursorService() =>
            Container
                .Bind<CursorService>()
                .AsSingle();

        private void BindSaveLoadService() =>
            Container
                .Bind<ISaveLoadService>()
                .To<SaveLoadService>()
                .AsSingle();

        private void BindPersistentProgressService() =>
            Container
                .Bind<IPersistentProgressService>()
                .To<PersistentProgressService>()
                .AsSingle();

        private void BindWindowService() =>
            Container
                .Bind<IWindowService>()
                .To<WindowService>()
                .AsSingle();

        private void BindStaticDataService()
        {
            Container
                .Bind<IStaticDataService>()
                .To<StaticDataService>()
                .AsSingle();

            Container.Resolve<IStaticDataService>().LoadStaticData();
        }

        private void BindCodeAccessChecker() =>
            Container
                .Bind<ICodeAccessChecker>()
                .To<CodeAccessChecker>()
                .AsSingle();

        private void BindUIInputService() =>
            Container
                .Bind<IUIInputService>()
                .To<UIInputService>()
                .AsSingle();

        private void BindLevelDataService() =>
            Container
                .Bind<ILevelDataService>()
                .To<LevelDataService>()
                .AsSingle();

        private void BindCoroutineRunner() =>
            Container
                .Bind<ICoroutineRunner>()
                .FromInstance(this)
                .AsSingle();

        private void BindAccessoriesService()
        {
            Container
                .Bind<IAccessoriesService>()
                .To<AccessoriesService>()
                .AsSingle();
        }
        
        private void BindSceneLoader() =>
            Container
                .Bind<ISceneLoader>()
                .To<SceneLoader>()
                .AsSingle();

        private void BindAssetProvider() =>
            Container
                .Bind<IAssetProvider>()
                .To<AssetProvider>()
                .AsSingle();

        private void BindFactories()
        {
            Container
                .Bind<IGameFactory>()
                .To<GameFactory>()
                .AsSingle();

            Container
                .Bind<IUIFactory>()
                .To<UIFactory>()
                .AsSingle();

            Container
                .Bind<StatesFactory>()
                .AsSingle();
        }

        private void BindCurtain()
        {
            LoadingCurtain loadingCurtain = Container.InstantiatePrefabForComponent<LoadingCurtain>(
                LoadingCurtainPrefab, Vector3.zero,
                Quaternion.identity, null);

            Container
                .Bind<LoadingCurtain>()
                .FromInstance(loadingCurtain)
                .AsSingle();
        }

        private void BindStateMachine()
        {
            Container.Bind<IGameStateMachine>().To<GameStateMachine>().AsSingle();

            Container.Bind<BootstrapState>().AsSingle();
            Container.Bind<MainMenuState>().AsSingle();
            Container.Bind<LoadLevelState>().AsSingle();
            Container.Bind<GameLoopState>().AsSingle();
            Container.Bind<DailyRewardState>().AsSingle();
        }
    }
}