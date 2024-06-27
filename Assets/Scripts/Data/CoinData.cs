using System;

namespace Data
{
    [Serializable]
    public class CoinData
    {
        public bool IsCollected;
        public int Id;

        public CoinData(int id)
        {
            Id = id;
            IsCollected = false;
        }
    }
}