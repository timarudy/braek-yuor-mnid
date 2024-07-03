using System.Collections.Generic;
using Infrastructure.AssetManagement;
using UnityEngine;
using Zenject;

namespace SoundManagement
{
    public class SoundService : ISoundService
    {
        private const float DefaultVolume = 0.1f;

        private readonly SOSounds _sounds;

        public SoundService(IAssetProvider assetProvider)
        {
            _sounds = assetProvider.LoadResource<SOSounds>(AssetPath.SoundsPath);
        }

        public void PlayStoneOpeningSound(Vector3 at) =>
            PlaySound(_sounds.StoneDoorOpenSound, at);

        public void PlayFailSound(AudioSource audioSource) => 
            PlayRandomSound(audioSource, _sounds.FailSound);
        
        public void PlayStoneSound(AudioSource audioSource) => 
            PlayRandomSound(audioSource, _sounds.StoneSound);
        
        public void PlayStickSound(AudioSource audioSource) => 
            PlayRandomSound(audioSource, _sounds.StickSound);
        
        public void PlayGrassSound(AudioSource audioSource) => 
            PlayRandomSound(audioSource, _sounds.GrassSound);

        public void PlayWateringCanSound(AudioSource audioSource) => 
            PlayRandomSound(audioSource, _sounds.WateringCanSound);

        public void PlayChopSound(AudioSource audioSource) => 
            PlayRandomSound(audioSource, _sounds.ChopSound);

        public void PlaySwordSound(AudioSource audioSource) => 
            PlayRandomSound(audioSource, _sounds.SwordSound);

        public void PlayPickUpSound(AudioSource audioSource) => 
            PlayRandomSound(audioSource, _sounds.PickUpSound);

        private static void PlayRandomSound(AudioSource audioSource, List<AudioClip> sounds)
        {
            audioSource.clip = GetRandomAudioClip(sounds);
            audioSource.Play();
        }

        private static void PlaySound(IReadOnlyList<AudioClip> sounds, Vector3 position, float volume = DefaultVolume) =>
            PlaySound(GetRandomAudioClip(sounds), position, volume);

        private static void PlaySound(AudioClip sound, Vector3 position, float volume = DefaultVolume) =>
            AudioSource.PlayClipAtPoint(sound, position, volume);

        private static AudioClip GetRandomAudioClip(IReadOnlyList<AudioClip> sounds) => 
            sounds[Random.Range(0, sounds.Count)];
    }
}