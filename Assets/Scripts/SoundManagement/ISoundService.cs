using UnityEngine;

namespace SoundManagement
{
    public interface ISoundService
    {
        void PlayStoneOpeningSound(Vector3 at);
        void PlayFailSound(AudioSource audioSource);
        void PlayPickUpSound(AudioSource audioSource);
        void PlayStoneSound(AudioSource audioSource);
        void PlayStickSound(AudioSource audioSource);
        void PlayGrassSound(AudioSource audioSource);
        void PlayWateringCanSound(AudioSource audioSource);
        void PlayChopSound(AudioSource audioSource);
        void PlaySwordSound(AudioSource audioSource);
    }
}