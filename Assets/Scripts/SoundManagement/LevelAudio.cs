using System;
using Services.SceneServices;
using UnityEngine;

namespace SoundManagement
{
    [Serializable]
    public class LevelAudio
    {
        public GameLevels Level;
        public AudioClip AudioClip;
    }
}