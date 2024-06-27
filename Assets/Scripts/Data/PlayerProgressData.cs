using System;
using System.Collections.Generic;
using UI.Windows;
using UnityEngine;
using UnityEngine.Serialization;

namespace Data
{
    [Serializable]
    public class PlayerProgressData
    {
        public int Balance;
        public List<AccessoryData> AccessoriesData;

        public PlayerProgressData(int balance, List<AccessoryData> accessoriesData)
        {
            Balance = balance;
            AccessoriesData = accessoriesData;
        }
    }
}