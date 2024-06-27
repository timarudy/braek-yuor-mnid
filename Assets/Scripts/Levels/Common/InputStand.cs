using Configs;
using Extensions;
using Infrastructure.AssetManagement;
using Inputs.Services;
using InteractionManagement;
using Player;
using Services.LevelAccess;
using TMPro;
using UI.PopUps;
using UnityEngine;
using Zenject;

namespace Levels.Common
{
    public abstract class InputStand : MonoBehaviour, IInteractable, IWorldCanvasInput
    {
        [SerializeField] private TMP_InputField InputField;
        [SerializeField] private ToolTip ToolTip;
        [SerializeField] private BoxCollider TriggerBox;

        public int Id { get; set; }
        public bool IsInteractable { get; set; } = true;

        protected IAssetProvider AssetProvider;
        protected ICodeAccessChecker CodeAccessChecker;

        private InputProgressState _inputState = InputProgressState.DISABLED;
        private IUIInputService _uiInput;
        private ToolTipVisibilityHandler _toolTipHandler;
        private Interactor _interactor;

        [Inject]
        private void Construct(IAssetProvider assetProvider, IUIInputService uiInput,
            ICodeAccessChecker codeAccessChecker, PlayerController player)
        {
            CodeAccessChecker = codeAccessChecker;
            AssetProvider = assetProvider;
            _uiInput = uiInput;
            _interactor = player.GetComponent<Interactor>();
        }

        private void Awake()
        {
            OnAwake();
        }

        protected virtual void OnAwake() =>
            InputField.interactable = false;

        private void OnEnable()
        {
            _uiInput.DisableInteractableInput += DisableUserInput;
        }

        private void OnDisable()
        {
            _uiInput.DisableInteractableInput -= DisableUserInput;
        }

        private void Start()
        {
            _toolTipHandler = GetComponent<ToolTipVisibilityHandler>();
        }

        public void Interact(Interactor interactor) =>
            EnableUserInput();

        public void OpenTooltip(Interactor interactor) =>
            _toolTipHandler.OpenToolTip();

        public void CloseTooltip(Interactor interactor) =>
            _toolTipHandler.CloseToolTip();

        public void DisableUserInput()
        {
            if (_inputState != InputProgressState.ENABLED) return;

            if (StringExtensions.IsEqualStrings(InputField.text, GetInputStandData().Decryption))
            {
                DecryptionAccepted();
            }
            else
            {
                DecryptionRejected();
            }

            if (!_interactor.HasHoldableObject()) 
                _uiInput.EnableNotepad();
        }

        public void CloseUserInput()
        {
            InputField.DeactivateInputField();
            InputField.interactable = false;

            ToolTip.Close();
            ToolTip.SwitchOff();

            TriggerBox.enabled = false;
        }

        private void DecryptionAccepted()
        {
            _inputState = InputProgressState.FINISHED;
            InputField.DeactivateInputField();
            InputField.interactable = false;

            ToolTip.Close();
            ToolTip.SwitchOff();

            Time.timeScale = 1f;

            CheckAccess();

            // _uiInput.EnableNotepad();

            TriggerBox.enabled = false;
        }


        protected virtual void DecryptionRejected()
        {
            InputField.DeactivateInputField();
            InputField.interactable = false;
            ToolTip.Open();

            Time.timeScale = 1f;

            _inputState = InputProgressState.DISABLED;
        }

        public void EnableUserInput()
        {
            if (_inputState == InputProgressState.DISABLED)
            {
                InputField.interactable = true;
                InputField.Select();
                InputField.ActivateInputField();
                ToolTip.Close();

                _uiInput.DisableNotepad();

                Time.timeScale = 0f;

                _inputState = InputProgressState.ENABLED;
            }
        }

        public abstract InputStandData GetInputStandData();
        protected abstract void CheckAccess();
    }
}