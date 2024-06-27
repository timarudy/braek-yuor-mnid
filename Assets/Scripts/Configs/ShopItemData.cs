using System;
using UI.Windows;
using UnityEngine;

namespace Configs
{
    [Serializable]
    public struct ShopItemData
    {
        public string Name;
        public int Price;
        public Sprite ShopItemImage;
        public bool IsCollectable;
        public AccessoryType AccessoryType;
    }
}