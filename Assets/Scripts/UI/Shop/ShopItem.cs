using System;
using Configs;
using Data;
using Extensions;
using Services.Progress;
using TMPro;
using UI.Windows;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Shop
{
    public class ShopItem : MonoBehaviour
    {
        private const string ProgressKey = "Progress";

        public event Action OnProductBought;

        public TextMeshProUGUI Name;
        public TextMeshProUGUI PriceTitle;
        public Image ShopItemImage;
        public string PrefabName { get; set; }

        public int Price { get; set; }

        // public ShopItemType ShopItemType { get; set; }
        public AccessoryType AccessoryType { get; set; }

        [SerializeField] private Button BuyButton;

        private IPersistentProgressService _progressService;

        [Inject]
        private void Construct(IPersistentProgressService progressService)
        {
            _progressService = progressService;
        }

        private void Awake() => 
            BuyButton.onClick.AddListener(Buy);

        private void Buy()
        {
            if (_progressService.PlayerProgress.PlayerProgressData.Balance >= Price)
            {
                // switch (ShopItemType)
                // {
                //     case ShopItemType.ACCESSORY:
                _progressService.PlayerProgress.PlayerProgressData.AccessoriesData.Add(
                    new AccessoryData(PrefabName, ShopItemImage.sprite, AccessoryType));
                //     break;
                // case ShopItemType.ANIMAL:
                //     _progressService.PlayerProgress.PlayerProgressData.AnimalsData.Add(
                //         new AnimalData(PrefabName, ShopItemImage.sprite));
                //     break;
            }

            _progressService.PlayerProgress.PlayerProgressData.Balance -= Price;
            OnProductBought?.Invoke();

            PlayerPrefs.SetString(ProgressKey, _progressService.PlayerProgress.ToJson());
        }
    }
}