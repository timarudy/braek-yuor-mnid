using System;
using Player.Input;
using UnityEngine;

namespace Player.Movement
{
    [RequireComponent(typeof(FirstPersonMovement), typeof(ThirdPersonMovement))]
    public class ViewHandler : MonoBehaviour
    {
        public event Action<GameObject> OnCameraFollowChanged; 
        
        public StarterAssetsInputs Input;
        public GameObject FirstPersonCamera;
        public GameObject ThirdPersonCamera;
        public GameObject FirstPersonCameraFollow;
        public GameObject ThirdPersonCameraFollow;
        
        private FirstPersonMovement _firstPerson;
        private ThirdPersonMovement _thirdPerson;
        private GameObject _currentCameraFollow;

        private void OnEnable()
        {
            Input.ChangeViewEvent += ChangeView;
        }

        private void OnDisable()
        {
            Input.ChangeViewEvent -= ChangeView;
        }

        private void Start()
        {
            _firstPerson = GetComponent<FirstPersonMovement>();
            _thirdPerson = GetComponent<ThirdPersonMovement>();

            InitView();
        }

        private void InitView()
        {
            EnableFirstPerson();
            _currentCameraFollow = FirstPersonCameraFollow;
            OnCameraFollowChanged?.Invoke(_currentCameraFollow);
        }

        private void ChangeView(PersonView view)
        {
            switch (view)
            {
                case PersonView.FIRST:
                    EnableFirstPerson();
                    _currentCameraFollow = FirstPersonCameraFollow;
                    _firstPerson.InitBasicSettings();
                    break;
                case PersonView.THIRD:
                    EnableThirdPerson();
                    _currentCameraFollow = ThirdPersonCameraFollow;
                    break;
            }
            
            OnCameraFollowChanged?.Invoke(_currentCameraFollow);
        }

        public void EnableFirstPerson()
        {
            _thirdPerson.MainCamera.GetComponent<AudioListener>().enabled = false;
            _thirdPerson.enabled = false;
            
            _firstPerson.enabled = true;
            _firstPerson.MainCamera.GetComponent<AudioListener>().enabled = true;
            
            ThirdPersonCamera.SetActive(false);
            FirstPersonCamera.SetActive(true);
        }
        
        private void EnableThirdPerson()
        {
            _firstPerson.MainCamera.GetComponent<AudioListener>().enabled = false;
            _firstPerson.enabled = false;
            
            _thirdPerson.enabled = true;
            _thirdPerson.MainCamera.GetComponent<AudioListener>().enabled = true;

            FirstPersonCamera.SetActive(false);
            ThirdPersonCamera.SetActive(true);
        }
    }
}