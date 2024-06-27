using Inputs.Services;
using Services.Cursor;
using Services.SceneServices;
using UnityEngine;

namespace Infrastructure.GameStates
{
    public class MainMenuState : IState
    {
        private readonly ISceneLoader _sceneLoader;
        private readonly IUIInputService _uiInputService;
        private readonly CursorService _cursorService;
        private readonly IInputService _inputService;

        public MainMenuState(ISceneLoader sceneLoader, IUIInputService uiInputService, CursorService cursorService, IInputService inputService)
        {
            _sceneLoader = sceneLoader;
            _uiInputService = uiInputService;
            _cursorService = cursorService;
            _inputService = inputService;
        }

        public void Exit()
        {
        }

        public void Enter()
        {
            Debug.Log("MainMenuScene");
            _cursorService.UnlockCursor();
            _sceneLoader.Load(SceneNames.MainMenu, OnLoaded);
        }

        private void OnLoaded()
        {
            _uiInputService.DisableAllInputs();
            _inputService.DisableAttackable();
        }
    }
}