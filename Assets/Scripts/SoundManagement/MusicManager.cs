using System;
using System.Collections.Generic;
using Services.SceneServices;
using UnityEngine;

namespace SoundManagement
{
    public class MusicManager : MonoBehaviour
    {
        public AudioSource AudioSource;
        public List<LevelAudio> AudioClips;
        
        public static MusicManager Instance { get; private set; }
        
        private void Awake() => 
            DontDestroyOnLoad(gameObject);

        private void Start() => 
            Instance = this;
    }
}