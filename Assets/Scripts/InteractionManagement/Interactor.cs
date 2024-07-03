using Inputs.Services;
using InteractionManagement.Attackable;
using InteractionManagement.Craftable;
using InteractionManagement.Holdable;
using Levels.Common;
using UnityEngine;
using Zenject;
using Monitor = Levels.Common.Monitor;

namespace InteractionManagement
{
    public class Interactor : MonoBehaviour, IHoldableParent
    {
        public Transform CapsuleTopPoint;
        public Transform CapsuleBottomPoint;
        public Transform HoldingPoint;
        public float InteractionRadius = 0.01f;
        public LayerMask InteractableMask;
        public HoldableParentType InteractorHoldableParentType;

        private Collider[] _interactableColliders = new Collider[3];
        private IUIInputService _uiInput;
        private IInputService _inputService;
        private IHoldable _holdableObject;
        private IInteractable _currentInteractableObject;
        private int _collidersFound;

        [Inject]
        private void Construct(IUIInputService uiInput, IInputService inputService)
        {
            _uiInput = uiInput;
            _inputService = inputService;
        }

        private void OnEnable()
        {
            _uiInput.EnableInteractableInput += TryInteraction;
            _uiInput.OnSettingsClose += TryEnableNotepad;
            _uiInput.OnSettingsClose += TryEnableAttackable;
        }

        private void OnDisable()
        {
            _uiInput.EnableInteractableInput -= TryInteraction;
            _uiInput.OnSettingsClose -= TryEnableNotepad;
            _uiInput.OnSettingsClose -= TryEnableAttackable;
        }

        private void Update()
        {
            _collidersFound = Physics.OverlapCapsuleNonAlloc(CapsuleTopPoint.position, CapsuleBottomPoint.position,
                InteractionRadius, _interactableColliders, InteractableMask);

            _interactableColliders = FilterColliders();

            SetTooltip();
        }


        // private void OnDrawGizmos()
        // {
        //     Gizmos.color = Color.red;
        //     Gizmos.DrawSphere(CapsuleBottomPoint.position, InteractionRadius);
        //     Gizmos.DrawSphere(CapsuleTopPoint.position, InteractionRadius);
        // }

        public IInteractable GetCurrentInteractableObject() =>
            _currentInteractableObject;

        public Transform GetHoldingPoint() =>
            HoldingPoint;

        public void ClearHoldableObject() =>
            _holdableObject = null;

        public void SetHoldableObject(IHoldable holdableObject) =>
            _holdableObject = holdableObject;

        public bool HasHoldableObject() =>
            _holdableObject != null;

        public IHoldable GetHoldableObject() =>
            _holdableObject;

        public HoldableParentType GetHoldableParentType() =>
            InteractorHoldableParentType;

        public T GetInteractableByType<T>() where T : class, IInteractable
        {
            foreach (Collider col in _interactableColliders)
            {
                if (col == null) continue;

                if (col.TryGetComponent(out T component))
                    return component;
            }

            return null;
        }

        private void TryEnableNotepad()
        {
            if (!HasHoldableObject())
            {
                _uiInput.EnableNotepad();
            }
        }
        
        private void TryEnableAttackable()
        {
            if (HasHoldableObject())
            {
                if (GetHoldableObject() is AttackBase)
                {
                    _inputService.EnableAttackable();
                }
            }
        }

        private void SetTooltip()
        {
            if (InteractablesFound(0))
            {
                Collider col = _interactableColliders[0];

                if (InteractablesFound(1))
                {
                    if (!HasHoldableObject())
                    {
                        if (col.TryGetComponent(out CraftTable craftTable))
                        {
                            if (_interactableColliders[1].transform.parent == _interactableColliders[1]
                                    .GetComponent<IHoldable>().NativeTransformComponent)
                            {
                                col = _interactableColliders[1];
                            }
                        }
                    }
                }

                if (col.GetComponent<IInteractable>() != _currentInteractableObject)
                {
                    _currentInteractableObject?.CloseTooltip(this);
                }

                _currentInteractableObject = col.GetComponent<IInteractable>();
                _currentInteractableObject.OpenTooltip(this);
            }
            else
            {
                _currentInteractableObject?.CloseTooltip(this);
            }
        }

        private void TryInteraction()
        {
            _collidersFound = Physics.OverlapCapsuleNonAlloc(CapsuleTopPoint.position, CapsuleBottomPoint.position,
                InteractionRadius, _interactableColliders, InteractableMask);

            _interactableColliders = FilterColliders();

            if (!InteractablesFound(0)) return;

            if (InteractablesFound(1))
            {
                foreach (Collider col in _interactableColliders)
                {
                    if (col != null)
                    {
                        if (col.TryGetComponent(out IWorldCanvasInput worldInput))
                        {
                            // monitor.GetComponent<IInteractable>().Interact(this);
                            IInteractable interactable = worldInput as IInteractable;
                            interactable?.Interact(this);
                            return;
                        }
                    }
                }

                for (int index = 0; index < _interactableColliders.Length; index++)
                {
                    Collider col = _interactableColliders[index];
                    if (col != null)
                    {
                        if (col.TryGetComponent(out CraftTable _))
                        {
                            col = _interactableColliders[index + 1];
                            if (col != null)
                            {
                                if (col.transform.parent == col.GetComponent<IHoldable>().NativeTransformComponent)
                                {
                                    Interact(col.GetComponent<IInteractable>());
                                    return;
                                }
                            }
                        }
                        else if (col.transform.parent == col.GetComponent<IHoldable>().NativeTransformComponent)
                        {
                            Interact(col.GetComponent<IInteractable>());
                            return;
                        }
                    }
                }
            }

            Interact(0);
        }

        private bool InteractablesFound(int moreThanCollidersAmount) =>
            _collidersFound > moreThanCollidersAmount;

        private Collider[] FilterColliders()
        {
            Collider[] filteredColliders = new Collider[3];

            int interactables = 0;
            int index = 0;
            int nullable = 0;
            while (index < 3)
            {
                Collider col = _interactableColliders[index];
                if (col != null)
                {
                    if (col.GetComponent<IInteractable>().IsInteractable)
                    {
                        filteredColliders[interactables] = col;
                        interactables++;
                    }
                    else
                    {
                        nullable++;
                        _collidersFound--;
                    }
                }
                else
                {
                    nullable++;
                }

                index++;

                if (nullable + interactables == 3)
                    return filteredColliders;
            }

            return filteredColliders;
        }

        private void Interact(int colliderNumber)
        {
            IInteractable interactable = _interactableColliders[colliderNumber].GetComponent<IInteractable>();
            interactable?.Interact(this);
        }

        private void Interact(IInteractable interactable) =>
            interactable?.Interact(this);
    }
}