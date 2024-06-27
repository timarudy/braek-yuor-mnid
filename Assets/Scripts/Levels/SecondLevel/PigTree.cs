using System;
using System.Linq;
using Extensions;
using Infrastructure.AssetManagement;
using InteractionManagement.Attackable;
using Services.LevelAccess;
using Services.Progress;
using UI.Windows;
using UnityEngine;
using Zenject;

namespace Levels.SecondLevel
{
    public class PigTree : AttackableBase
    {
        private static readonly int Shake = Animator.StringToHash("Shake");

        [SerializeField] private GameObject Pig;

        private ICodeAccessChecker _codeAccessChecker;
        private IPersistentProgressService _progressService;

        [Inject]
        private void Construct(ICodeAccessChecker codeAccessChecker, IPersistentProgressService progressService)
        {
            _progressService = progressService;
            _codeAccessChecker = codeAccessChecker;
        }

        private void Awake()
        {
            if (HasPigAnimal())
                Pig.SetActive(false);
        }

        protected override void Hit()
        {
            if (!HasPigAnimal())
                GameFactory.SpawnCollectableObject(transform.position + Vector3.up, AnimalType.PIG.ToAnimalCoinPath(),
                    null);

            _codeAccessChecker.IsPigRescued = true;

            DestroySelf();
        }

        private bool HasPigAnimal() =>
            _progressService.PlayerProgress.PlayerProgressData.AccessoriesData
                .FindAll(accessory => accessory.AccessoryType == AccessoryType.ANIMALS).Any(animal =>
                    animal.Name.ToAnimalType() == AnimalType.PIG);

        protected override void PlayHitAnimation() =>
            ShakeTree();

        private void ShakeTree() =>
            Animator.SetTrigger(Shake);
    }
}