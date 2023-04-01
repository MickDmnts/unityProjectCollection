using UnityEngine;
using UnityEngine.AI;
using RPG.Combat;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour
    {
        [Header("Set dynamically")]
        [SerializeField] NavMeshAgent playerNavAgent;
        [SerializeField] Fighter fighterScript;

        private void Awake()
        {
            AssignReferences();
        }

        void AssignReferences()
        {
            playerNavAgent = GetComponent<NavMeshAgent>();
            fighterScript = GetComponent<Fighter>();
        }

        public void StartMoveAction(Vector3 clickedPoint)
        {
            fighterScript.CancelAttacking();
            MoveTo(clickedPoint);
        }

        public void MoveTo(Vector3 clickedPoint)
        {
            playerNavAgent.destination = clickedPoint;
            playerNavAgent.isStopped = false;
        }

        public void StopMoving()
        {
            playerNavAgent.isStopped = true;
        }
    }
}
