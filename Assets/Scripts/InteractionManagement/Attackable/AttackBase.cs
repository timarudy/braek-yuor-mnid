using Inputs.Services;
using InteractionManagement.Holdable;
using UnityEngine;
using Zenject;

namespace InteractionManagement.Attackable
{
    public class AttackBase : HoldableBase, IInputableObject
    {
        public LayerMask AttackLayer;

        private IInputService _inputService;

        [Inject]
        private void Construct(IInputService inputService)
        {
            _inputService = inputService;
        }

        protected override void RegisterInputableHoldingObject() =>
            _inputService.EnableAttackable();

        protected override void UnregisterInputableHoldingObject() =>
            _inputService.DisableAttackable();
    }
}