using UnityEngine;

namespace SoundManagement
{
    public interface ISoundService
    {
        void PlayStoneOpeningSound(Vector3 at);
        void PlayFailSound(AudioSource audioSource);
    }
}