using Data;
using Extensions;
using Services.StaticData;
using UI.Windows;
using UnityEngine;
using Zenject;

namespace Levels.Coins
{
    public class AnimalCoin : CollectableBase
    {
        [SerializeField] private AnimalType Animal;

        private IStaticDataService _staticData;

        [Inject]
        private void Construct(IStaticDataService staticData)
        {
            _staticData = staticData;
        }

        protected override void Collect()
        {
            Sprite image = _staticData.ForShopItems(Animal.AnimalTypeToString()).ShopItemImage;
            ProgressService.PlayerProgress.PlayerProgressData.AccessoriesData.Add(
                new AccessoryData(Animal.AnimalTypeToString(), image, AccessoryType.ANIMALS));

            base.Collect();
        }
    }
}