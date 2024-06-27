using Extensions;
using InteractionManagement.Attackable;
using UI.Windows;
using UnityEngine;

namespace Levels.Enemies
{
    public class LamaEnemy : EnemyBase
    {
        protected override int MaxHitCount => 4;
        protected override AnimalType AnimalType => AnimalType.LAMA;

        protected override void PlayHitAnimation()
        {
            Debug.Log("Hit");
        }
    }
}