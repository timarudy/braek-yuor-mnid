using UnityEngine;

namespace Player
{
    public class PlayerAnimator : MonoBehaviour
    {
        [SerializeField] private Animator Animator;

        private static readonly int IsGrounded = Animator.StringToHash("IsGrounded");
        private static readonly int IsFalling = Animator.StringToHash("IsFalling");
        private static readonly int Attack = Animator.StringToHash("Attack");
        private static readonly int Speed = Animator.StringToHash("Speed");
        private static readonly int IsHolding = Animator.StringToHash("IsHolding");
        private static readonly int Jumping = Animator.StringToHash("Jump");
        private static readonly int IsReading = Animator.StringToHash("IsReading");
        private static readonly int IsWatering = Animator.StringToHash("IsWatering");
        private static readonly int Death = Animator.StringToHash("Death");

        public void PlayAttack() =>
            Animator.SetTrigger(Attack);

        public void Jump() =>
            Animator.SetTrigger(Jumping);

        public void Ground(bool value) =>
            Animator.SetBool(IsGrounded, value);

        public void Fall(bool value) =>
            Animator.SetBool(IsFalling, value);

        public void Die() =>
            Animator.SetTrigger(Death);

        public void Idle() =>
            Animator.SetFloat(Speed, 0f, 0.1f, Time.deltaTime);

        public void Walk() =>
            Animator.SetFloat(Speed, 0.5f, 0.1f, Time.deltaTime);

        public void Run() =>
            Animator.SetFloat(Speed, 1f, 0.1f, Time.deltaTime);

        public void Read(bool value) =>
            Animator.SetBool(IsReading, value);

        public void Water(bool value) =>
            Animator.SetBool(IsWatering, value);

        public bool GetBool(int hash) =>
            Animator.GetBool(hash);

        public void Hold() =>
            Animator.SetBool(IsHolding, true);

        public void Unhold() =>
            Animator.SetBool(IsHolding, false);
    }
}