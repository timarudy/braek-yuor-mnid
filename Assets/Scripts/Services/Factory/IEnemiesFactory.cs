using Levels.Enemies;
using Player;
using UnityEngine;

namespace Services.Factory
{
    public interface IEnemiesFactory
    {
        EnemyAttack SpawnEnemy(EnemyType enemyType, Vector3 at, Transform parentTransform,
            PlayerFollow following);
    }
}