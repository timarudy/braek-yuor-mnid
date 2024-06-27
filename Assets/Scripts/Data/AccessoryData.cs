using System;
using UI.Windows;
using UnityEngine;
using UnityEngine.Serialization;

namespace Data
{
    [Serializable]
    public class AccessoryData
    {
        public string Name;
        public bool IsOn;
        public Sprite AccessoryImage;
        public AccessoryType AccessoryType;

        public AccessoryData(string name, bool isOn, Sprite accessoryImage, AccessoryType accessoryType)
        {
            Name = name;
            IsOn = isOn;
            AccessoryImage = accessoryImage;
            AccessoryType = accessoryType;
        }
        
        public AccessoryData(string name, Sprite accessoryImage, AccessoryType accessoryType)
        {
            Name = name;
            AccessoryImage = accessoryImage;
            AccessoryType = accessoryType;
            IsOn = false;
        }
    }
}