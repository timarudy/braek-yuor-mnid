using Player;
using Services.Progress;
using Services.Progress.SaveLoadService;
using UnityEngine;
using Zenject;

namespace Levels.Coins
{
    public class CollectableBase : MonoBehaviour
    {
        private const float LevitatingAltitude = 0.27f;

        protected PlayerController Player;
        protected IPersistentProgressService ProgressService;
        
        private bool _isCollecting;
        private float _collectingSpeed = 0.01f;
        private bool _isFalling;
        private ISaveLoadService _saveLoadService;

        [Inject]
        private void Construct(PlayerController player, IPersistentProgressService progressService, ISaveLoadService saveLoadService)
        {
            Player = player;
            ProgressService = progressService;
            _saveLoadService = saveLoadService;
        }

        private void Update()
        {
            if (_isCollecting)
            {
                Vector3 targetPosition = Vector3.Lerp(transform.position, Player.CollectPosition.position,
                    _collectingSpeed * Time.deltaTime);

                transform.position = targetPosition;

                _collectingSpeed *= 1.03f;

                if (IsCollectable())
                    Collect();
            }

            if (_isFalling)
            {
                Vector3 targetPosition = Vector3.Lerp(transform.position, new Vector3(transform.position.x, LevitatingAltitude, transform.position.z),
                    2 * Time.deltaTime);

                transform.position = targetPosition;
            }

        }

        private void OnTriggerEnter(Collider other)
        {
            _isFalling = false;
            _isCollecting = true;
        }

        private void OnTriggerExit(Collider other)
        {
            _isFalling = true;
            _isCollecting = false;
            _collectingSpeed = 0.001f;
        }

        public void Fall() => 
            _isFalling = true;

        protected virtual void Collect()
        {
            _saveLoadService.UpdatePlayerPrefs();

            _isCollecting = false;
            DestroySelf();
        }

        private void DestroySelf() =>
            Destroy(gameObject);

        private bool IsCollectable() =>
            Vector3.Distance(transform.position, Player.CollectPosition.position) <= 0.2f;
    }
}