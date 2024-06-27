using System.Linq;
using Configs;
using Infrastructure.AssetManagement;
using Services.Progress;
using UnityEngine.SceneManagement;

namespace Services.SceneServices
{
    public class LevelDataService : ILevelDataService
    {
        private readonly IPersistentProgressService _progressService;
        private readonly SOLevels _levels;

        private ILevelDataService _levelDataServiceImplementation;

        public LevelDataService(IAssetProvider assetProvider, IPersistentProgressService progressService)
        {
            _progressService = progressService;
            _levels = assetProvider.LoadResource<SOLevels>(AssetPath.LevelsDataPath);
        }

        public LevelData GetLevelData(string levelName)
        {
            int id = _levels
                .LevelsData
                .FirstOrDefault(level => level.LevelName == levelName)!
                .Id;

            foreach (LevelData levelData in _levels.LevelsData.Where(levelData => levelData.Id == id))
                return levelData;

            return new LevelData();
        }

        public string GetCurrentLevelName() => 
            SceneManager.GetActiveScene().name;

        public string GetNextSceneName(string currentSceneName)
        {
            int nextLevelId = _levels
                .LevelsData
                .FirstOrDefault(level => level.LevelName == currentSceneName)!
                .Id + 1;

            foreach (LevelData levelData in _levels.LevelsData.Where(levelData => levelData.Id == nextLevelId))
                return levelData.LevelName;

            return currentSceneName;
        }

        public string GetMaxOpenedLevelName() =>
            _levels.LevelsData
                .FirstOrDefault(level => level.Id == _progressService.PlayerProgress.LevelProgressData.MaxLevelId)
                ?.LevelName;

        public LevelData GetLevelData(int id)
        {
            foreach (LevelData levelData in _levels.LevelsData.Where(levelData => levelData.Id == id))
                return levelData;

            return new LevelData();
        }
    }
}