using System;
using Inputs.Services;
using UI.Service;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Player.Input
{
    public class StarterAssetsInputs : MonoBehaviour
    {
        public event Action<PersonView> ChangeViewEvent;

        [Header("Character Input Values")] public Vector2 move;
        public Vector2 look;
        public bool jump;
        public bool sprint;
        public PersonView view = PersonView.FIRST;

        [Header("Movement Settings")] public bool analogMovement;

        [Header("Mouse Cursor Settings")] public bool cursorLocked = true;
        public bool cursorInputForLook = true;

        private IUIInputService _uiInputService;
        private PlayerInput _playerInput;
        private IWindowService _windowService;

        [Inject]
        private void Construct(IUIInputService uiInputService, IWindowService windowService)
        {
            _uiInputService = uiInputService;
            _windowService = windowService;
        }

        private void OnEnable()
        {
            _uiInputService.OnWindowOpen += DisableInput;
            _uiInputService.OnWindowClose += EnableInput;
        }

        private void OnDisable()
        {
            _uiInputService.OnWindowOpen -= DisableInput;
            _uiInputService.OnWindowClose -= EnableInput;
        }

        private void Start()
        {
            _playerInput = GetComponent<PlayerInput>();
        }

#if ENABLE_INPUT_SYSTEM
        public void OnMove(InputValue value)
        {
            MoveInput(value.Get<Vector2>());
        }

        public void OnLook(InputValue value)
        {
            if (cursorInputForLook)
            {
                LookInput(value.Get<Vector2>());
            }
        }

        public void OnJump(InputValue value)
        {
            JumpInput(value.isPressed);
        }

        public void OnSprint(InputValue value)
        {
            SprintInput(value.isPressed);
        }

        public void OnChangeView(InputValue value)
        {
            ChangeViewInput(view == PersonView.FIRST ? PersonView.THIRD : PersonView.FIRST);
            ChangeViewEvent?.Invoke(view);
        }

#endif


        public void MoveInput(Vector2 newMoveDirection)
        {
            move = newMoveDirection;
        }

        public void LookInput(Vector2 newLookDirection)
        {
            look = newLookDirection;
        }

        public void JumpInput(bool newJumpState)
        {
            if (Time.timeScale != 0f)
                jump = newJumpState;
        }

        public void SprintInput(bool newSprintState)
        {
            sprint = newSprintState;
        }

        public void ChangeViewInput(PersonView newViewState)
        {
            view = newViewState;
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            SetCursorState(cursorLocked);
        }

        private void SetCursorState(bool newState)
        {
            Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
        }

        private void DisableInput() =>
            _playerInput.DeactivateInput();

        private void EnableInput()
        {
            if (!_uiInputService.IsNotepadOpened && _windowService.CurrentWindow == null)
            {
                _playerInput.ActivateInput();
            }
        }
    }
}