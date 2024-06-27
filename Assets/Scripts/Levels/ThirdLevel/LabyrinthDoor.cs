using System;
using System.Collections;
using SoundManagement;
using UnityEngine;
using Zenject;

namespace Levels.ThirdLevel
{
    public class LabyrinthDoor : MonoBehaviour
    {
        [SerializeField] private Vector3 Destination;
        [SerializeField] private GameObject Particles;
        
        private ISoundService _soundService;
        private bool _isOpening;

        [Inject]
        private void Construct(ISoundService soundService)
        {
            _soundService = soundService;
        }
        
        private void Start()
        {
            Destination += transform.position;
        }

        public void OpenDoor() =>
            StartCoroutine(Open());

        private IEnumerator Open()
        {
            Particles.SetActive(true);
            _soundService.PlayStoneOpeningSound(transform.position);
            
            while (Vector3.Distance(transform.position, Destination) > 0.05f)
            {
                transform.position = Vector3.MoveTowards(transform.position, Destination, Time.deltaTime);
                yield return null;
            }

            Destroy(gameObject);
        }
    }
}