using System.Collections.Generic;
using System.Linq;
using Data;
using Infrastructure.AssetManagement;
using Levels.Coins;
using Services.Factory;
using Services.Progress;
using Services.Progress.SaveLoadService;
using Services.SceneServices;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class CoinsInstaller : MonoInstaller
    {
        public List<CoinSpawner> CoinSpawners;

        public override void InstallBindings()
        {
            IGameFactory gameFactory = Container.Resolve<IGameFactory>();
            IPersistentProgressService progressService = Container.Resolve<IPersistentProgressService>();
            ILevelDataService levelData = Container.Resolve<ILevelDataService>();
            ISaveLoadService saveLoadService = Container.Resolve<ISaveLoadService>();
            
            if (progressService.PlayerProgress.LevelProgressData.LevelsCoinsData.All(level => level.LevelName != levelData.GetCurrentLevelName()))
            {
                List<CoinData> coinsData = CoinSpawners
                    .Select(spawner => new CoinData(spawner.Id)).ToList();

                progressService.PlayerProgress.LevelProgressData.LevelsCoinsData
                    .Add(new LevelCoinsData(levelData.GetCurrentLevelName(), coinsData));
                
                saveLoadService.UpdatePlayerPrefs();
            }
            
            List<CoinData> currentLevelCoinsData = progressService.PlayerProgress.LevelProgressData.LevelsCoinsData
                .Find(level => level.LevelName == levelData.GetCurrentLevelName()).CoinsData
                .FindAll(coin => !coin.IsCollected);

            foreach (CoinData coinData in currentLevelCoinsData)
            {
                CoinSpawner coinSpawner = CoinSpawners.Find(spawner => spawner.Id == coinData.Id);

                Transform coinSpawnerTransform = coinSpawner.transform;

                Coin coin = gameFactory.SpawnCollectableObject(coinSpawnerTransform.position, AssetPath.CoinPrefabPath,
                    coinSpawnerTransform) as Coin;

                if (coin != null)
                    coin.CoinSpawner = coinSpawner;
            }
        }
    }
}