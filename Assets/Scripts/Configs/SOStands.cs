using System.Collections.Generic;
using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "InputStandsData", menuName = "Input Stand/Input Stands Data")]
    public class SOStands : ScriptableObject
    {
        public List<InputStandData> InputStandsData;
    }
}