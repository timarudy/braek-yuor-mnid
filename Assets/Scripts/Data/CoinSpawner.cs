using System;
using UnityEngine;

namespace Data
{
    public class CoinSpawner : MonoBehaviour
    {
        public int Id;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(transform.position, 0.3f);
        }
    }
}