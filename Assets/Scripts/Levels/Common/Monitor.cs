using System;
using Configs;
using Infrastructure.AssetManagement;
using Inputs;
using Inputs.Services;
using InteractionManagement;
using Player;
using Services.Progress;
using Services.Progress.SaveLoadService;
using Services.SceneServices;
using Services.StaticData;
using TMPro;
using UI.PopUps;
using UI.Windows;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Levels.Common
{
    public class Monitor : MonoBehaviour, IInteractable, IWorldCanvasInput
    {
        [SerializeField] private TMP_InputField InputField;
        [SerializeField] private ToolTip ToolTip;
        [SerializeField] private BoxCollider TriggerBox;

        public bool IsInteractable { get; set; } = true;

        private InputProgressState _inputState = InputProgressState.DISABLED;
        private BlockHandler _block;
        private IUIInputService _uiInput;
        private ILevelDataService _levelData;
        private IStaticDataService _staticDataService;
        private ISaveLoadService _saveLoadService;
        private PlayerController _player;
        private ToolTipVisibilityHandler _toolTipHandler;
        private IAssetProvider _assetProvider;

        [Inject]
        private void Construct(BlockHandler block, IUIInputService uiInput, ILevelDataService levelData,
            IStaticDataService staticDataService, ISaveLoadService saveLoadService, PlayerController player,
            IAssetProvider assetProvider)
        {
            _assetProvider = assetProvider;
            _player = player;
            _saveLoadService = saveLoadService;
            _staticDataService = staticDataService;
            _block = block;
            _uiInput = uiInput;
            _levelData = levelData;
        }

        private void Awake()
        {
            InputField.interactable = false;
        }

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

        public void Interact(Interactor interactor)
        {
            EnableUserInput();
        }

        public void OpenTooltip(Interactor interactor)
        {
            _toolTipHandler.OpenToolTip();
        }

        public void CloseTooltip(Interactor interactor)
        {
            _toolTipHandler.CloseToolTip();
        }

        public void DisableUserInput()
        {
            if (_inputState != InputProgressState.ENABLED) return;

            if (InputField.text == _levelData.GetLevelData(SceneManager.GetActiveScene().name).LevelCode)
            {
                _inputState = InputProgressState.FINISHED;
                InputField.DeactivateInputField();
                InputField.interactable = false;

                ToolTip.Close();
                ToolTip.SwitchOff();
                _block.Close();

                Time.timeScale = 1f;

                _uiInput.EnableNotepad();

                if (_levelData.GetLevelData(SceneManager.GetActiveScene().name).Id != _assetProvider.LoadResource<SOLevels>(AssetPath.LevelsDataPath).LevelsData[^1].Id)
                {
                    if (!_levelData.GetLevelData(_levelData.GetLevelData(SceneManager.GetActiveScene().name).Id + 1)
                            .IsOpenedLevel)
                    {
                        _player.AddMoney(10);
                        _staticDataService.OpenNewLevel(_levelData.GetLevelData(SceneManager.GetActiveScene().name).Id);
                        _saveLoadService.SaveProgress(_staticDataService as ISavedProgress);
                    }
                }
                
                TriggerBox.enabled = false;
            }
            else
            {
                InputField.DeactivateInputField();
                InputField.interactable = false;
                ToolTip.Open();

                Time.timeScale = 1f;

                _inputState = InputProgressState.DISABLED;
            }
        }

        public void EnableUserInput()
        {
            if (_inputState == InputProgressState.DISABLED)
            {
                InputField.interactable = true;
                InputField.Select();
                InputField.ActivateInputField();
                ToolTip.Close();

                Time.timeScale = 0f;

                _inputState = InputProgressState.ENABLED;
            }
        }
    }
}