using UnityEngine;

namespace Player.Movement
{
    public class ThirdPersonMovement : PlayerMovement
    {
        [Tooltip("How fast the character turns to face movement direction")] [Range(0.0f, 0.3f)]
        public float RotationSmoothTime = 0.12f;

        [Tooltip("Additional degress to override the camera. Useful for fine tuning camera position when locked")]
        public float CameraAngleOverride;

        [Tooltip("For locking the camera position on all axis")]
        public bool LockCameraPosition;

        private float _cinemachineTargetYaw;
        private float _cinemachineTargetPitch;
        private float _targetRotation;

        private void OnEnable()
        {
            UIInputService.OpenNotepadEvent += ToFirstPersonView;
        }

        private void OnDisable()
        {
            UIInputService.OpenNotepadEvent -= ToFirstPersonView;
        }

        protected override void CameraRotation()
        {
            // if there is an input and camera position is not fixed
            if (Input.look.sqrMagnitude >= Threshold && !LockCameraPosition)
            {
                //Don't multiply mouse input by Time.deltaTime;
                float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;

                _cinemachineTargetYaw += Input.look.x * deltaTimeMultiplier;
                _cinemachineTargetPitch += Input.look.y * deltaTimeMultiplier;
            }

            // clamp our rotations so our values are limited 360 degrees
            _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
            _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

            // Cinemachine will follow this target
            CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride,
                _cinemachineTargetYaw, 0.0f);
        }

        protected override void FindDirectionAndMove(Vector3 inputDirection)
        {
            if (Input.move != Vector2.zero)
            {
                _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                                  MainCamera.transform.eulerAngles.y;
                float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref RotationVelocity,
                    RotationSmoothTime);

                // rotate to face input direction relative to camera position
                transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
            }


            Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

            // move the player
            Controller.Move(targetDirection.normalized * (Speed * Time.deltaTime) +
                             new Vector3(0.0f, VerticalVelocity, 0.0f) * Time.deltaTime);
        }

        protected override void GroundedCheck()
        {
            base.GroundedCheck();
            PlayerAnimator.Ground(true);
        }

        protected override void FallingAnimationOn() => 
            PlayerAnimator.Fall(true);

        protected override void JumpingAnimationOn() => 
            PlayerAnimator.Jump();

        protected override void JumpingAnimationOff()
        {
            PlayerAnimator.Ground(false);
            PlayerAnimator.Fall(false);
        }

        private void ToFirstPersonView()
        {
            FirstPersonMovement firstPerson = GetComponent<FirstPersonMovement>();
            firstPerson.Enableself();
        }
    }
}