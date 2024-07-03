using InteractionManagement.Holdable;
using UnityEngine;

namespace InteractionManagement.Craftable
{
    public class CraftableBranch : HoldableBase, ICraftable
    {
        public CraftableType CraftableName => CraftableType.BRANCH;

        public void DestroySelf() => 
            Destroy(gameObject);
        
        protected override void SetLocalTransform(HoldableParentType holdableParentType)
        {
            switch (holdableParentType)
            {
                case HoldableParentType.PLAYER:
                    transform.localPosition = new Vector3(0.3868f, 0.0065f, -0.0425f);
                    transform.localRotation = Quaternion.Euler(-20.5f, -76, 0);
                    return;
                case HoldableParentType.CRAFTABLE_TABLE:
                    transform.localPosition = new Vector3(0.163f, 0.075f, -0.529f);
                    transform.localRotation = Quaternion.Euler(0, 0, 0);
                    return;
            }
        }

        public override void PlayTookSound(AudioSource audioSource) => 
            SoundService.PlayStickSound(audioSource);
    }
}