using InteractionManagement.Holdable;
using Levels.Animals;
using Levels.Coins;
using Levels.Common;
using Levels.Enemies;
using Levels.SecondLevel;
using Player;
using UnityEngine;

namespace Services.Factory
{
    public interface IGameFactory
    {
        // TComponent CreateAndBind<TComponent>(Vector3 at, string path) where TComponent : Object;
        TComponent CreateAndBind<TComponent>(Vector3 at, TComponent prefab) where TComponent : Object;
        TComponent Create<TComponent>(Vector3 at, TComponent prefab) where TComponent : Object;
        // TComponent Create<TComponent>(Vector3 at, string path) where TComponent : Object;
        HoldableBase CreateHoldableObject(Vector3 at, string path, Transform nativeTransform);
        AnimalBase SpawnAnimal(Vector3 at, string path, PlayerController playerController);
        CollectableBase SpawnCollectableObject(Vector3 at, string path, Transform parentTransform);
        EnemyAttack SpawnEnemy(EnemyType enemyType, Vector3 at, Transform parentTransform, PlayerFollow following);
    }
}