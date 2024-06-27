using Inputs.Services;
using Player.Input;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Player.Movement
{
    [RequireComponent(typeof(CharacterController))]
#if ENABLE_INPUT_SYSTEM
    [RequireComponent(typeof(PlayerInput))]
#endif
    public abstract class PlayerMovement : MonoBehaviour
    {
        [Header("Player")] [Tooltip("Move speed of the character in m/s")]
        public float MoveSpeed = 4.0f;

        [Tooltip("Player animator")] 
        public PlayerAnimator PlayerAnimator;

        [Tooltip("Sprint speed of the character in m/s")]
        public float SprintSpeed = 6f;

        [Tooltip("Acceleration and deceleration")]
        public float SpeedChangeRate = 10.0f;

        [Space(10)] [Tooltip("The height the player can jump")]
        public float JumpHeight = 1.2f;

        [Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
        public float Gravity = -15.0f;

        [Space(10)]
        [Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
        public float JumpTimeout = 0.50f;

        [Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
        public float FallTimeout = 0.15f;

        [Header("Player Grounded")]
        [Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
        public bool Grounded = true;

        [Tooltip("Useful for rough ground")] public float GroundedOffset = -0.14f;

        [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
        public float GroundedRadius = 0.28f;

        [Tooltip("What layers the character uses as ground")]
        public LayerMask GroundLayers;

        [Header("Cinemachine")]
        [Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
        public GameObject CinemachineCameraTarget;

        [Tooltip("How far in degrees can you move the camera up")]
        public float TopClamp = 70.0f;

        [Tooltip("How far in degrees can you move the camera down")]
        public float BottomClamp = -30.0f;

        [Tooltip("Main camera")] public GameObject MainCamera;
        
        private const float TerminalVelocity = 53.0f;
        protected const float Threshold = 0.01f;

        protected float Speed;
        protected float RotationVelocity;
        protected float VerticalVelocity;

        private float _jumpTimeoutDelta;
        private float _fallTimeoutDelta;

#if ENABLE_INPUT_SYSTEM
        protected PlayerInput PlayerInput;
#endif
        protected CharacterController Controller;
        protected StarterAssetsInputs Input;
        protected IUIInputService UIInputService;

        protected bool IsCurrentDeviceMouse
        {
            get
            {
#if ENABLE_INPUT_SYSTEM
                return PlayerInput.currentControlScheme == "KeyboardMouse";
#else
				return false;
#endif
            }
        }

        [Inject]
        private void Construct(IUIInputService uiInput)
        {
            UIInputService = uiInput;
        }

        private void Start()
        {
            OnStart();
        }

        private void Update()
        {
            OnUpdate();
        }

        private void LateUpdate()
        {
            CameraRotation();
        }

        protected virtual void OnStart()
        {
            Controller = GetComponent<CharacterController>();
            Input = GetComponent<StarterAssetsInputs>();
            PlayerInput = GetComponent<PlayerInput>();

            _jumpTimeoutDelta = JumpTimeout;
            _fallTimeoutDelta = FallTimeout;
        }

        protected virtual void OnUpdate()
        {
            JumpAndGravity();
            GroundedCheck();
            Move();
        }

        protected virtual void GroundedCheck()
        {
            // set sphere position, with offset
            Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset,
                transform.position.z);
            Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers,
                QueryTriggerInteraction.Ignore);
        }

        // private void OnDrawGizmos()
        // {
        //     Gizmos.color = Color.red;
        //     Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset,
        //         transform.position.z);
        //     Gizmos.DrawSphere(spherePosition, GroundedRadius);
        // }

        private void JumpAndGravity()
        {
            if (Grounded)
            {
                // reset the fall timeout timer
                _fallTimeoutDelta = FallTimeout;

                JumpingAnimationOff();

                // stop our velocity dropping infinitely when grounded
                if (VerticalVelocity < 0.0f)
                {
                    VerticalVelocity = -2f;
                }

                // Jump
                if (Input.jump)
                {
                    if (_jumpTimeoutDelta <= 0.0f)
                    {
                        // the square root of H * -2 * G = how much velocity needed to reach desired height
                        VerticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);

                        // update animator if using character
                        JumpingAnimationOn();
                        Input.jump = false;
                    }
                }

                // jump timeout
                if (_jumpTimeoutDelta >= 0.0f)
                {
                    _jumpTimeoutDelta -= Time.deltaTime;
                }
            }
            else
            {
                // reset the jump timeout timer
                _jumpTimeoutDelta = JumpTimeout;

                // fall timeout
                if (_fallTimeoutDelta >= 0.0f)
                {
                    _fallTimeoutDelta -= Time.deltaTime;
                }
                else
                {
                    // update animator if using character
                    FallingAnimationOn();
                }

                // if we are not grounded, do not jump
                Input.jump = false;
            }

            // apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
            if (VerticalVelocity < TerminalVelocity)
            {
                VerticalVelocity += Gravity * Time.deltaTime;
            }
        }

        private void Move()
        {
            // set target speed based on move speed, sprint speed and if sprint is pressed
            float targetSpeed = Input.sprint ? SprintSpeed : MoveSpeed;

            // a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon

            // note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
            // if there is no input, set the target speed to 0
            if (Input.move == Vector2.zero)
            {
                targetSpeed = 0.0f;
                MovingAnimationOff();
            }
            else
            {
                MovingAnimationAdjust();
            }

            // a reference to the players current horizontal velocity
            float currentHorizontalSpeed = new Vector3(Controller.velocity.x, 0.0f, Controller.velocity.z).magnitude;

            float speedOffset = 0.1f;
            float inputMagnitude = Input.analogMovement ? Input.move.magnitude : 1f;

            // accelerate or decelerate to target speed
            if (currentHorizontalSpeed < targetSpeed - speedOffset ||
                currentHorizontalSpeed > targetSpeed + speedOffset)
            {
                // creates curved result rather than a linear one giving a more organic speed change
                // note T in Lerp is clamped, so we don't need to clamp our speed
                Speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude,
                    Time.deltaTime * SpeedChangeRate);

                // round speed to 3 decimal places
                Speed = Mathf.Round(Speed * 1000f) / 1000f;
            }
            else
            {
                Speed = targetSpeed;
            }

            // normalise input direction
            Vector3 inputDirection = new Vector3(Input.move.x, 0.0f, Input.move.y).normalized;

            // note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
            // if there is a move input rotate player when the player is moving
            FindDirectionAndMove(inputDirection);
        }

        private void MovingAnimationAdjust()
        {
            if (Input.sprint)
            {
                PlayerAnimator.Run();
            }
            else
            {
                PlayerAnimator.Walk();
            }
        }

        private void MovingAnimationOff() => 
            PlayerAnimator.Idle();

        protected static float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }

        protected virtual void FallingAnimationOn()
        {
        }

        protected virtual void JumpingAnimationOn()
        {
        }

        protected virtual void JumpingAnimationOff()
        {
        }

        protected abstract void FindDirectionAndMove(Vector3 inputDirection);

        protected abstract void CameraRotation();
    }
}