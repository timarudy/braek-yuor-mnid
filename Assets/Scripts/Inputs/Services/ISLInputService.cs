using System;

namespace Inputs.Services
{
    public interface ISLInputService
    {
        void SetWateringCan();
        void SetCraftTable();
        void DisableWateringCan();
        void DisableCraftableTable();
        event Action OnCraftStart;
        event Action OnCraftCancelled;
        event Action OnWateringStarted;
        event Action OnWateringCancelled;
    }
}