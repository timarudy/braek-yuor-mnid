using InteractionManagement.Holdable;
using UnityEngine;

namespace InteractionManagement.Craftable
{
    public class CraftableRock : HoldableBase, ICraftable
    {
        public CraftableType CraftableName => CraftableType.ROCK;

        public void DestroySelf() => 
            Destroy(gameObject);

        protected override void SetLocalTransform(HoldableParentType holdableParentType)
        {
            switch (holdableParentType)
            {
                case HoldableParentType.PLAYER:
                    transform.localPosition = new Vector3(0.004f,-0.088f,0.218f);
                    transform.localRotation = Quaternion.Euler(0, 0, 0);
                    return;
                case HoldableParentType.CRAFTABLE_TABLE:
                    transform.localPosition = new Vector3(-0.16f,0.108f,0.277f);
                    transform.localRotation = Quaternion.Euler(-270, 0, -12.629f);
                    return;
            }
        }
    }
}