using System.Collections.Generic;
using System.Linq;
using Configs;
using Extensions;
using Infrastructure.AssetManagement;
using Player;
using Services.Progress;
using TMPro;
using UI.Factory;
using UI.Service;
using UI.Windows;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Shop
{
    public class ShopWindow : WindowBase
    {
        [SerializeField] private Canvas ShopItemsContainer;
        [SerializeField] private TextMeshProUGUI Balance;

        private IAssetProvider _assetProvider;
        private IUIFactory _uiFactory;
        private IPersistentProgressService _progressService;
        private IWindowService _windowService;

        [Inject]
        private void Construct(IAssetProvider assetProvider, IUIFactory uiFactory,
            IPersistentProgressService progressService, IWindowService windowService)
        {
            _windowService = windowService;
            _progressService = progressService;
            _uiFactory = uiFactory;
            _assetProvider = assetProvider;
        }

        private void SetBalance(int money) =>
            Balance.text = $"balance: ${money}";

        protected override void OnAwake()
        {
            base.OnAwake();
            SetBalance(_progressService.PlayerProgress.PlayerProgressData.Balance);

            List<ShopItemData> shopItemsData =
                _assetProvider.LoadResource<SOShopItems>(AssetPath.ShopItemsDataPath).ShopItemsData;

            foreach (ShopItemData shopItemData in shopItemsData)
            {
                if (NotAlreadyBoughtItem(shopItemData))
                {
                    if (!CollectableItem(shopItemData))
                    {
                        ShopItem shopItem = _uiFactory.CreateShopItem(shopItemData.Name, ShopItemsContainer.transform);
                        shopItem.OnProductBought += ReopenShopWindow;
                    }
                }
            }
        }

        private bool CollectableItem(ShopItemData shopItemData) =>
            shopItemData.IsCollectable;

        private bool NotAlreadyBoughtItem(ShopItemData shopItemData)
        {
            // return _progressService.PlayerProgress.PlayerProgressData.AccessoriesData.All(accessory =>
            //            accessory.Name != shopItemData.Name) &&
            //        _progressService.PlayerProgress.PlayerProgressData.AnimalsData.All(animal =>
            //            animal.Name.AnimalTypeToString() != shopItemData.Name);
            return _progressService.PlayerProgress.PlayerProgressData.AccessoriesData.All(accessory =>
                accessory.Name != shopItemData.Name);
        }

        private void ReopenShopWindow() =>
            _windowService.ReopenWindow(WindowType.SHOP);
    }
}