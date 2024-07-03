using System;
using System.Collections.Generic;
using Configs;
using Data;
using Services.Progress;
using Services.SceneServices;
using TMPro;
using UI.Factory;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Windows
{
    class AccessoriesWindow : WindowBase
    {
        [SerializeField] private TextMeshProUGUI Title;
        [SerializeField] private Transform AccessoriesContainer;
        [SerializeField] private Button Next;
        [SerializeField] private Button Previous;

        private readonly List<AccessoryData> _clothes = new();
        private readonly List<AccessoryData> _skins = new();
        private readonly List<AccessoryData> _animals = new();
        private readonly Dictionary<int, List<AccessoryData>> _accessories = new();
        
        private IPersistentProgressService _progressService;
        private IAccessoriesService _accessoriesService;
        private IUIFactory _uiFactory;
        private int _currentPage;

        [Inject]
        private void Construct(IAccessoriesService accessoriesService, IPersistentProgressService progressService, IUIFactory uiFactory)
        {
            _uiFactory = uiFactory;
            _progressService = progressService;
            _accessoriesService = accessoriesService;
        }

        protected override void OnEnableAction()
        {
            base.OnEnableAction();
            
            Next.onClick.AddListener(NextPage);
            Previous.onClick.AddListener(PreviousPage);
        }

        protected override void OnAwake()
        {
            base.OnAwake();

            _currentPage = _accessoriesService.CurrentAccessoriesPage;
            
            InitAccessoryContainers();
            SetAccessories(_accessories[_accessoriesService.CurrentAccessoriesPage]);
        }

        private void NextPage()
        {
            if (_currentPage < _accessories.Count)
            {
                _currentPage++;
                UpdateAccessories();
            }
        }

        private void PreviousPage()
        {
            if (_currentPage != 1)
            {
                _currentPage--;
                UpdateAccessories();
            }
        }

        private void UpdateAccessories()
        {
            _accessoriesService.CurrentAccessoriesPage = _currentPage;
            ClearAccessories();
            SetAccessories(_accessories[_currentPage]);
        }

        private void ClearAccessories()
        {
            foreach (Transform child in AccessoriesContainer.transform)
                Destroy(child.gameObject);
        }

        protected override void CloseSelf()
        {
            base.CloseSelf();
            WindowService.Open(WindowType.SETTINGS);
        }

        private void SetAccessories(List<AccessoryData> accessories)
        {
            foreach (AccessoryData accessoryData in accessories)
            {
                _uiFactory.CreateAccessoryItem(accessoryData.Name, accessoryData.AccessoryImage,
                    accessoryData.IsOn, accessoryData.AccessoryType, AccessoriesContainer);
            }

            SetTitle();
        }

        private void SetTitle()
        {
            Title.text = _currentPage switch
            {
                1 => "clothes",
                2 => "skins",
                3 => "animals",
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private void InitAccessoryContainers()
        {
            var accessoriesData = _progressService.PlayerProgress.PlayerProgressData.AccessoriesData;

            foreach (AccessoryData accessoryData in accessoriesData)
            {
                if (accessoryData.AccessoryType == AccessoryType.CLOTHES)
                {
                    _clothes.Add(accessoryData);
                }
                else if (accessoryData.AccessoryType == AccessoryType.SKINS)
                {
                    _skins.Add(accessoryData);
                }
                else if (accessoryData.AccessoryType == AccessoryType.ANIMALS)
                {
                    _animals.Add(accessoryData);
                }
            }

            _accessories.Add(1, _clothes);
            _accessories.Add(2, _skins);
            _accessories.Add(3, _animals);
        }
    }
}