using System;
using Data;
using Services.Progress;
using UnityEngine;

namespace Levels.Enemies
{
    public class EnemySpawner : MonoBehaviour, ISavedProgress
    {
        public EnemyType EnemyType;
        public Transform SpawnObjectsNativeTransformParent;
        
        private string _id;
        private bool _isKilled;

        private void OnEnable()
        {
            _id = GetComponent<UniqueId>().Id;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position, 0.5f);
        }

        public void LoadProgress(PlayerProgress progress)
        {
            if (progress.LevelProgressData.KillData.ClearedSpawners.Contains(_id))
            {
                _isKilled = true;
            }
        }

        public void UpdateProgress(PlayerProgress progress)
        {
            if (_isKilled)
            {
                progress.LevelProgressData.KillData.ClearedSpawners.Add(_id);
            }
        }
    }
}