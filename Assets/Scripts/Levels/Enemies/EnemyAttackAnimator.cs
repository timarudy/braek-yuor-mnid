using Levels.Animals;
using UnityEngine;

namespace Levels.Enemies
{
    public class EnemyAttackAnimator : AnimalAnimator
    {
        private static readonly int Attack = Animator.StringToHash("Attack");

        public void PlayAttack() =>
            Animator.SetTrigger(Attack);
    }
}