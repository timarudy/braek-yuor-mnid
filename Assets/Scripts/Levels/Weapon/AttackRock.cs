using InteractionManagement.Attackable;
using InteractionManagement.Holdable;
using UnityEngine;

namespace Levels.Weapon
{
    public class AttackRock : AttackBase
    {
        protected override void SetLocalTransform(HoldableParentType holdableParentType)
        {
            transform.localPosition = new Vector3(0.003611438f, -0.052307f, 0.3187328f);
            transform.localRotation = Quaternion.Euler(-10.771f, 39.33f, -167.521f);
        }

        public override void PlayTookSound(AudioSource audioSource) => 
            SoundService.PlayStoneSound(audioSource);

        public override void PlayHitSound(AudioSource audioSource) => 
            SoundService.PlayStoneSound(audioSource);
    }
}