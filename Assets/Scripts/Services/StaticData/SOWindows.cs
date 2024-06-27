using System.Collections.Generic;
using UnityEngine;

namespace Services.StaticData
{
    [CreateAssetMenu(fileName = "WindowStaticData", menuName = "Static Data/Window Static Data")]
    public class SOWindows : ScriptableObject
    {
        public List<WindowConfig> WindowConfigs;
    }
}