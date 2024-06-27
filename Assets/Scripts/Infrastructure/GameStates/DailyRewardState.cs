using Inputs.Services;
using Services.Cursor;
using Services.SceneServices;
using UnityEngine;
using Zenject;

namespace Infrastructure.GameStates
{
    public class DailyRewardState : IState
    {
        private IUIInputService _uiInputService;
        private CursorService _cursorService;
        private ISceneLoader _sceneLoader;

        [Inject]
        private void Construct(ISceneLoader sceneLoader, CursorService cursorService, IUIInputService uiInputService)
        {
            _sceneLoader = sceneLoader;
            _cursorService = cursorService;
            _uiInputService = uiInputService;
        }

        public void Enter()
        {
            Debug.Log("Reward State");
            // _cursorService.LockCursor();
            _sceneLoader.Load(SceneNames.DailyReward, OnLoaded);
        }

        private void OnLoaded()
        {
            _uiInputService.DisableAllInputs();
        }

        public void Exit()
        {
        }
    }
}