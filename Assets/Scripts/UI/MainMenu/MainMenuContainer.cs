using Infrastructure.GameStates;
using Services.SceneServices;
using Services.StateMachine;
using UI.Factory;
using UI.Service;
using UI.Windows;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Zenject;

namespace UI.MainMenu
{
    public class MainMenuContainer : MonoBehaviour
    {
        [SerializeField] private Button LevelsListButton;
        [SerializeField] private Button ContinueGameButton;
        [SerializeField] private Button QuitButton;
        [SerializeField] private Button ShopButton;
        [SerializeField] private Button InfoButton;

        private IGameStateMachine _stateMachine;
        private IWindowService _windowService;
        private ILevelDataService _levelDataService;

        [Inject]
        private void Construct(IGameStateMachine stateMachine, IWindowService windowService, ILevelDataService levelDataService)
        {
            _levelDataService = levelDataService;
            _stateMachine = stateMachine;
            _windowService = windowService;
        }

        private void Start()
        {
            ContinueGameButton.onClick.AddListener(Continue);
            ShopButton.onClick.AddListener(ToShop);
            QuitButton.onClick.AddListener(Quit);
            LevelsListButton.onClick.AddListener(ToLevelsList);
            InfoButton.onClick.AddListener(ToOverallInfo);
        }

        private void Quit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
            Application.Quit();
        }

        private void ToOverallInfo() => 
            _windowService.Open(WindowType.INFO);

        private void ToLevelsList() => 
            _windowService.Open(WindowType.LEVELS_LIST);

        private void ToShop() => 
            _windowService.Open(WindowType.SHOP);

        private void Continue() => 
            _stateMachine.Enter<LoadLevelState, string>(_levelDataService.GetMaxOpenedLevelName());
    }
}