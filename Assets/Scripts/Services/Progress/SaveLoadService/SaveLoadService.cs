using Data;
using Extensions;
using UnityEngine;

namespace Services.Progress.SaveLoadService
{
    class SaveLoadService : ISaveLoadService
    {
        private readonly IPersistentProgressService _progressService;
        private const string ProgressKey = "Progress";
        
        public SaveLoadService(IPersistentProgressService progressService)
        {
            _progressService = progressService;
        }

        public void SaveProgress(ISavedProgress savedProgress)
        {
            savedProgress.UpdateProgress(_progressService.PlayerProgress);

            UpdatePlayerPrefs();
        }

        public void UpdatePlayerPrefs() => 
            PlayerPrefs.SetString(ProgressKey, _progressService.PlayerProgress.ToJson());

        public PlayerProgress LoadProgress() => 
            PlayerPrefs.GetString(ProgressKey)?.ToDeserialized<PlayerProgress>();

        public void LoadReaderProgress(ISavedProgressReader savedProgressReader) => 
            savedProgressReader.LoadProgress(_progressService.PlayerProgress);
    }
}