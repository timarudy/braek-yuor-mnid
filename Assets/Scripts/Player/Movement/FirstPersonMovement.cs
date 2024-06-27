// using System;
// using System.Collections;
// using System.Collections.Generic;
// using Infrastructure.AssetManagement;
// using Levels.Common;
// using Player.Input;
// using Services.Cursor;
// using UnityEngine;
// using Zenject;
//
// namespace Player.Movement
// {
//     public class FirstPersonMovement : PlayerMovement
//     {
//         private const float RotationDuration = 0.3f;
//
//         [Tooltip("Rotation speed of the character")]
//         public float RotationSpeed = 1.0f;
//
//         [Tooltip("Player body")] public List<GameObject> Body;
//
//         [Tooltip("Notepad")] public Notepad Notepad;
//
//         [Tooltip("Head")] public GameObject Head;
//
//         [Tooltip("How far in degrees can you move the camera down when attacking")]
//         public float AttackBottomClamp = -20f;
//
//         private static readonly int IsReading = Animator.StringToHash("IsReading");
//
//         private float _cinemachineTargetPitch;
//         private bool _isRotating;
//         private Quaternion _initialRotation;
//         private Quaternion _targetRotation;
//         private float _rotationStartTime;
//         private float _notepadDisappearTimer = 0.5f;
//
//         private IAssetProvider _assetProvider;
//         private CursorService _cursorService;
//         private PlayerController _playerController;
//         private bool _followHead;
//         private float _cameraToHeadDistance;
//
//         [Inject]
//         private void Construct(CursorService cursorService)
//         {
//             _cursorService = cursorService;
//         }
//
//         private void OnEnable()
//         {
//             _playerController = GetComponentInParent<PlayerController>();
//
//             UIInputService.OpenNotepadEvent += OpenNotepad;
//             UIInputService.CloseNotepadEvent += CloseNotepad;
//
//             _playerController.HideHair();
//         }
//
//         private void OnDisable()
//         {
//             UIInputService.OpenNotepadEvent -= OpenNotepad;
//             UIInputService.CloseNotepadEvent -= CloseNotepad;
//
//             _playerController.ShowHair();
//         }
//
//         protected override void OnStart()
//         {
//             base.OnStart();
//
//             _cameraToHeadDistance =
//                 Vector3.Distance(CinemachineCameraTarget.transform.position, Head.transform.position);
//         }
//
//         protected override void OnUpdate()
//         {
//             if (_isRotating)
//             {
//                 float t = (Time.time - _rotationStartTime) / RotationDuration;
//                 CinemachineCameraTarget.transform.localRotation =
//                     Quaternion.Slerp(_initialRotation, _targetRotation, t);
//
//                 // If the rotation is complete, stop rotating
//                 if (t >= 1.0f)
//                 {
//                     _isRotating = false;
//                 }
//             }
//
//             if (_followHead)
//             {
//                 Vector3 head = Head.transform.position;
//
//                 CinemachineCameraTarget.transform.position =
//                     new Vector3(head.x, head.y + _cameraToHeadDistance, head.z);
//             }
//
//             base.OnUpdate();
//         }
//
//         private void LateUpdate()
//         {
//             // if (_followHead) return;
//
//             CameraRotation();
//         }
//
//         public void Enableself()
//         {
//             ViewHandler viewHandler = GetComponent<ViewHandler>();
//             StarterAssetsInputs starterAssetsInputs = GetComponent<StarterAssetsInputs>();
//             starterAssetsInputs.ChangeViewInput(PersonView.FIRST);
//             viewHandler.EnableFirstPerson();
//             OpenNotepad();
//         }
//
//         public void InitBasicSettings()
//         {
//             float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;
//
//             _cinemachineTargetPitch += Input.look.y * RotationSpeed * deltaTimeMultiplier;
//
//             // clamp our pitch rotation
//             _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);
//
//             // Update Cinemachine camera target pitch
//             CinemachineCameraTarget.transform.localRotation = Quaternion.Euler(_cinemachineTargetPitch, 0.0f, 0.0f);
//         }
//
//         protected override void FindDirectionAndMove(Vector3 inputDirection)
//         {
//             if (Input.move != Vector2.zero)
//             {
//                 // move
//                 inputDirection = transform.right * Input.move.x + transform.forward * Input.move.y;
//             }
//
//             // move the player
//             Controller.Move(inputDirection.normalized * (Speed * Time.deltaTime) +
//                             new Vector3(0.0f, VerticalVelocity, 0.0f) * Time.deltaTime);
//         }
//
//         protected override void CameraRotation()
//         {
//             // if there is an input
//             if (Input.look.sqrMagnitude >= Threshold)
//             {
//                 //Don't multiply mouse input by Time.deltaTime
//                 float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;
//
//                 _cinemachineTargetPitch += Input.look.y * RotationSpeed * deltaTimeMultiplier;
//                 RotationVelocity = Input.look.x * RotationSpeed * deltaTimeMultiplier;
//
//                 // clamp our pitch rotation
//                 _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);
//
//                 // Update Cinemachine camera target pitch
//                 CinemachineCameraTarget.transform.localRotation = Quaternion.Euler(_cinemachineTargetPitch, 0.0f, 0.0f);
//
//                 // rotate the player left and right
//                 transform.Rotate(Vector3.up * RotationVelocity);
//             }
//         }
//
//         private void OpenNotepad()
//         {
//             PlayerInput.DeactivateInput();
//
//             _notepadDisappearTimer = 0.5f;
//             _cursorService.UnlockCursor();
//             Notepad.Open();
//             PlayerAnimator.Read(true);
//
//             _targetRotation = Quaternion.Euler(30 * RotationSpeed, 0.0f, 0.0f);
//             _initialRotation = CinemachineCameraTarget.transform.localRotation;
//             _cinemachineTargetPitch = 30 * RotationSpeed;
//             _rotationStartTime = Time.time;
//             _isRotating = true;
//         }
//
//         private void CloseNotepad()
//         {
//             _cursorService.LockCursor();
//             PlayerAnimator.Read(false);
//             PlayerInput.ActivateInput();
//             StartCoroutine(DisableNotepad());
//         }
//
//         private IEnumerator DisableNotepad()
//         {
//             while (_notepadDisappearTimer > 0)
//             {
//                 _notepadDisappearTimer -= Time.deltaTime;
//                 yield return null;
//             }
//
//             if (!PlayerAnimator.GetBool(IsReading))
//                 Notepad.Close();
//
//             _notepadDisappearTimer = 0.5f;
//         }
//
//         // private void HideBody()
//         // {
//         //     foreach (GameObject part in Body)
//         //         part.SetActive(false);
//         // }
//         //
//         // private void ShowBody()
//         // {
//         //     foreach (GameObject part in Body)
//         //         part.SetActive(true);
//         // }
//
//         public void FollowHead() =>
//             _followHead = true;
//
//         public void UnfollowHead() =>
//             _followHead = false;
//     }
// }

using System;
using System.Collections;
using System.Collections.Generic;
using Infrastructure.AssetManagement;
using Levels.Common;
using Player.Input;
using Services.Cursor;
using UnityEngine;
using Zenject;

namespace Player.Movement
{
    public class FirstPersonMovement : PlayerMovement
    {
        private const float RotationDuration = 0.3f;

        [Tooltip("Rotation speed of the character")]
        public float RotationSpeed = 1.0f;

        [Tooltip("Player body")] public List<GameObject> Body;

        [Tooltip("Notepad")] public Notepad Notepad;

        [Tooltip("Head")] public GameObject Head;

        [Tooltip("How far in degrees can you move the camera down when attacking")]
        public float AttackBottomClamp = -20f;

        private static readonly int IsReading = Animator.StringToHash("IsReading");
        private static readonly int IsAttacking = Animator.StringToHash("IsAttacking");

        private float _cinemachineTargetPitch;
        private bool _isRotating;
        private Quaternion _initialRotation;
        private Quaternion _targetRotation;
        private float _rotationStartTime;
        private float _notepadDisappearTimer = 0.5f;
        private bool _isAttacking;

        private IAssetProvider _assetProvider;
        private CursorService _cursorService;
        private PlayerController _playerController;
        private bool _followHead;
        private float _cameraToHeadDistance;
        private Vector3 _startCameraPosition;

        [Inject]
        private void Construct(CursorService cursorService)
        {
            _cursorService = cursorService;
        }

        private void OnEnable()
        {
            _playerController = GetComponentInParent<PlayerController>();

            UIInputService.OpenNotepadEvent += OpenNotepad;
            UIInputService.CloseNotepadEvent += CloseNotepad;

            _playerController.HideHair();
        }

        private void OnDisable()
        {
            UIInputService.OpenNotepadEvent -= OpenNotepad;
            UIInputService.CloseNotepadEvent -= CloseNotepad;

            _playerController.ShowHair();
        }

        protected override void OnStart()
        {
            base.OnStart();

            _startCameraPosition = CinemachineCameraTarget.transform.localPosition;
            
            _cameraToHeadDistance =
                Vector3.Distance(CinemachineCameraTarget.transform.position, Head.transform.position);
        }

        protected override void OnUpdate()
        {
            if (_isRotating)
            {
                float t = (Time.time - _rotationStartTime) / RotationDuration;
                CinemachineCameraTarget.transform.localRotation =
                    Quaternion.Slerp(_initialRotation, _targetRotation, t);

                // If the rotation is complete, stop rotating
                if (t >= 1.0f)
                {
                    _isRotating = false;
                }
            }

            if (_followHead)
            {
                Vector3 head = Head.transform.position;

                CinemachineCameraTarget.transform.position =
                    new Vector3(head.x, head.y + _cameraToHeadDistance, head.z);
            }

            // Follow head when attacking
            if (_isAttacking)
            {
                Vector3 head = Head.transform.position;
                CinemachineCameraTarget.transform.position = Vector3.Lerp(CinemachineCameraTarget.transform.position,
                    new Vector3(head.x, head.y + _cameraToHeadDistance, head.z), Time.deltaTime * RotationSpeed);
            }

            base.OnUpdate();
        }

        private void LateUpdate()
        {
            // if (_followHead) return;

            CameraRotation();
        }

        public void Enableself()
        {
            ViewHandler viewHandler = GetComponent<ViewHandler>();
            StarterAssetsInputs starterAssetsInputs = GetComponent<StarterAssetsInputs>();
            starterAssetsInputs.ChangeViewInput(PersonView.FIRST);
            viewHandler.EnableFirstPerson();
            OpenNotepad();
        }

        public void InitBasicSettings()
        {
            float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;

            _cinemachineTargetPitch += Input.look.y * RotationSpeed * deltaTimeMultiplier;

            // clamp our pitch rotation
            _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

            // Update Cinemachine camera target pitch
            CinemachineCameraTarget.transform.localRotation = Quaternion.Euler(_cinemachineTargetPitch, 0.0f, 0.0f);
        }

        protected override void FindDirectionAndMove(Vector3 inputDirection)
        {
            if (Input.move != Vector2.zero)
            {
                // move
                inputDirection = transform.right * Input.move.x + transform.forward * Input.move.y;
            }

            // move the player
            Controller.Move(inputDirection.normalized * (Speed * Time.deltaTime) +
                            new Vector3(0.0f, VerticalVelocity, 0.0f) * Time.deltaTime);
        }

        protected override void CameraRotation()
        {
            // if there is an input
            if (Input.look.sqrMagnitude >= Threshold)
            {
                //Don't multiply mouse input by Time.deltaTime
                float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;

                _cinemachineTargetPitch += Input.look.y * RotationSpeed * deltaTimeMultiplier;
                RotationVelocity = Input.look.x * RotationSpeed * deltaTimeMultiplier;

                // clamp our pitch rotation
                _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

                // Update Cinemachine camera target pitch
                CinemachineCameraTarget.transform.localRotation = Quaternion.Euler(_cinemachineTargetPitch, 0.0f, 0.0f);

                // rotate the player left and right
                transform.Rotate(Vector3.up * RotationVelocity);
            }
        }

        private void OpenNotepad()
        {
            PlayerInput.DeactivateInput();

            _notepadDisappearTimer = 0.5f;
            _cursorService.UnlockCursor();
            Notepad.Open();
            PlayerAnimator.Read(true);

            _targetRotation = Quaternion.Euler(30 * RotationSpeed, 0.0f, 0.0f);
            _initialRotation = CinemachineCameraTarget.transform.localRotation;
            _cinemachineTargetPitch = 30 * RotationSpeed;
            _rotationStartTime = Time.time;
            _isRotating = true;
        }

        private void CloseNotepad()
        {
            _cursorService.LockCursor();
            PlayerAnimator.Read(false);
            PlayerInput.ActivateInput();
            StartCoroutine(DisableNotepad());
        }

        private IEnumerator DisableNotepad()
        {
            while (_notepadDisappearTimer > 0)
            {
                _notepadDisappearTimer -= Time.deltaTime;
                yield return null;
            }

            if (!PlayerAnimator.GetBool(IsReading))
                Notepad.Close();

            _notepadDisappearTimer = 0.5f;
        }

        public void FollowHead() =>
            _followHead = true;

        public void UnfollowHead() =>
            _followHead = false;

        public void StartAttacking() => 
            _isAttacking = true;

        public void StopAttacking()
        {
            _isAttacking = false;
            StartCoroutine(SmoothReturnToStart());
        }

        private IEnumerator SmoothReturnToStart()
        {
            Vector3 startPosition = CinemachineCameraTarget.transform.localPosition;
            float elapsedTime = 0;

            while (elapsedTime < RotationDuration)
            {
                CinemachineCameraTarget.transform.localPosition =
                    Vector3.Lerp(startPosition, _startCameraPosition, elapsedTime / RotationDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            CinemachineCameraTarget.transform.localPosition = _startCameraPosition;
        }
    }
}