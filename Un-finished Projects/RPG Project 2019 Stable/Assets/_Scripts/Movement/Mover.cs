using UnityEngine;
using UnityEngine.AI;
using RPG.Core;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction
    {
        [Header("Set dynamically")]
        [SerializeField] NavMeshAgent playerNavAgent;
        [SerializeField] ActionScheduler actionScheduler;

        private void Awake()
        {
            AssignReferences();
        }

        void AssignReferences()
        {
            playerNavAgent = GetComponent<NavMeshAgent>();
            actionScheduler = GetComponent<ActionScheduler>();
        }

        public void StartMoveAction(Vector3 clickedPoint)
        {
            actionScheduler.StartAction(this);
            MoveTo(clickedPoint);
        }

        public void MoveTo(Vector3 clickedPoint)
        {
            playerNavAgent.destination = clickedPoint;
            playerNavAgent.isStopped = false;
        }

        public void Cancel()
        {
            StopMoving();
        }

        void StopMoving()
        {
            playerNavAgent.isStopped = true;
        }
    }
}
