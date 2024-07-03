using Infrastructure.GameStates;
using Inputs.Services;
using Levels.Common;
using Services.StateMachine;
using UI.Factory;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Windows
{
    public class SettingsWindow : WindowBase
    {
        [SerializeField] private Button MainMenuButton;
        [SerializeField] private Button AccessoriesWindowButton;
        [SerializeField] private Button GameSettingsWindowButton;

        private IGameStateMachine _stateMachine;
        private IUIInputService _uiInputService;

        [Inject]
        private void Construct(IGameStateMachine stateMachine, IUIInputService uiInputService)
        {
            _uiInputService = uiInputService;
            _stateMachine = stateMachine;
        }

        protected override void OnAwake()
        {
            base.OnAwake();
            MainMenuButton.onClick.AddListener(ToMainMenu);
            AccessoriesWindowButton.onClick.AddListener(ToAccessories);
            GameSettingsWindowButton.onClick.AddListener(ToGameSettings);
        }

        protected override void OnEnableAction()
        {
            base.OnEnableAction();
            _uiInputService.OpenSettings();
        }

        protected override void OnDisableAction()
        {
            base.OnDisableAction();
            _uiInputService.CloseSettings();
        }

        private void ToGameSettings() => 
            WindowService.ReopenWindow(WindowType.GAME_SETTINGS);

        private void ToAccessories() => 
            WindowService.ReopenWindow(WindowType.ACCESSORIES);

        private void ToMainMenu()
        {
            WindowService.CurrentWindow = null;
            if (Time.timeScale == 0f) 
                Time.timeScale = 1f;
            
            _stateMachine.Enter<MainMenuState>();
        }
    }
}