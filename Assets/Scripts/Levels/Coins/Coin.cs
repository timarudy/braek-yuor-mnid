using Data;
using Services.SceneServices;
using Zenject;

namespace Levels.Coins
{
    public class Coin : CollectableBase
    {
        public CoinSpawner CoinSpawner { get; set; }

        private ILevelDataService _levelDataService;

        [Inject]
        private void Construct(ILevelDataService levelDataService)
        {
            _levelDataService = levelDataService;
        }

        protected override void Collect()
        {
            Player.AddMoney(1);
            UpdateCollection();
            
            base.Collect();
        }

        private void UpdateCollection()
        {
            ProgressService.PlayerProgress.LevelProgressData.LevelsCoinsData
                .Find(level => level.LevelName == _levelDataService.GetCurrentLevelName())
                .CoinsData
                .Find(coin => coin.Id == CoinSpawner.Id)
                .IsCollected = true;
        }
    }
}