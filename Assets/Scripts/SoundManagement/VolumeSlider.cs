﻿using UnityEngine;
using UnityEngine.UI;

namespace SoundManagement
{
    public class VolumeSlider : MonoBehaviour
    {
        [SerializeField] private Slider VolumeSliderComponent;
        
        private MusicManager _musicManager;

        private void Awake()
        {
            _musicManager = MusicManager.Instance;
            VolumeSliderComponent.value = _musicManager.AudioSource.volume / 0.07f;
        }

        public void ChangeVolume() => 
            _musicManager.AudioSource.volume = VolumeSliderComponent.value * 0.07f;
    }
}