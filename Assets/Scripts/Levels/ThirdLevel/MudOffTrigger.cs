using UnityEngine;
using UnityEngine.Rendering;

namespace Levels.ThirdLevel
{
    public class MudOffTrigger : MonoBehaviour
    {
        [SerializeField] private Volume GlobalVolume;

        private void OnTriggerEnter(Collider other) => 
            GlobalVolume.profile = null;
    }
}