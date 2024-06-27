using Player;

namespace InteractionManagement
{
    public interface IInteractable
    {
        public bool IsInteractable { get; set; }
        public void Interact(Interactor interactor);
        // void SetTooltip(Interactor interactor);
        void OpenTooltip(Interactor interactor);
        void CloseTooltip(Interactor interactor);
    }
}