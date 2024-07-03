using UnityEngine;

namespace InteractionManagement.Holdable
{
    public interface IHoldable : IInteractable
    {
        void SetParent(IHoldableParent parent, Transform holdingPoint);
        Rigidbody Rigidbody { get; }
        Transform NativeTransformComponent { get; }
        void SetHoldableObjectParent(Transform nativeTransform);
        void PlayTookSound(AudioSource audioSource);
    }
}