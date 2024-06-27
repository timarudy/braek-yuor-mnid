using Infrastructure.GameStates;
using Services.StateMachine;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Windows
{
    public class KilledWindow : WindowBase
    {
        [SerializeField] private Button MainMenuButton;
        
        private IGameStateMachine _stateMachine;

        [Inject]
        private void Construct(IGameStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }
        
        protected override void OnAwake() => 
            MainMenuButton.onClick.AddListener(ToMainMenu);

        private void ToMainMenu()
        {
            WindowService.CurrentWindow = null;
            if (Time.timeScale == 0f) 
                Time.timeScale = 1f;
            
            _stateMachine.Enter<MainMenuState>();
        }
    }
}