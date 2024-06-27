using InteractionManagement;
using InteractionManagement.Craftable;
using InteractionManagement.Holdable;
using UnityEngine;

namespace Levels.SecondLevel
{
    public class CraftableGrass : HoldableBase, ICraftable
    {
        private const float WateringTime = 3f;

        public CraftableType CraftableName => CraftableType.GRASS;

        public bool IsTimerSet { get; private set; }
        public bool IsGrown { get; private set; }

        private readonly Vector3 _targetScale = new(1.7f, 1.7f, 1.7f);
        private float _wateringTimer = WateringTime;
        private Vector3 _initialScale;

        protected override void OnStart()
        {
            base.OnStart();
            IsInteractable = false;
            _initialScale = transform.localScale;
        }

        private void Update()
        {
            if (!IsTimerSet) return;

            if (_wateringTimer > 0)
            {
                _wateringTimer -= Time.deltaTime;
                float progress = 1 - _wateringTimer / WateringTime;

                Vector3 newScale = Vector3.Lerp(_initialScale, _targetScale, progress);

                transform.localScale = newScale;
            }
            else
            {
                GrowGrass();
            }
        }

        public void DestroySelf() =>
            Destroy(gameObject);

        public override void Interact(Interactor interactor)
        {
            if (IsGrown)
            {
                base.Interact(interactor);
            }
        }

        public override void OpenTooltip(Interactor interactor)
        {
            if (IsGrown)
            {
                base.OpenTooltip(interactor);
            }
        }

        protected override void SetLocalTransform(HoldableParentType holdableParentType)
        {
            switch (holdableParentType)
            {
                case HoldableParentType.PLAYER:
                    transform.localPosition = new Vector3(0.134f, 0.0771f, -0.006f);
                    transform.localRotation = Quaternion.Euler(-55.04f, 185.606f, -80.355f);
                    return;
                case HoldableParentType.CRAFTABLE_TABLE:
                    transform.localPosition = new Vector3(0.087f, 0.012f, 0.064f);
                    transform.localRotation = Quaternion.Euler(32.6f, 113.4f, 62.062f);
                    return;
            }
        }

        public void StartGrowing()
        {
            IsTimerSet = true;
        }

        public void StopGrowing()
        {
            IsTimerSet = false;
        }

        private void GrowGrass()
        {
            IsGrown = true;
            IsInteractable = true;
        }
    }
}