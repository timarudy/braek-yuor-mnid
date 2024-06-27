using System;
using UnityEngine.InputSystem;

namespace Inputs.Services
{
    class InputService : IInputService, InputActions.IAttackActions
    {
        public event Action OnHitAction;

        private readonly InputActions _input;

        public InputService()
        {
            _input = new InputActions();

            _input.Attack.SetCallbacks(this);
        }
        
        public void EnableAttackable() => 
            _input.Attack.Enable();

        public void DisableAttackable() => 
            _input.Attack.Disable();

        public void OnHit(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                OnHitAction?.Invoke();
            }
        }
    }
}