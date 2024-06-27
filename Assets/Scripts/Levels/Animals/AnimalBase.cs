using Player;
using UI.Windows;
using UnityEngine;
using UnityEngine.AI;

namespace Levels.Animals
{
    public class AnimalBase : MonoBehaviour
    {
        private const float TeleportDistance = 10f;
        private const float StopDistance = 2f;

        public AnimalType AnimalType;
        public NavMeshAgent Agent;
        public PlayerFollow Following { get; set; }

        private AnimalAnimator _animalAnimator;

        private void Start()
        {
            _animalAnimator = GetComponent<AnimalAnimator>();
        }

        private void Update()
        {
            Agent.destination = Following.transform.position;

            if (IsTooFar())
            {
                gameObject.SetActive(false);
                transform.position = Following.AnimalSpawnPoint.position;
                gameObject.SetActive(true);
            }
            
            if (ReachedFollowing())
            {
                _animalAnimator.Idle();
            }
            else
            {
                _animalAnimator.Walk();
            }
        }

        private bool IsTooFar() => 
            Vector3.Distance(Agent.transform.position, Following.transform.position) > TeleportDistance;

        private bool ReachedFollowing()
        {
            Vector3 agentPosition = new(Agent.transform.position.x, 0f, Agent.transform.position.z);
            Vector3 followingPosition = new(Following.transform.position.x, 0f, Following.transform.position.z);
            float distance = Vector3.Distance(agentPosition, followingPosition);

            return distance < StopDistance;
        }
    }
}