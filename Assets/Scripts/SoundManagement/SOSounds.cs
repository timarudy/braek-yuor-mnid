using System.Collections.Generic;
using UnityEngine;

namespace SoundManagement
{
    [CreateAssetMenu(fileName = "Sounds", menuName = "Audio/ Sounds")]
    public class SOSounds : ScriptableObject
    {
        public List<AudioClip> StoneDoorOpenSound;
        public List<AudioClip> FailSound;
    }
}