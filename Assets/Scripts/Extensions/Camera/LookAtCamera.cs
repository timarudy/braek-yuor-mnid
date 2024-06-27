using UnityEngine;

namespace Extensions.Camera
{
    public class LookAtCamera : MonoBehaviour
    {
        [SerializeField] private LookMode Mode;

        private void LateUpdate()
        {
            switch (Mode)
            {
                case LookMode.LOOK_AT_CAMERA:
                    Vector3 dirFromCamera = transform.position - UnityEngine.Camera.main.transform.position;
                    transform.LookAt(transform.position + dirFromCamera);
                    break;
                case LookMode.LOOK_AT_FORWARD:
                    transform.forward = UnityEngine.Camera.main.transform.forward;
                    break;
            }
        }
    }
}