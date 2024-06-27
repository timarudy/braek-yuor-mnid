using UnityEngine;
using UnityEngine.Rendering;

namespace Levels.ThirdLevel
{
    public class MudOnTrigger : MonoBehaviour
    {
        [SerializeField] private VolumeProfile BrownProfile;
        [SerializeField] private Volume GlobalVolume;
        
        private void OnDestroy() => 
            GlobalVolume.profile = null;

        private void OnTriggerEnter(Collider other) => 
            GlobalVolume.profile = BrownProfile;
    }
}