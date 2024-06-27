using System;
using UI.Windows;
using UnityEngine;

namespace Levels.Enemies
{
    public class ChickenEnemy : EnemyBase
    {
        protected override int MaxHitCount => 3;
        protected override AnimalType AnimalType => AnimalType.CHICKEN;
        
        protected override void PlayHitAnimation()
        {
            Debug.Log("Hit");
        }
    }
}