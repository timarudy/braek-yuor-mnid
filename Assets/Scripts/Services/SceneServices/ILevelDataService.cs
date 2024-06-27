using Configs;

namespace Services.SceneServices
{
    public interface ILevelDataService
    {
        string GetNextSceneName(string currentSceneName);
        string GetMaxOpenedLevelName();
        LevelData GetLevelData(int id);
        LevelData GetLevelData(string levelName);
        string GetCurrentLevelName();
    }
}