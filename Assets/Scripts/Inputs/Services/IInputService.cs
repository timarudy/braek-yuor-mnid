using System;

namespace Inputs.Services
{
    public interface IInputService
    {
        event Action OnHitAction;
        void EnableAttackable();
        void DisableAttackable();
    }
}