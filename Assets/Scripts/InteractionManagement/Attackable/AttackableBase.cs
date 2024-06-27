using Services.Factory;
using UnityEngine;
using Zenject;

namespace InteractionManagement.Attackable
{
    public abstract class AttackableBase : MonoBehaviour, IAttackable
    {
        // private static readonly int Death = Animator.StringToHash("Die");

        public Transform SpawnObjectsNativeTransformParent;

        protected virtual int MaxHitCount => 3;
        // protected abstract AnimalType AnimalType { get; set; }

        // protected EnemyAttack EnemyAttack;
        protected IGameFactory GameFactory;
        protected Animator Animator;

        // private IPersistentProgressService _progressService;
        private int _hitsCount;


        [Inject]
        private void Construct(IGameFactory gameFactory)
        {
            // _progressService = progressService;
            GameFactory = gameFactory;
        }

        private void Start() =>
            OnStart();

        protected virtual void OnStart()
        {
            // EnemyAttack = GetComponent<EnemyAttack>();
            Animator = GetComponent<Animator>();
        }

        public bool GetHit()
        {
            _hitsCount++;
            if (_hitsCount == MaxHitCount)
            {
                Hit();
                return true;
            }

            PlayHitAnimation();

            return false;
        }

        protected void DestroySelf() =>
            Destroy(gameObject);

        protected virtual void Hit()
        {
            // EnemyAttack.Stop();
            // PlayDeathAnimation();
        }

        // protected bool HasAnimal(AnimalType animalType) =>
        //     _progressService.PlayerProgress.PlayerProgressData.AnimalsData.Any(animal =>
        //         animal.Name == animalType);

        // private void Flatten() =>
        //     transform.localScale = new Vector3(1, 0.1f, 1);

        // private void Die()
        // {
        //     if (!HasAnimal(AnimalType))
        //     {
        //         GameFactory.SpawnCollectableObject(transform.position, AnimalType.ToAnimalPath(),
        //             SpawnObjectsNativeTransformParent);
        //     }
        //
        //     Flatten();
        //     EnemyAttack.Died = true;
        // }

        // private void PlayDeathAnimation() =>
        //     Animator.SetTrigger(Death);

        protected abstract void PlayHitAnimation();
    }
}