using System;
using UI.Factory;
using UI.Windows;

namespace Services.StaticData
{
    [Serializable]
    public struct WindowConfig
    {
        public WindowType WindowType;
        public WindowBase Prefab;
    }
}