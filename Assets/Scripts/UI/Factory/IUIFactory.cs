using System;
using TMPro;
using UI.Shop;
using UI.Windows;
using UnityEngine;

namespace UI.Factory
{
    public interface IUIFactory
    {
        void CreateUIRoot();
        ShopItem CreateShopItem(string name, Transform parentTransform);
        void CreateLevelItem(int id, Transform parentTransform);
        WindowBase CreateWindow(WindowType windowType);
        void CreateAccessoryItem(string name, Sprite accessoryImage, bool isOn,
            AccessoryType accessoryType, Transform parentTransform);
        // void CreateAnimalItem(AnimalType name, bool isOn, Transform parentTransform);
        void CreateHpBar();
        event Action<HpBar> OnHpBarCreated;
    }
}