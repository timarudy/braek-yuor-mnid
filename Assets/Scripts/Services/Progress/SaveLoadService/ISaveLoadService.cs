using Data;

namespace Services.Progress.SaveLoadService
{
    public interface ISaveLoadService
    {
        void SaveProgress(ISavedProgress savedProgress);
        PlayerProgress LoadProgress();
        void UpdatePlayerPrefs();
        void LoadReaderProgress(ISavedProgressReader savedProgressReader);
    }
}