using System;
using Inputs.Services;
using Observer;
using Player;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Levels.Enemies
{
    public class EnemyAttack : Subject
    {
        private const float StopDistance = 1f;
        private const float AttackRadius = 0.5f;

        public Transform Following { get; set; }
        public bool Died { get; set; }

        public LayerMask AttackLayer;
        public NavMeshAgent Agent;
        public Transform AttackTransform;

        private EnemyAttackAnimator _enemyAnimator;
        private IUIInputService _uiInputService;
        private bool _stop;

        [Inject]
        private void Construct(IUIInputService uiInputService) => 
            _uiInputService = uiInputService;

        private void OnEnable()
        {
            _uiInputService.OnSettingsOpen += Stop;
            _uiInputService.OnSettingsClose += Go;
        }
        
        private void OnDisable()
        {
            _uiInputService.OnSettingsOpen -= Stop;
            _uiInputService.OnSettingsClose -= Go;
        }

        private void Start() => 
            _enemyAnimator = GetComponent<EnemyAttackAnimator>();

        private void Update()
        {
            if (!_stop)
            {
                Agent.destination = Following.transform.position;

                if (ReachedFollowing())
                {
                    Vector3 direction = Following.position - transform.position;
                    direction.y = 0;
                    Quaternion targetRotation = Quaternion.LookRotation(direction);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 2 * Time.deltaTime);

                    _enemyAnimator.PlayAttack();
                }
                else
                {
                    _enemyAnimator.Walk();
                }
            }
            else
            {
                Agent.destination = transform.position;
                _enemyAnimator.Idle();
            }
        }

        public void RemoveObservers() =>
            RemoveObserver(Following.GetComponentInParent<PlayerHealth>());

        public void Stop() => 
            _stop = true;

        public void Go()
        {
            if (_stop)
            {
                if (!Died)
                {
                    _stop = false;
                }
            }
        }

        public void CheckDamage()
        {
            var hits = new Collider[1];
            int overlaps = Physics.OverlapSphereNonAlloc(AttackTransform.position, AttackRadius, hits, AttackLayer);

            if (overlaps > 0)
                NotifyObservers();
        }


        private bool ReachedFollowing()
        {
            Vector3 agentPosition = new(Agent.transform.position.x, 0f, Agent.transform.position.z);
            Vector3 followingPosition = new(Following.transform.position.x, 0f, Following.transform.position.z);
            float distance = Vector3.Distance(agentPosition, followingPosition);

            return distance < StopDistance + 0.8f;
        }
    }
}