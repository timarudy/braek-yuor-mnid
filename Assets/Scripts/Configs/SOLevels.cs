using System.Collections.Generic;
using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "Levels", menuName = "Levels/LevelsConfig")]
    public class SOLevels : ScriptableObject
    {
        public List<LevelData> LevelsData;
    }
}