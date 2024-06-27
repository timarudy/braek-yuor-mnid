using Player;
using Services.Progress;
using Services.Progress.SaveLoadService;
using TMPro;
using UI.Service;
using UI.Windows;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    public class AccessoryItem : MonoBehaviour
    {
        public Image AccessoryImage;
        public TextMeshProUGUI Status;
        
        public string Name { get; set; }
        public bool IsOn { get; set; }
        public AccessoryType AccessoryType { get; set; }
        
        private PlayerController _player;
        private IPersistentProgressService _progressService;
        private ISaveLoadService _saveLoadService;
        private IWindowService _windowService;
        
        [Inject]
        private void Construct(PlayerController player, IPersistentProgressService progressService,
            ISaveLoadService saveLoadService, IWindowService windowService)
        {
            _player = player;
            _progressService = progressService;
            _saveLoadService = saveLoadService;
            _windowService = windowService;
        }
        
        public void ManageAccessory()
        {
            if (!IsOn)
            {
                _player.EquipAccessory(Name, AccessoryType);
                Manage(true);
            }
            else
            {
                _player.UnequipAccessory(Name, AccessoryType);
                Manage(false);
            }
        
            _windowService.ReopenWindow(WindowType.ACCESSORIES);
        }
        
        private void Manage(bool value)
        {
            IsOn = value;
            
            _progressService.PlayerProgress.PlayerProgressData.AccessoriesData.Find(accessory =>
                accessory.Name == Name).IsOn = value;
            
            _saveLoadService.UpdatePlayerPrefs();
        }
    }
}