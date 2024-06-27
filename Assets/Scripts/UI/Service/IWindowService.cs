using System;
using UI.Factory;
using UI.Windows;

namespace UI.Service
{
    public interface IWindowService
    {
        event Action CloseCurrentWindowEvent;
        WindowBase CurrentWindow { get; set; }
        void Open(WindowType windowType);
        void CloseCurrentWindow();
        void ReopenWindow(WindowType windowType);
    }
}