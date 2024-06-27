using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using Extensions;
using Levels.Animals;
using Services.Factory;
using Services.Progress;
using Services.Progress.SaveLoadService;
using UI.Windows;
using UnityEngine;
using Zenject;

namespace Player
{
    public class PlayerController : MonoBehaviour, ISavedProgress
    {
        public event Action OnDirtSkinWornOn;
        public event Action OnDirtSkinWornOff;
        
        private const string HatAccessory = "Hat";
        private const string BlueSkinAccessory = "Skin_Blue";
        private const string DirtySkinAccessory = "Skin_Dirt";

        [SerializeField] private List<GameObject> Hair;
        [SerializeField] private List<SkinnedMeshRenderer> PlayerMeshRenderers;
        [SerializeField] private GameObject Hat;
        [SerializeField] private Transform AnimalSpawnPoint;
        [SerializeField] private GameObject Lightening;
        [SerializeField] private Material BlueSkinMaterial;
        [SerializeField] private Material StandartSkinMaterial;
        [SerializeField] private Material DirtySkinMaterial;

        public Transform CollectPosition;

        private int _balance;
        private Material _playerSkin;
        private ISaveLoadService _saveLoadService;
        private IGameFactory _gameFactory;
        private AnimalBase _currentAnimal;
        private IPersistentProgressService _progressService;

        [Inject]
        private void Construct(ISaveLoadService saveLoadService, IGameFactory gameFactory,
            IPersistentProgressService progressService)
        {
            _gameFactory = gameFactory;
            _saveLoadService = saveLoadService;
            _progressService = progressService;
        }

        private void OnDestroy()
        {
            if (_currentAnimal != null)
                DeactivateAnimal();
        }

        public void ShowHair()
        {
            foreach (GameObject part in Hair)
                part.SetActive(true);
        }

        public void HideHair()
        {
            foreach (GameObject part in Hair)
                part.SetActive(false);
        }

        public void ChangeSkin(Material material)
        {
            _playerSkin = material;
            foreach (SkinnedMeshRenderer meshRenderer in PlayerMeshRenderers)
                meshRenderer.material = material;
        }

        public void AddMoney(int money)
        {
            _balance += money;
            _saveLoadService.SaveProgress(this);
        }

        public void EquipAccessory(string accessoryName, AccessoryType accessoryType)
        {
            switch (accessoryType)
            {
                case AccessoryType.CLOTHES:
                    ChangeCloth(accessoryName, true);
                    return;
                case AccessoryType.SKINS:
                    UpdateStatus(AccessoryType.SKINS, accessoryName);
                    ChangeSkin(accessoryName);
                    return;
                case AccessoryType.ANIMALS:
                    UpdateStatus(AccessoryType.ANIMALS, accessoryName);
                    DeactivateAnimal();
                    SpawnAnimal(accessoryName.ToAnimalType());
                    return;
            }
        }

        public void UpdateStatus(AccessoryType accessoryType, string accessoryName)
        {
            var accessoriesData = new List<AccessoryData>();
            
            switch (accessoryType)
            {
                case AccessoryType.SKINS:
                    accessoriesData = _progressService.PlayerProgress.PlayerProgressData.AccessoriesData.FindAll(accessory =>
                        accessory.AccessoryType == AccessoryType.SKINS);
                    break;
                case AccessoryType.ANIMALS:
                    accessoriesData = _progressService.PlayerProgress.PlayerProgressData.AccessoriesData.FindAll(accessory =>
                        accessory.AccessoryType == AccessoryType.ANIMALS);
                    break;
            }
            
            foreach (AccessoryData skin in accessoriesData)
            {
                if (skin.Name != accessoryName)
                {
                    skin.IsOn = false;
                }
            }
            
            _saveLoadService.UpdatePlayerPrefs();
        }

        public void UnequipAccessory(string accessoryName, AccessoryType accessoryType)
        {
            switch (accessoryType)
            {
                case AccessoryType.CLOTHES:
                    ChangeCloth(accessoryName, false);
                    return;
                case AccessoryType.SKINS:
                    ChangeSkin(StandartSkinMaterial);
                    return;
                case AccessoryType.ANIMALS:
                    DeactivateAnimal();
                    return;
            }
        }

        private void ChangeSkin(string skinName)
        {
            Material material = skinName switch
            {
                BlueSkinAccessory => BlueSkinMaterial,
                DirtySkinAccessory => DirtySkinMaterial,
                _ => StandartSkinMaterial
            };

            if (skinName == DirtySkinAccessory)
            {
                OnDirtSkinWornOn?.Invoke();
            }
            else
            {
                OnDirtSkinWornOff?.Invoke();
            }

            _playerSkin = material;
            foreach (SkinnedMeshRenderer meshRenderer in PlayerMeshRenderers)
                meshRenderer.material = material;
        }

        private void ChangeCloth(string clothName, bool value)
        {
            if (clothName == HatAccessory)
            {
                Hat.SetActive(value);
            }
        }

        public void LoadProgress(PlayerProgress progress)
        {
            _balance = progress.PlayerProgressData.Balance;

            foreach (AccessoryData accessoryData in progress.PlayerProgressData.AccessoriesData.Where(accessoryData =>
                         accessoryData.IsOn))
            {
                EquipAccessory(accessoryData.Name, accessoryData.AccessoryType);
            }
        }

        public void UpdateProgress(PlayerProgress progress)
        {
            progress.PlayerProgressData.Balance = _balance;
        }

        public void LightOn() =>
            Lightening.SetActive(true);

        private void DeactivateAnimal()
        {
            if (_currentAnimal != null)
            {
                Destroy(_currentAnimal.gameObject);
                _currentAnimal = null;
            }
        }

        private AnimalBase SpawnAnimal(AnimalType animalType)
        {
            AnimalBase animal = _gameFactory.SpawnAnimal(AnimalSpawnPoint.position, animalType.ToAnimalPath(), this);
            _currentAnimal = animal;

            return animal;
        }
    }
}