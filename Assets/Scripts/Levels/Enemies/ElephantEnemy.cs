using UI.Windows;
using UnityEngine;

namespace Levels.Enemies
{
    public class ElephantEnemy : EnemyBase
    {
        protected override int MaxHitCount => 5;
        protected override AnimalType AnimalType => AnimalType.ELEPHANT;

        protected override void PlayHitAnimation()
        {
            Debug.Log("Hit");
        }
    }
}