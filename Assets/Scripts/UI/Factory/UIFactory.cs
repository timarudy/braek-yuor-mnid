using System;
using Configs;
using Extensions;
using Infrastructure.AssetManagement;
using Services.Progress;
using Services.StaticData;
using UI.Shop;
using UI.Windows;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace UI.Factory
{
    class UIFactory : IUIFactory
    {
        public event Action<HpBar> OnHpBarCreated; 

        private readonly DiContainer _diContainer;
        private readonly IAssetProvider _assetProvider;
        private readonly IStaticDataService _staticData;
        private readonly IPersistentProgressService _progressService;

        private Transform _uiRoot;

        public UIFactory(DiContainer diContainer, IAssetProvider assetProvider, IStaticDataService staticData,
            IPersistentProgressService progressService)
        {
            _diContainer = diContainer;
            _assetProvider = assetProvider;
            _staticData = staticData;
            _progressService = progressService;
        }

        public WindowBase CreateWindow(WindowType windowType)
        {
            Debug.Log("Create");
            WindowBase window = _staticData.ForWindow(windowType);
            _diContainer.InstantiatePrefab(window, _uiRoot);

            return window;
        }

        public void CreateAccessoryItem(string name, Sprite accessoryImage, bool isOn,
            AccessoryType accessoryType, Transform parentTransform)
        {
            GameObject accessoryItemPrefab =
                _assetProvider.LoadResource<AccessoryItem>(AssetPath.AccessoryItemPrefabPath).gameObject;

            AccessoryItem accessoryItem =
                _diContainer.InstantiatePrefabForComponent<AccessoryItem>(accessoryItemPrefab, parentTransform);

            accessoryItem.AccessoryImage.sprite = accessoryImage;
            accessoryItem.Status.text = isOn ? "On" : "Off";
            accessoryItem.IsOn = isOn;
            accessoryItem.Name = name;
            accessoryItem.AccessoryType = accessoryType;
        }

        public ShopItem CreateShopItem(string name, Transform parentTransform)
        {
            ShopItemData shopItemData = _staticData.ForShopItems(name);

            GameObject shopItemPrefab = _assetProvider.LoadResource<ShopItem>(AssetPath.ShopItemPrefabPath).gameObject;

            ShopItem shopItem = _diContainer.InstantiatePrefabForComponent<ShopItem>(shopItemPrefab, parentTransform);

            shopItem.ShopItemImage.sprite = shopItemData.ShopItemImage;
            shopItem.Name.text = name.CutUnderscores();
            shopItem.PriceTitle.text = $"${shopItemData.Price.ToString()}";
            shopItem.PrefabName = shopItemData.Name;
            shopItem.AccessoryType = shopItemData.AccessoryType;
            shopItem.Price = shopItemData.Price;

            return shopItem;
        }

        public void CreateHpBar()
        {
            HpBar hpBar = _diContainer.InstantiatePrefabForComponent<HpBar>(_assetProvider.LoadResource<HpBar>(AssetPath.HpBarPath),
                _uiRoot.GetComponentInChildren<PlayerHud>().transform);
            
            OnHpBarCreated?.Invoke(hpBar);
        }

        public void CreateLevelItem(int id, Transform parentTransform)
        {
            LevelData levelItemData = _staticData.ForLevelItems(id);

            GameObject levelItemPrefab =
                _assetProvider.LoadResource<LevelItem>(AssetPath.LevelItemPrefabPath).gameObject;

            LevelItem levelItem =
                _diContainer.InstantiatePrefabForComponent<LevelItem>(levelItemPrefab, parentTransform);

            levelItem.LevelImage.color = levelItemData.IsOpenedLevel ? Color.green : Color.red;
            levelItem.LevelNumber.text = id.ToString();
            levelItem.LevelName = levelItemData.LevelName;
        }

        public void CreateUIRoot()
        {
            // Transform uiRoot = _diContainer
            //     .InstantiatePrefab(_assetProvider.LoadResource<GameObject>(AssetPath.UIRootPath))
            //     .transform;
            
            Transform uiRoot = Object
                .Instantiate(_assetProvider.LoadResource<GameObject>(AssetPath.UIRootPath))
                .transform;
            
            CreateHud(uiRoot);

            _uiRoot = uiRoot;
        }

        private void CreateHud(Transform parentTransform)
        {
            _diContainer.InstantiatePrefabForComponent<PlayerHud>(
                _assetProvider.LoadResource<PlayerHud>(AssetPath.PlayerHudPath), parentTransform);
        }
    }
}