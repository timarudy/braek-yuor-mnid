using InteractionManagement.Attackable;
using InteractionManagement.Holdable;
using UnityEngine;

namespace Levels.Weapon
{
    public class Sword : AttackBase
    {
        protected override void SetLocalTransform(HoldableParentType holdableParentType)
        {
            transform.localPosition = new Vector3(0.0505000018f,0.0839999989f,-0.0155999996f);
            transform.localRotation = Quaternion.Euler(0.909190416f,7.72223091f,79.1233292f);
        }
    }
}