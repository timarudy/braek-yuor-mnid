using System;
using System.Collections.Generic;
using InteractionManagement.Attackable;
using InteractionManagement.Holdable;
using Levels.Animals;
using Player;
using Services.Factory;
using Services.Progress;
using UnityEngine;
using Zenject;

namespace Levels.Enemies
{
    public class EnemiesTrigger : MonoBehaviour
    {
        private const string EnemySpawnersTag = "EnemySpawner";

        private readonly List<EnemyAttack> _enemies = new();
        private IGameFactory _gameFactory;
        private IPersistentProgressService _progressService;
        private bool _isSpawned;

        [Inject]
        private void Construct(IGameFactory gameFactory, IPersistentProgressService progressService)
        {
            _gameFactory = gameFactory;
            _progressService = progressService;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out IHoldable _) || other.TryGetComponent(out AnimalBase _)) return;
            
            PlayerFollow following = other.GetComponentInChildren<PlayerFollow>();
            other.GetComponentInParent<PlayerAttack>().InAttackZone = true;
            if (!_isSpawned)
            {
                var enemySpawners = GameObject.FindGameObjectsWithTag(EnemySpawnersTag);

                foreach (GameObject spawner in enemySpawners)
                {
                    if (!_progressService.PlayerProgress.LevelProgressData.KillData.ClearedSpawners.Contains(
                            spawner.GetComponent<UniqueId>().Id))
                    {
                        EnemyAttack enemy = _gameFactory.SpawnEnemy(
                            spawner.GetComponent<EnemySpawner>().EnemyType,
                            spawner.transform.position,
                            null,
                            following);

                        enemy.GetComponent<EnemyBase>().SpawnObjectsNativeTransformParent =
                            spawner.GetComponent<EnemySpawner>().SpawnObjectsNativeTransformParent;

                        _enemies.Add(enemy);
                    }
                }

                _isSpawned = true;
            }
            else
            {
                foreach (EnemyAttack enemy in _enemies)
                {
                    enemy.Go();
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out IHoldable _) || other.TryGetComponent(out AnimalBase _)) return;
            
            other.GetComponentInParent<PlayerAttack>().InAttackZone = false;
            if (_isSpawned)
            {
                foreach (EnemyAttack enemy in _enemies)
                {
                    enemy.Stop();
                }
            }
        }
    }
}