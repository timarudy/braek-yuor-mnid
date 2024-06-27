using System.Collections.Generic;
using Configs;
using Infrastructure.AssetManagement;
using Infrastructure.GameStates;
using Services.Progress;
using Services.Progress.SaveLoadService;
using Services.StateMachine;
using Services.StaticData;
using UI.Factory;
using UI.Service;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Windows
{
    class LevelsListWindow : WindowBase
    {
        [SerializeField] private Canvas LevelItemsContainer;
        // [SerializeField] private Button ResetLevelsButton;
        [SerializeField] private Button DailyRewardButton;

        private IAssetProvider _assetProvider;
        private IUIFactory _uiFactory;
        private IStaticDataService _staticData;
        private IPersistentProgressService _progressService;
        private IWindowService _windowService;
        private ISaveLoadService _saveLoadService;
        private IGameStateMachine _stateMachine;

        [Inject]
        private void Construct(ISaveLoadService saveLoadService, IAssetProvider assetProvider, IUIFactory uiFactory,
            IStaticDataService staticData, IPersistentProgressService progressService, IWindowService windowService,
            IGameStateMachine stateMachine)
        {
            _windowService = windowService;
            _progressService = progressService;
            _staticData = staticData;
            _uiFactory = uiFactory;
            _assetProvider = assetProvider;
            _saveLoadService = saveLoadService;
            _stateMachine = stateMachine;
        }

        protected override void OnAwake()
        {
            base.OnAwake();
            // ResetLevelsButton.onClick.AddListener(ResetLevels);
            DailyRewardButton.onClick.AddListener(ToDailyRewardScene);

            List<LevelData> levelsData = _assetProvider.LoadResource<SOLevels>(AssetPath.LevelsDataPath).LevelsData;

            foreach (LevelData levelData in levelsData)
                _uiFactory.CreateLevelItem(levelData.Id, LevelItemsContainer.transform);
        }

        private void ToDailyRewardScene()
        {
            _windowService.CloseCurrentWindow();
            _stateMachine.Enter<DailyRewardState>();
        }

        // private void ResetLevels()
        // {
        //     CleanUpPassedLevels();
        //
        //     ISavedProgress staticData = _staticData as ISavedProgress;
        //     staticData?.LoadProgress(_progressService.PlayerProgress);
        //
        //     _windowService.ReopenWindow(WindowType.LEVELS_LIST);
        // }

        private void CleanUpPassedLevels()
        {
            _progressService.PlayerProgress.LevelProgressData.MaxLevelId = 1;
            _saveLoadService.UpdatePlayerPrefs();
        }
    }
}