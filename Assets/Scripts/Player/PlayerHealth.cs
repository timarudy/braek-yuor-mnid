using System.Collections;
using Inputs.Services;
using InteractionManagement;
using Levels.Common;
using Levels.Enemies;
using Observer;
using Player.Movement;
using SoundManagement;
using UI;
using UI.Factory;
using UI.Service;
using UI.Windows;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Player
{
    public class PlayerHealth : MonoBehaviour, IObserver
    {
        public AudioSource AudioSource;
        
        [SerializeField] private int MaxHp = 10;
        [SerializeField] private Transform CameraDestination;

        private ISoundService _soundService;
        private IUIFactory _uiFactory;
        private HpBar _hpBar;
        private int _currentHp;
        private PlayerController _playerController;
        private PlayerAnimator _playerAnimator;
        private PlayerInput _playerInput;
        private Interactor _playerInteractor;
        private IUIInputService _uiInputService;
        private IWindowService _windowService;
        private GameObject _currentCameraFollow;
        private ViewHandler _viewHandler;

        [Inject]
        private void Construct(ISoundService soundService, IUIFactory uiFactory, IUIInputService uiInputService,
            IWindowService windowService)
        {
            _uiFactory = uiFactory;
            _uiFactory.OnHpBarCreated += SetHpBar;
            _soundService = soundService;
            _uiInputService = uiInputService;
            _windowService = windowService;
        }

        private void Start()
        {
            _currentHp = MaxHp;
            _playerController = GetComponent<PlayerController>();
            _playerAnimator = GetComponent<PlayerAnimator>();
            _playerInput = GetComponentInChildren<PlayerInput>();
            _playerInteractor = GetComponent<Interactor>();
            _viewHandler = GetComponentInChildren<ViewHandler>();

            _viewHandler.OnCameraFollowChanged += ChangeCameraFollow;
        }

        private void OnDisable() =>
            _viewHandler.OnCameraFollowChanged -= ChangeCameraFollow;

        public void OnNotify(Subject sub)
        {
            GetDamage(1);
            
            if (_currentHp <= 0) 
                sub.GetComponent<EnemyAttack>().Stop();
        }

        public void GetDamage(int power)
        {
            if (_currentHp > power)
            {
                _soundService.PlayFailSound(AudioSource);
                _currentHp -= power;
                _hpBar.SetHp(_currentHp, MaxHp);
            }
            else
            {
                _currentHp = 0;
                _hpBar.SetHp(_currentHp, MaxHp);
                Kill();
            }
        }

        private void Kill()
        {
            _playerInput.enabled = false;
            _playerAnimator.Die();
            _playerController.ShowHair();

            InputStand stand = _playerInteractor.GetCurrentInteractableObject() as InputStand;
            if (stand != null) stand.CloseUserInput();

            _uiInputService.DisableAllInputs();

            StartCoroutine(MoveCamera(CameraDestination.position, CameraDestination.localRotation));
        }

        private IEnumerator MoveCamera(Vector3 destinationPosition, Quaternion destinationRotation)
        {
            while (Vector3.Distance(_currentCameraFollow.transform.position, destinationPosition) > 0.05)
            {
                Vector3 targetPosition = Vector3.Slerp(_currentCameraFollow.transform.position, destinationPosition,
                    Time.deltaTime);
                Quaternion targetRotation = Quaternion.Slerp(_currentCameraFollow.transform.localRotation,
                    destinationRotation, Time.deltaTime);
                _currentCameraFollow.transform.position = targetPosition;
                _currentCameraFollow.transform.localRotation = targetRotation;

                yield return null;
            }


            _windowService.Open(WindowType.KILLED);
        }
        
        private void SetHpBar(HpBar hpBar)
        {
            _hpBar = hpBar;
            _uiFactory.OnHpBarCreated -= SetHpBar;
        }

        private void ChangeCameraFollow(GameObject cameraFollow) => 
            _currentCameraFollow = cameraFollow;
    }
}