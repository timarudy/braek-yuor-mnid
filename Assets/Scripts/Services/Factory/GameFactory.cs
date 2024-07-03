using Extensions;
using Infrastructure.AssetManagement;
using InteractionManagement.Holdable;
using Levels.Animals;
using Levels.Coins;
using Levels.Common;
using Levels.Enemies;
using Levels.SecondLevel;
using Player;
using Player.Movement;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Services.Factory
{
    public class GameFactory : IGameFactory
    {
        private readonly IAssetProvider _assetProvider;
        private readonly DiContainer _diContainer;

        public GameFactory(IAssetProvider assetProvider, DiContainer diContainer)
        {
            _assetProvider = assetProvider;
            _diContainer = diContainer;
        }

        public TComponent CreateAndBind<TComponent>(Vector3 at, TComponent prefab) where TComponent : Object
        {
            if (_diContainer.HasBinding<TComponent>()) _diContainer.Unbind<TComponent>();

            TComponent component = _diContainer
                .InstantiatePrefabForComponent<TComponent>(
                    prefab: prefab,
                    position: at,
                    Quaternion.identity,
                    null
                );

            SceneExtensions.MoveToActiveScene(component);

            _diContainer.Bind<TComponent>().FromInstance(component).AsTransient();

            return component;
        }

        public TComponent Create<TComponent>(Vector3 at, TComponent prefab) where TComponent : Object
        {
            TComponent component = _diContainer
                .InstantiatePrefabForComponent<TComponent>(
                    prefab: prefab,
                    position: at,
                    Quaternion.identity,
                    null
                );

            SceneExtensions.MoveToActiveScene(component);

            return component;
        }

        public HoldableBase CreateHoldableObject(Vector3 at, string path, Transform nativeTransform)
        {
            HoldableBase holdable = _diContainer
                .InstantiatePrefabForComponent<HoldableBase>(
                    prefab: _assetProvider.LoadResource<HoldableBase>(path),
                    position: at,
                    Quaternion.identity,
                    nativeTransform
                );

            holdable.SetNativeTransform(nativeTransform);
            SceneExtensions.MoveToActiveScene(holdable);

            return holdable;
        }

        public AnimalBase SpawnAnimal(Vector3 at, string path, PlayerController following)
        {
            AnimalBase animal = _diContainer
                .InstantiatePrefabForComponent<AnimalBase>(
                    prefab: _assetProvider.LoadResource<AnimalBase>(path),
                    position: at,
                    Quaternion.identity,
                    null
                );

            animal.Following = following.GetComponentInChildren<PlayerFollow>();
            SceneExtensions.MoveToActiveScene(animal);

            return animal;
        }

        public CollectableBase SpawnCollectableObject(Vector3 at, string path, Transform parentTransform)
        {
            CollectableBase collectable = _diContainer
                .InstantiatePrefabForComponent<CollectableBase>(
                    prefab: _assetProvider.LoadResource<CollectableBase>(path),
                    position: at,
                    Quaternion.identity,
                    parentTransform
                );

            collectable.Fall();
            SceneExtensions.MoveToActiveScene(collectable);

            return collectable;
        }
    }
}