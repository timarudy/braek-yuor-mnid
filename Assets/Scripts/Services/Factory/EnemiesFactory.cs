using Extensions;
using Infrastructure.AssetManagement;
using Levels.Enemies;
using Player;
using UnityEngine;
using Zenject;

namespace Services.Factory
{
    public class EnemiesFactory : IEnemiesFactory
    {
        private readonly DiContainer _diContainer;
        private readonly IAssetProvider _assetProvider;

        public EnemiesFactory(DiContainer diContainer, IAssetProvider assetProvider)
        {
            _diContainer = diContainer;
            _assetProvider = assetProvider;
        }

        public EnemyAttack SpawnEnemy(EnemyType enemyType, Vector3 at, Transform parentTransform,
            PlayerFollow following)
        {
            return enemyType switch
            {
                EnemyType.CHICKEN => CreateEnemy(at, parentTransform, following, AssetPath.Enemy_ChickenPath),
                EnemyType.LAMA => CreateEnemy(at, parentTransform, following, AssetPath.Enemy_LamaPath),
                EnemyType.ELEPHANT => CreateEnemy(at, parentTransform, following, AssetPath.Enemy_ElephantPath),
                _ => null
            };
        }

        private EnemyAttack CreateEnemy(Vector3 at, Transform parentTransform, PlayerFollow following, string path)
        {
            EnemyAttack enemy = _diContainer
                .InstantiatePrefabForComponent<EnemyAttack>(
                    prefab: _assetProvider.LoadResource<EnemyAttack>(path),
                    position: at,
                    Quaternion.identity,
                    parentTransform
                );

            enemy.Following = following.transform;
            enemy.AddObserver(following.GetComponentInParent<PlayerHealth>());
            SceneExtensions.MoveToActiveScene(enemy);

            return enemy;
        }
    }
}