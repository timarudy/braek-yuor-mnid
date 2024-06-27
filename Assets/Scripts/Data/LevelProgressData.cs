using System;
using System.Collections.Generic;
using Configs;

namespace Data
{
    [Serializable]
    public class LevelProgressData
    {
        public int MaxLevelId;
        public List<LevelCoinsData> LevelsCoinsData;
        public KillData KillData;

        public LevelProgressData(int maxLevelId, List<LevelCoinsData> levelsCoinsData)
        {
            MaxLevelId = maxLevelId;
            LevelsCoinsData = levelsCoinsData;
            KillData = new KillData();
        }
    }
}