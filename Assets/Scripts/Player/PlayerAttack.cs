using Inputs.Services;
using InteractionManagement;
using InteractionManagement.Attackable;
using Player.Movement;
using Services.SceneServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Zenject;

namespace Player
{
    public class PlayerAttack : MonoBehaviour
    {
        public float AttackRadius = 0.5f;
        
        [FormerlySerializedAs("AttackPosition")]
        public Transform AttackTransform;

        public bool InAttackZone { get; set; }
        
        private PlayerAnimator _playerAnimator;
        private IInputService _inputService;
        private readonly Collider[] _hits = new Collider[1];

        private bool _isAttacking;
        private FirstPersonMovement _playerMovement;

        [Inject]
        private void Construct(IInputService inputService)
        {
            _inputService = inputService;
        }

        private void OnEnable()
        {
            _inputService.OnHitAction += Attack;
        }

        private void OnDisable()
        {
            _inputService.OnHitAction -= Attack;
        }

        private void Start()
        {
            _playerAnimator = GetComponent<PlayerAnimator>();
            _playerMovement = GetComponentInChildren<FirstPersonMovement>();
        }

        // private void OnDrawGizmos()
        // {
        //     Gizmos.color = Color.red;
        //     Gizmos.DrawSphere(AttackTransform.position, AttackRadius);
        // }

        private void Attack()
        {
            if (!_isAttacking)
            {
                if (_playerMovement.enabled)
                {
                    _playerMovement.StartAttacking();
                    _playerMovement.FollowHead();
                }

                _playerAnimator.PlayAttack();
            }
        }

        private void OnAttackStarted()
        {
            if (!_isAttacking)
            {
                _isAttacking = true;
            }
        }

        private void OnAttackEnded()
        {
            if (_isAttacking)
            {
                if (_playerMovement.enabled)
                {
                    _playerMovement.StopAttacking();
                    _playerMovement.UnfollowHead();
                }
                
                AttackBase attack = GetComponent<Interactor>().GetHoldableObject() as AttackBase;
                
                if (IsHit(out IAttackable hit, attack))
                {
                    bool isHit = hit.GetHit();
                    attack.PlayHitSound(GetComponent<PlayerHealth>().AudioSource);

                    if (isHit)
                    {
                        _hits[0] = null;
                    }
                }

                _isAttacking = false;
            }
        }

        private bool IsHit(out IAttackable hit, AttackBase attack)
        {
            int hitCount = 0;
            if (attack != null)
            {
                LayerMask attackLayer = attack.AttackLayer;
                hitCount = Physics.OverlapSphereNonAlloc(AttackTransform.position, AttackRadius, _hits, attackLayer);
            }

            hit = _hits[0]?.GetComponent<IAttackable>();

            return hitCount > 0 && InAttackZone;
        }

    }
}