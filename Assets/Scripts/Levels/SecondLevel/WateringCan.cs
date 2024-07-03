using Inputs.Services;
using InteractionManagement;
using InteractionManagement.Holdable;
using Player;
using UnityEngine;
using Zenject;

namespace Levels.SecondLevel
{
    public class WateringCan : HoldableBase, IInputableObject
    {
        public CraftableGrass CraftableGrass;

        private ISLInputService _slInputService;
        private PlayerController _player;

        [Inject]
        private void Construct(ISLInputService slInputService, PlayerController player)
        {
            _player = player;
            _slInputService = slInputService;
        }

        private void OnEnable()
        {
            _slInputService.OnWateringStarted += StartWateringGrass;
            _slInputService.OnWateringCancelled += StopWateringGrass;
        }

        private void OnDisable()
        {
            _slInputService.OnWateringStarted -= StartWateringGrass;
            _slInputService.OnWateringCancelled -= StopWateringGrass;
        }

        public override void Interact(Interactor interactor)
        {
            base.Interact(interactor);

            if (!CraftableGrass.IsGrown)
            {
                if (!interactor.HasHoldableObject())
                {
                    CraftableGrass.IsInteractable = false;
                }
                else
                {
                    CraftableGrass.IsInteractable = true;
                }
            }
        }

        protected override void SetLocalTransform(HoldableParentType holdableParentType)
        {
            transform.localPosition = new Vector3(0.3823f, 0.0738f, -0.1173f);
            transform.localRotation = Quaternion.Euler(-99.776f, -42.125f, 153.268f);
        }

        protected override void RegisterInputableHoldingObject() =>
            _slInputService.SetWateringCan();

        protected override void UnregisterInputableHoldingObject() =>
            _slInputService.DisableWateringCan();

        public override void PlayTookSound(AudioSource audioSource) => 
            SoundService.PlayWateringCanSound(audioSource);

        private void StartWateringGrass() =>
            WaterGrass(true);

        private void StopWateringGrass() =>
            WaterGrass(false);

        private void WaterGrass(bool water)
        {
            CraftableGrass craftableGrass = _player.GetComponent<Interactor>().GetInteractableByType<CraftableGrass>();
            PlayerAnimator animator = _player.GetComponent<PlayerAnimator>();

            if (craftableGrass != null)
            {
                animator.Water(water);
                if (!craftableGrass.IsTimerSet)
                {
                    craftableGrass.StartGrowing();
                }
                else
                {
                    craftableGrass.StopGrowing();
                }
            }
        }
    }
}