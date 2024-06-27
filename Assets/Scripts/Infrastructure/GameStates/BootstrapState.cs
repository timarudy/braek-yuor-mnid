using System.Collections.Generic;
using Data;
using Services.Progress;
using Services.Progress.SaveLoadService;
using Services.SceneServices;
using Services.StateMachine;
using Services.StaticData;
using UI.Windows;
using UnityEngine;

namespace Infrastructure.GameStates
{
    public class BootstrapState : IState
    {
        private readonly IGameStateMachine _stateMachine;
        private readonly ISceneLoader _sceneLoader;
        private readonly ISaveLoadService _saveLoadService;
        private readonly IPersistentProgressService _progressService;
        private readonly ISavedProgress _staticData;

        public BootstrapState(IGameStateMachine stateMachine, ISceneLoader sceneLoader,
            ISaveLoadService saveLoadService, IPersistentProgressService progressService, IStaticDataService staticData)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _saveLoadService = saveLoadService;
            _progressService = progressService;
            _staticData = staticData as ISavedProgress;
        }

        public void Enter()
        {
            Debug.Log("BootstrapState");
            LoadOrInitNewProgress();
            LoadLevelsProgress();
            _sceneLoader.Load(SceneNames.BootstrapScene, onLoaded: EnterMainMenu);
        }

        public void Exit()
        {
        }

        private void LoadLevelsProgress()
        {
            _staticData.LoadProgress(_progressService.PlayerProgress);
        }

        private void LoadOrInitNewProgress() =>
            _progressService.PlayerProgress = _saveLoadService.LoadProgress() ?? NewProgress();

        private PlayerProgress NewProgress() =>
            new(
                maxLevelId: 1,
                balance: 0,
                new List<AccessoryData>(),
                new List<LevelCoinsData>()
            );

        private void EnterMainMenu()
        {
            _stateMachine.Enter<MainMenuState>();
        }
    }
}