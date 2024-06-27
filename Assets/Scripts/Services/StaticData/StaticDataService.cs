using System.Collections.Generic;
using System.Linq;
using Configs;
using Data;
using Infrastructure.AssetManagement;
using Services.Progress;
using Services.Progress.SaveLoadService;
using UI.Factory;
using UI.Windows;

namespace Services.StaticData
{
    public class StaticDataService : IStaticDataService, ISavedProgress
    {
        private readonly IAssetProvider _assetProvider;
        private readonly IPersistentProgressService _progressService;
        private readonly ISaveLoadService _saveLoadService;

        private Dictionary<WindowType, WindowConfig> _windowConfigs;
        private Dictionary<string, ShopItemData> _shopItemsConfigs;
        private Dictionary<int, LevelData> _levelItemsConfigs;

        public int MaxOpenedLevel { get; private set; } = 1;

        public StaticDataService(IAssetProvider assetProvider, IPersistentProgressService progressService, ISaveLoadService saveLoadService)
        {
            _assetProvider = assetProvider;
            _progressService = progressService;
            _saveLoadService = saveLoadService;
        }

        public void LoadStaticData()
        {
            _windowConfigs = _assetProvider
                .LoadResource<SOWindows>(AssetPath.WindowsDataPath)
                .WindowConfigs
                .ToDictionary(x => x.WindowType, x => x);

            _shopItemsConfigs = _assetProvider
                .LoadResource<SOShopItems>(AssetPath.ShopItemsDataPath)
                .ShopItemsData
                .ToDictionary(x => x.Name, x => x);

            _levelItemsConfigs = _assetProvider
                .LoadResource<SOLevels>(AssetPath.LevelsDataPath)
                .LevelsData
                .ToDictionary(x => x.Id, x => x);
        }

        public WindowBase ForWindow(WindowType windowType) =>
            _windowConfigs.TryGetValue(windowType, out WindowConfig windowConfig)
                ? windowConfig.Prefab
                : null;

        public ShopItemData ForShopItems(string name) =>
            _shopItemsConfigs.TryGetValue(name, out ShopItemData shopItemConfig)
                ? shopItemConfig
                : new ShopItemData();

        public LevelData ForLevelItems(int id) =>
            _levelItemsConfigs.TryGetValue(id, out LevelData levelData)
                ? levelData
                : new LevelData();

        public void OpenNewLevel(int id)
        {
            if (_levelItemsConfigs.TryGetValue(id + 1, out LevelData levelData))
            {
                _levelItemsConfigs[id + 1].IsOpenedLevel = true;
                MaxOpenedLevel++;
            }
        }

        public void UpdateProgress(PlayerProgress progress)
        {
            progress.LevelProgressData.MaxLevelId = MaxOpenedLevel;
        }

        public void LoadProgress(PlayerProgress progress)
        {
            MaxOpenedLevel = progress.LevelProgressData.MaxLevelId;

            for (int i = 1; i <= _levelItemsConfigs.Count; i++)
            {
                _levelItemsConfigs[i].IsOpenedLevel = i <= MaxOpenedLevel;
            }
        }
    }
}