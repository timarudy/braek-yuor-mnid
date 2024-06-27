using System;
using Player;
using Player.Movement;
using UnityEngine;
using Zenject;


namespace Levels.ThirdLevel
{
    public class MudBarrier : MonoBehaviour
    {
        [SerializeField] private GameObject Blocker;
        
        private PlayerController _player;

        [Inject]
        private void Construct(PlayerController player)
        {
            _player = player;
            player.OnDirtSkinWornOn += Open;
            player.OnDirtSkinWornOff += Close;
        }

        private void OnDisable()
        {
            _player.OnDirtSkinWornOn -= Open;
            _player.OnDirtSkinWornOff -= Close;
        }

        private void Close() => 
            Blocker.gameObject.SetActive(true);

        public void Open() =>
            Blocker.gameObject.SetActive(false);

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out PlayerMovement player))
            {
                player.MoveSpeed = 1f;
                player.SprintSpeed = 2f;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out PlayerMovement player))
            {
                player.MoveSpeed = 4f;
                player.SprintSpeed = 6f;
            }
        }
    }
}