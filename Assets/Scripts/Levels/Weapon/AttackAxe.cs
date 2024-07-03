using InteractionManagement.Attackable;
using InteractionManagement.Craftable;
using InteractionManagement.Holdable;
using UnityEngine;

namespace Levels.Weapon
{
    public class AttackAxe : AttackBase, ICraftable
    {
        public CraftableType CraftableName => CraftableType.AXE;

        public void DestroySelf() =>
            Destroy(gameObject);

        protected override void SetLocalTransform(HoldableParentType holdableParentType)
        {
            switch (holdableParentType)
            {
                case HoldableParentType.PLAYER:
                    transform.localPosition = new Vector3(0.031f, 0.092f, 0.018f);
                    transform.localRotation = Quaternion.Euler(20.721f, 186.377f, -80.758f);
                    return;
                case HoldableParentType.CRAFTABLE_TABLE:
                    transform.localPosition = new Vector3(0, 0.017f, -0.269f);
                    transform.localRotation = Quaternion.Euler(90, 0, 0);
                    return;
            }
        }

        public override void PlayTookSound(AudioSource audioSource) => 
            SoundService.PlayStickSound(audioSource);

        public override void PlayHitSound(AudioSource audioSource) =>
            SoundService.PlayChopSound(audioSource);
    }
}