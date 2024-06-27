using System;
using System.Collections.Generic;

namespace Data
{
    [Serializable]
    public class PlayerProgress
    {
        public PlayerProgressData PlayerProgressData;
        public LevelProgressData LevelProgressData;

        public PlayerProgress(int maxLevelId, int balance, List<AccessoryData> accessoriesData, List<LevelCoinsData> levelsCoinsData)
        {
            LevelProgressData = new LevelProgressData(maxLevelId, levelsCoinsData);
            PlayerProgressData = new PlayerProgressData(balance, accessoriesData);
        }
    }
}