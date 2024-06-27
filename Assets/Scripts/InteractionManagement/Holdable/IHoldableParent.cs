using UnityEngine;

namespace InteractionManagement.Holdable
{
    public interface IHoldableParent
    {
        Transform GetHoldingPoint();
        void ClearHoldableObject();
        void SetHoldableObject(IHoldable holdableObject);
        bool HasHoldableObject();
        IHoldable GetHoldableObject();
        HoldableParentType GetHoldableParentType();
    }
}