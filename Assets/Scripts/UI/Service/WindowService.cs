using System;
using Services.Cursor;
using UI.Factory;
using UI.Windows;

namespace UI.Service
{
    public class WindowService : IWindowService
    {
        public event Action CloseCurrentWindowEvent;

        public WindowBase CurrentWindow { get; set; }

        private readonly IUIFactory _uiFactory;
        private readonly CursorService _cursorService;

        public WindowService(IUIFactory uiFactory, CursorService cursorService)
        {
            _uiFactory = uiFactory;
            _cursorService = cursorService;
        }

        public void Open(WindowType windowType)
        {
            CurrentWindow = _uiFactory.CreateWindow(windowType);

            _cursorService.UnlockCursor();
        }

        public void CloseCurrentWindow()
        {
            if (CurrentWindow != null)
            {
                CloseCurrentWindowEvent?.Invoke();
                if (CurrentWindow is SettingsWindow or AccessoriesWindow or GameSettingsWindow)
                    _cursorService.LockCursor();

                CurrentWindow = null;
            }
        }

        public void ReopenWindow(WindowType windowType)
        {
            CloseCurrentWindow();
            Open(windowType);
        }
    }
}