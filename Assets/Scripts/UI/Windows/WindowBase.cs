using Inputs;
using Inputs.Services;
using UI.Service;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Windows
{
    public abstract class WindowBase : MonoBehaviour
    {
        public Button CloseButton;

        protected IWindowService WindowService;
        protected IUIInputService UIInput;
        
        [Inject]
        private void Construct(IWindowService windowService, IUIInputService uiInput)
        {
            UIInput = uiInput;
            WindowService = windowService;
        }

        private void Awake() => 
            OnAwake();

        private void OnEnable() => 
            OnEnableAction();

        private void OnDisable() => 
            OnDisableAction();

        protected virtual void OnAwake() => 
            CloseButton.onClick.AddListener(CloseSelf);

        protected virtual void CloseSelf()
        {
            WindowService.CloseCurrentWindow();
            UIInput.IsWindowOpened = false;
        }

        private void DestroySelf() =>
            Destroy(gameObject);

        protected virtual void OnEnableAction()
        {
            WindowService.CloseCurrentWindowEvent += DestroySelf;
        }

        protected virtual void OnDisableAction()
        {
            WindowService.CloseCurrentWindowEvent -= DestroySelf;
        }
    }
}