using System;
using UI.Factory;
using UI.Service;
using UI.Windows;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Inputs.Services
{
    public class UIInputService : IUIInputService, UIInputActions.INotepadActions,
        UIInputActions.IWorldSpaceInputActions,
        UIInputActions.IWindowsActions
    {
        public event Action OpenNotepadEvent;
        public event Action CloseNotepadEvent;
        public event Action EnableInteractableInput;
        public event Action DisableInteractableInput;
        public event Action OnSettingsOpen;
        public event Action OnSettingsClose;

        public bool IsWindowOpened { get; set; }
        public bool IsNotepadOpened { get; set; }

        private readonly UIInputActions _uiInput;
        private readonly IWindowService _windowService;
        private readonly IInputService _inputService;


        public UIInputService(IWindowService windowService, IInputService inputService)
        {
            _inputService = inputService;
            _windowService = windowService;

            _uiInput = new UIInputActions();

            _uiInput.Notepad.SetCallbacks(this);
            _uiInput.WorldSpaceInput.SetCallbacks(this);
            _uiInput.Windows.SetCallbacks(this);
        }

        public void EnableWindows() =>
            _uiInput.Windows.Enable();

        public void EnableWorldInput() =>
            _uiInput.WorldSpaceInput.Enable();

        public void DisableWorldInput() =>
            _uiInput.WorldSpaceInput.Disable();

        public void EnableNotepad() =>
            _uiInput.Notepad.Enable();

        public void DisableNotepad() => 
            _uiInput.Notepad.Disable();

        public void DisableAllInputs()
        {
            _uiInput.WorldSpaceInput.Disable();
            _uiInput.Notepad.Disable();
            _uiInput.Windows.Disable();
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                EnableInteractableInput?.Invoke();
            }
        }

        public void OnExit(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                DisableInteractableInput?.Invoke();
            }
        }

        public void OnSettings(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                if (_windowService.CurrentWindow == null)
                {
                    _windowService.Open(WindowType.SETTINGS);
                    OpenSettings();
                }
                else
                {
                    _windowService.CloseCurrentWindow();
                    CloseSettings();
                }

                IsWindowOpened = !IsWindowOpened;
            }
        }

        public void CloseSettings()
        {
            OnSettingsClose?.Invoke();
            // EnableNotepad();
            EnableWorldInput();
        }

        public void OpenSettings()
        {
            OnSettingsOpen?.Invoke();
            _inputService.DisableAttackable();
            DisableNotepad();
            DisableWorldInput();
        }

        public void OnSwitch(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                if (!IsNotepadOpened)
                {
                    OpenNotepadEvent?.Invoke();
                }
                else
                {
                    CloseNotepadEvent?.Invoke();
                }

                IsNotepadOpened = !IsNotepadOpened;
            }
        }
    }
}