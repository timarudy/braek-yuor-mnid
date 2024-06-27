using System.Collections.Generic;
using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "GuessImagesData", menuName = "Input Stand/Guess Images Data")]
    public class SOGuessImages : ScriptableObject
    {
        public List<GuessImageData> GuessImages;
    }
}