using System;

namespace Inputs.Services
{
    public interface IUIInputService
    {
        event Action OpenNotepadEvent;
        event Action CloseNotepadEvent;
        event Action EnableInteractableInput;
        event Action DisableInteractableInput;
        event Action OnWindowOpen;
        event Action OnWindowClose;
        bool IsWindowOpened { get; set; }
        bool IsNotepadOpened { get; set; }
        void EnableWorldInput();
        void EnableNotepad();
        void EnableWindows();
        void DisableAllInputs();
        void DisableWorldInput();
        void DisableNotepad();
        void CloseSettings();
        void OpenSettings();
    }
}