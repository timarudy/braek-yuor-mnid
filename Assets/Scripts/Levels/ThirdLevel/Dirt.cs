using System.Linq;
using Configs;
using Data;
using Infrastructure.AssetManagement;
using InteractionManagement;
using Player;
using Services.Progress;
using UI.PopUps;
using UI.Windows;
using UnityEngine;
using Zenject;

namespace Levels.ThirdLevel
{
    public class Dirt : MonoBehaviour, IInteractable
    {
        private const string DirtSkin = "Skin_Dirt";
        
        [SerializeField] private Material DirtMaterial;
        [SerializeField] private MudBarrier MudBarrier;

        private ToolTipVisibilityHandler _toolTipHandler;
        private IPersistentProgressService _progressService;
        private IAssetProvider _assetProvider;

        public bool IsInteractable { get; set; } = true;

        [Inject]
        private void Construct(IPersistentProgressService progressService, IAssetProvider assetProvider)
        {
            _progressService = progressService;
            _assetProvider = assetProvider;
        }

        public void Interact(Interactor interactor)
        {
            PlayerController player = interactor.GetComponent<PlayerController>();
            player.ChangeSkin(DirtMaterial);
            player.UpdateStatus(AccessoryType.SKINS, DirtSkin);
            MudBarrier.Open();

            if (_progressService.PlayerProgress.PlayerProgressData.AccessoriesData.All(accessory => accessory.Name != DirtSkin))
            {
                ShopItemData skin = _assetProvider.LoadResource<SOShopItems>(AssetPath.ShopItemsDataPath).ShopItemsData
                    .Find(item => item.Name == DirtSkin);

                _progressService.PlayerProgress.PlayerProgressData.AccessoriesData.Add(
                    new AccessoryData(skin.Name, true, skin.ShopItemImage, AccessoryType.SKINS));   
            }

            DestroySelf();
        }

        private void Start()
        {
            _toolTipHandler = GetComponent<ToolTipVisibilityHandler>();
        }

        public void OpenTooltip(Interactor interactor)
        {
            _toolTipHandler.OpenToolTip();
        }

        public void CloseTooltip(Interactor interactor)
        {
            _toolTipHandler.CloseToolTip();
        }

        private void DestroySelf() =>
            Destroy(gameObject);
    }
}