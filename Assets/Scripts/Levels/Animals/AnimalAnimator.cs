using System;
using UnityEngine;

namespace Levels.Animals
{
    public class AnimalAnimator : MonoBehaviour
    {
        private static readonly int Speed = Animator.StringToHash("Speed");

        protected Animator Animator;

        private void Start()
        {
            Animator = GetComponent<Animator>();
        }

        public void Idle() =>
            Animator.SetFloat(Speed, 0f, 0.1f, Time.deltaTime);


        public void Walk() =>
            Animator.SetFloat(Speed, 1f, 0.1f, Time.deltaTime);
    }
}