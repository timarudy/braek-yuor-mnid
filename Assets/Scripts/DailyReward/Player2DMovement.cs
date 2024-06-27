using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DailyReward
{
    public class Player2DMovement : MonoBehaviour
    {
        private static readonly int Horizontal = Animator.StringToHash("Horizontal");
        private static readonly int Vertical = Animator.StringToHash("Vertical");
        private static readonly int Speed = Animator.StringToHash("Speed");

        public float MovementSpeed;
        public LayerMask LayerMask;

        private Rigidbody2D _rigidbody;
        private Animator _playerAnimator;

        private Vector2 _movementDirection;

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _playerAnimator = GetComponent<Animator>();
        }

        private void Update()
        {
            _movementDirection.x = Input.GetAxisRaw("Horizontal");
            _movementDirection.y = Input.GetAxisRaw("Vertical");

            if (_movementDirection.x != 0)
                _movementDirection.y = 0;

            Vector2 targetPos = _rigidbody.position + _movementDirection * (MovementSpeed * Time.fixedDeltaTime);

            if (IsWalkable(targetPos))
            {
                _playerAnimator.SetFloat(Horizontal, _movementDirection.x);
                _playerAnimator.SetFloat(Vertical, _movementDirection.y);
                _playerAnimator.SetFloat(Speed, _movementDirection.magnitude);
            }
        }

        private void FixedUpdate()
        {
            Vector2 targetPos = _rigidbody.position + _movementDirection * (MovementSpeed * Time.fixedDeltaTime);

            if (IsWalkable(targetPos))
                _rigidbody.MovePosition(targetPos);
        }

        private bool IsWalkable(Vector2 targetPos)
        {
            if (Physics2D.OverlapCircle(targetPos, 0.2f, LayerMask) != null)
            {
                return false;
            }

            return true;
        }
    }
}