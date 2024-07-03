using Inputs.Services;
using Player;
using SoundManagement;
using UI.Factory;
using UI.PopUps;
using UnityEngine;
using Zenject;

namespace InteractionManagement.Holdable
{
    public abstract class HoldableBase : MonoBehaviour, IHoldable
    {
        [SerializeField] private Transform HoldableObjectsParentTransform;

        public bool IsInteractable { get; set; } = true;
        public Rigidbody Rigidbody { get; private set; }
        public Transform NativeTransformComponent { get; private set; }
        
        protected ISoundService SoundService;
        
        private IHoldableParent _holdableParent;
        private ToolTipVisibilityHandler _toolTipHandler;
        private IUIInputService _uiInputService;

        [Inject]
        private void Construct(IUIInputService uiInputService, ISoundService soundService)
        {
            SoundService = soundService;
            _uiInputService = uiInputService;
        }

        private void Start() => 
            OnStart();

        protected virtual void OnStart()
        {
            Rigidbody = GetComponent<Rigidbody>();
            NativeTransformComponent = HoldableObjectsParentTransform;
            _toolTipHandler = GetComponent<ToolTipVisibilityHandler>();
        }

        public void SetParent(IHoldableParent holdableParent, Transform holdingPoint)
        {
            gameObject.SetActive(false);
            _holdableParent = holdableParent;
            _holdableParent.SetHoldableObject(this);

            transform.parent = holdingPoint;
            SetLocalTransform(holdableParent.GetHoldableParentType());
            gameObject.SetActive(true);
        }


        public virtual void Interact(Interactor interactor)
        {
            PlayerAnimator interactorAnimator = interactor.GetComponent<PlayerAnimator>();

            if (!interactor.HasHoldableObject())
            {
                if (this is IInputableObject)
                    RegisterInputableHoldingObject();

                PlayTookSound(interactor.GetComponent<PlayerHealth>().AudioSource);
                
                Rigidbody.isKinematic = true;
                interactorAnimator.Hold();
                interactor.SetHoldableObject(this);
                SetParent(interactor, interactor.HoldingPoint);
                SetLocalTransform(interactor.GetHoldableParentType());
                
                _uiInputService.DisableNotepad();
            }
            else
            {
                if (interactor.GetHoldableObject() is IInputableObject)
                {
                    HoldableBase holdableObj = interactor.GetHoldableObject() as HoldableBase;
                    
                    if (holdableObj != null) 
                        holdableObj.UnregisterInputableHoldingObject();
                }

                interactor.GetHoldableObject().PlayTookSound(interactor.GetComponent<PlayerHealth>().AudioSource);
                
                interactor.GetHoldableObject().Rigidbody.isKinematic = false;
                interactorAnimator.Unhold();
                interactor.GetHoldableObject().SetHoldableObjectParent(NativeTransformComponent);
                interactor.ClearHoldableObject();
                
                _uiInputService.EnableNotepad();
            }
        }


        public virtual void OpenTooltip(Interactor interactor)
        {
            if (interactor.GetHoldableObject() == null)
            {
                _toolTipHandler.OpenToolTip();
            }
            else
            {
                CloseTooltip(interactor);
            }
        }

        public virtual void CloseTooltip(Interactor interactor) => 
            _toolTipHandler.CloseToolTip();

        public void SetHoldableObjectParent(Transform nativeTransform) =>
            transform.parent = nativeTransform;

        public void SetNativeTransform(Transform nativeTransform) =>
            NativeTransformComponent = nativeTransform;

        protected virtual void SetLocalTransform(HoldableParentType holdableParentType) =>
            transform.localPosition = Vector3.zero;

        protected virtual void RegisterInputableHoldingObject()
        {
        }

        protected virtual void UnregisterInputableHoldingObject()
        {
        }

        public abstract void PlayTookSound(AudioSource audioSource);
    }
}
