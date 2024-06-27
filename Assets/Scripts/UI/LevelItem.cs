using Configs;
using Infrastructure.AssetManagement;
using Infrastructure.GameStates;
using Services.StateMachine;
using Services.StaticData;
using TMPro;
using UI.Service;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    public class LevelItem : MonoBehaviour
    {
        public Image LevelImage;
        public TextMeshProUGUI LevelNumber;
        public string LevelName { get; set; }

        private IGameStateMachine _stateMachine;
        private IWindowService _windowService;
        private IAssetProvider _assetProvider;
        private IStaticDataService _staticData;

        [Inject]
        private void Construct(IGameStateMachine stateMachine, IWindowService windowService,
            IAssetProvider assetProvider, IStaticDataService staticData)
        {
            _staticData = staticData;
            _assetProvider = assetProvider;
            _windowService = windowService;
            _stateMachine = stateMachine;
        }

        public void LoadLevel()
        {
            int levelId = _assetProvider
                .LoadResource<SOLevels>(AssetPath.LevelsDataPath)
                .LevelsData
                .Find(level => level.LevelName == LevelName)
                .Id;

            LevelData levelItemData = _staticData.ForLevelItems(levelId);

            if (levelItemData.IsOpenedLevel)
            {
                _windowService.CloseCurrentWindow();
                _stateMachine.Enter<LoadLevelState, string>(LevelName);
            }
        }
    }
}