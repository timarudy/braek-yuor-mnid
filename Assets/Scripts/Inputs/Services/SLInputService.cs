using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Inputs.Services
{
    public class SLInputService : ISLInputService,
        SLInputActions.IWateringCanActions, SLInputActions.ICraftTableActions
    {
        public event Action OnCraftStart;
        public event Action OnCraftCancelled;
        public event Action OnWateringStarted;
        public event Action OnWateringCancelled;

        private readonly SLInputActions _slInput;

        public SLInputService()
        {
            _slInput = new SLInputActions();

            _slInput.WateringCan.SetCallbacks(this);
            _slInput.CraftTable.SetCallbacks(this);
        }

        public void SetWateringCan() => 
            _slInput.WateringCan.Enable();

        public void SetCraftTable() =>
            _slInput.CraftTable.Enable();

        public void DisableWateringCan() => 
            _slInput.WateringCan.Disable();

        public void DisableCraftableTable() =>
            _slInput.CraftTable.Disable();

        public void OnWater(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Started)
            {
                OnWateringStarted?.Invoke();
            }
            else if (context.phase == InputActionPhase.Canceled)
            {
                OnWateringCancelled?.Invoke();
            }
        }

        public void OnMakeAxe(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Started)
            {
                OnCraftStart?.Invoke();
            }
            else if (context.phase == InputActionPhase.Canceled)
            {
                OnCraftCancelled?.Invoke();
            }
        }
    }
}