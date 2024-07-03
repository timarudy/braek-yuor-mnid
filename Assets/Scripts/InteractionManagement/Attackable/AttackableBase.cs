using Services.Factory;
using UnityEngine;
using Zenject;

namespace InteractionManagement.Attackable
{
    public abstract class AttackableBase : MonoBehaviour, IAttackable
    {
        public Transform SpawnObjectsNativeTransformParent;

        protected virtual int MaxHitCount => 3;

        protected IGameFactory GameFactory;
        protected Animator Animator;
        
        private int _hitsCount;


        [Inject]
        private void Construct(IGameFactory gameFactory)
        {
            GameFactory = gameFactory;
        }

        private void Start() =>
            OnStart();

        protected virtual void OnStart()
        {
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
            
        }

        protected abstract void PlayHitAnimation();
    }
}