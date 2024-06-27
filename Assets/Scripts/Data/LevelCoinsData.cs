using System;
using System.Collections.Generic;
using Configs;

namespace Data
{
    [Serializable]
    public class LevelCoinsData
    {
        public string LevelName;
        public List<CoinData> CoinsData;

        public LevelCoinsData(string levelName, List<CoinData> coinsData)
        {
            LevelName = levelName;
            CoinsData = coinsData;
        }
    }
}