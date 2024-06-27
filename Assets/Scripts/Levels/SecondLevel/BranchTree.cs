using System;
using Infrastructure.AssetManagement;
using InteractionManagement.Attackable;
using InteractionManagement.Holdable;
using UnityEngine;

namespace Levels.SecondLevel
{
    public class BranchTree : AttackableBase
    {
        private static readonly int Shake = Animator.StringToHash("Shake");

        protected override int MaxHitCount => 2;

        protected override void Hit()
        {
            HoldableBase holdable = GameFactory.CreateHoldableObject(
                at: transform.position + 2 * Vector3.up,
                path: AssetPath.BranchPath,
                SpawnObjectsNativeTransformParent
            );

            holdable.gameObject.transform.rotation = Quaternion.Euler(-93, 0, 0);

            DestroySelf();
        }

        protected override void PlayHitAnimation() =>
            ShakeTree();

        private void ShakeTree() =>
            Animator.SetTrigger(Shake);
    }
}