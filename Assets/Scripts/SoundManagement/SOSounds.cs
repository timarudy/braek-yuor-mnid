using System.Collections.Generic;
using UnityEngine;

namespace SoundManagement
{
    [CreateAssetMenu(fileName = "Sounds", menuName = "Audio/ Sounds")]
    public class SOSounds : ScriptableObject
    {
        public List<AudioClip> StoneDoorOpenSound;
        public List<AudioClip> FailSound;
        public List<AudioClip> PickUpSound;
        public List<AudioClip> StoneSound;
        public List<AudioClip> StickSound;
        public List<AudioClip> GrassSound;
        public List<AudioClip> WateringCanSound;
        public List<AudioClip> ChopSound;
        public List<AudioClip> SwordSound;
    }
}