using System.Linq;
using Extensions;
using InteractionManagement.Attackable;
using Services.Factory;
using Services.Progress;
using UI.Windows;
using UnityEngine;
using Zenject;

namespace Levels.Enemies
{
    public abstract class EnemyBase : MonoBehaviour, IAttackable
    {
        private static readonly int Death = Animator.StringToHash("Die");

        public Transform SpawnObjectsNativeTransformParent;
        public EnemyHpBar HpBar;
        public GameObject Prefab;

        protected virtual int MaxHitCount => 3;
        protected abstract AnimalType AnimalType { get; }

        private EnemyAttack _enemyAttack;
        private IGameFactory _gameFactory;
        private Animator _animator;

        private IPersistentProgressService _progressService;
        private int _hitsCount;


        [Inject]
        private void Construct(IPersistentProgressService progressService, IGameFactory gameFactory)
        {
            _progressService = progressService;
            _gameFactory = gameFactory;
        }

        private void Start() =>
            OnStart();

        protected virtual void OnStart()
        {
            _enemyAttack = GetComponent<EnemyAttack>();
            _animator = GetComponent<Animator>();
        }

        public bool GetHit()
        {
            _hitsCount++;
            HpBar.SetHp(MaxHitCount - _hitsCount, MaxHitCount);
            if (_hitsCount == MaxHitCount)
            {
                FinalHit();
                return true;
            }

            PlayHitAnimation();

            return false;
        }

        protected virtual void FinalHit()
        {
            _enemyAttack.Stop();
            Destroy(HpBar.gameObject);
            PlayDeathAnimation();
        }

        private bool HasAnimal(AnimalType animalType) =>
            _progressService.PlayerProgress.PlayerProgressData.AccessoriesData.FindAll(accessory => accessory.AccessoryType == AccessoryType.ANIMALS).Any(animal =>
                animal.Name.ToAnimalType() == animalType);

        private void Die()
        {
            if (!HasAnimal(AnimalType))
            {
                _gameFactory.SpawnCollectableObject(transform.position, AnimalType.ToAnimalCoinPath(),
                    SpawnObjectsNativeTransformParent);
            }

            Flatten();
            GetComponent<BoxCollider>().enabled = false;
            _enemyAttack.Died = true;
            _enemyAttack.RemoveObservers();
        }

        private void Flatten() => 
            Prefab.transform.localScale = new Vector3(1, 0.1f, 1);

        private void PlayDeathAnimation() =>
            _animator.SetTrigger(Death);

        protected abstract void PlayHitAnimation();
    }
}