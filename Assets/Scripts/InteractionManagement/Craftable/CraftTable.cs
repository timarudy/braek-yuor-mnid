using System.Collections.Generic;
using System.Linq;
using Configs;
using Infrastructure.AssetManagement;
using Inputs.Services;
using InteractionManagement.Attackable;
using InteractionManagement.Holdable;
using Player;
using UI.PopUps;
using UnityEngine;
using Zenject;

namespace InteractionManagement.Craftable
{
    public class CraftTable : MonoBehaviour, IHoldableParent, IInteractable
    {
        public float CraftingTime = 3f;
        public bool IsInteractable { get; set; } = true;

        public Transform HoldingPoint;
        public GameObject CraftingItem;

        private List<ICraftable> _craftableObjects = new();
        private IAssetProvider _assetProvider;
        private ISLInputService _slInputService;
        private IInputService _inputService;
        private ICraftable _craftableItem;
        private ToolTipVisibilityHandler _toolTipHandler;
        private IUIInputService _uiInputService;
        private bool _isCrafting;
        private float _craftingTimer;

        [Inject]
        private void Construct(IAssetProvider assetProvider, ISLInputService slInputService, IInputService inputService, IUIInputService uiInputService)
        {
            _uiInputService = uiInputService;
            _inputService = inputService;
            _assetProvider = assetProvider;
            _slInputService = slInputService;
        }

        private void OnEnable()
        {
            _slInputService.OnCraftStart += Craft;
            _slInputService.OnCraftCancelled += CancelCraft;
        }

        private void OnDisable()
        {
            _slInputService.OnCraftStart -= Craft;
            _slInputService.OnCraftCancelled -= CancelCraft;
        }

        private void Start()
        {
            _craftableItem = CraftingItem.GetComponent<ICraftable>();
            _toolTipHandler = GetComponent<ToolTipVisibilityHandler>();
        }

        private void Update()
        {
            if (!_isCrafting) return;

            if (_craftingTimer < CraftingTime)
            {
                _craftingTimer += Time.deltaTime;
                _toolTipHandler.ToolTip.Fill.fillAmount = _craftingTimer / CraftingTime;
            }
            else
            {
                DestroyAllCraftableObjects();
                CraftingItem.gameObject.SetActive(true);
                _slInputService.DisableCraftableTable();
                _craftableObjects.Add(_craftableItem);
                _isCrafting = false;
                _toolTipHandler.ToolTip.Fill.fillAmount = 0;
            }
        }

        public Transform GetHoldingPoint() =>
            HoldingPoint;

        public void ClearHoldableObject() =>
            _craftableObjects.RemoveAt(_craftableObjects.Count - 1);

        public void SetHoldableObject(IHoldable holdableObject) =>
            _craftableObjects.Add(holdableObject as ICraftable);

        public bool HasHoldableObject() =>
            _craftableObjects.Count != 0;

        public IHoldable GetHoldableObject() =>
            _craftableObjects[^1] as IHoldable;

        public HoldableParentType GetHoldableParentType() =>
            HoldableParentType.CRAFTABLE_TABLE;


        public void Interact(Interactor interactor)
        {
            PlayerAnimator interactorAnimator = interactor.GetComponent<PlayerAnimator>();

            if (interactor.HasHoldableObject())
            {
                if (_craftableObjects.Count < 3)
                {
                    if (interactor.GetHoldableObject() is ICraftable)
                    {
                        interactor.GetHoldableObject().SetParent(this, GetHoldingPoint());
                        interactor.ClearHoldableObject();
                        interactorAnimator.Unhold();
                        
                        _uiInputService.EnableNotepad();

                        if (GetHoldableObject() is AttackBase)
                            _inputService.DisableAttackable();

                        if (CraftEnabled())
                            _slInputService.SetCraftTable();
                    }
                }
            }
            else if (HasHoldableObject())
            {
                if (GetHoldableObject() is AttackBase)
                    _inputService.EnableAttackable();

                GetHoldableObject().SetParent(interactor, interactor.GetHoldingPoint());
                ClearHoldableObject();

                _uiInputService.DisableNotepad();
                _slInputService.DisableCraftableTable();
                interactorAnimator.Hold();
            }
        }

        public void OpenTooltip(Interactor interactor)
        {
            _toolTipHandler.ToolTip.SetKey(CraftEnabled() ? "R" : "E");

            if (interactor.HasHoldableObject())
            {
                if (interactor.GetHoldableObject() is ICraftable)
                {
                    _toolTipHandler.OpenToolTip();
                }
            }
            else if (_craftableObjects.Count > 0)
            {
                _toolTipHandler.OpenToolTip();
            }
            else
            {
                CloseTooltip(interactor);
            }
        }

        public void CloseTooltip(Interactor interactor)
        {
            _toolTipHandler.CloseToolTip();
        }

        private void DestroyAllCraftableObjects()
        {
            int craftableObjectsAmount = _craftableObjects.Count;

            foreach (ICraftable craftable in _craftableObjects)
                craftable.DestroySelf();

            for (int i = 0; i < craftableObjectsAmount; i++)
                _craftableObjects.RemoveAt(0);
        }

        private bool CraftEnabled()
        {
            List<CraftableType> craftableObjectsNames = _assetProvider
                .LoadResource<SOCraftData>(AssetPath.AxeCraftPath)
                .CraftableObjectsNames;

            List<CraftableType> holdingObjectsNames = _craftableObjects.Select(obj => obj.CraftableName).ToList();

            bool isEnabled = craftableObjectsNames.All(craftableType => holdingObjectsNames.Contains(craftableType));

            return isEnabled;
        }

        private void CancelCraft()
        {
            if (_isCrafting)
            {
                _toolTipHandler.ToolTip.Fill.fillAmount = 0;
                _isCrafting = false;
                _craftingTimer = 0;
            }
        }

        private void Craft()
        {
            _isCrafting = true;
        }
    }
}