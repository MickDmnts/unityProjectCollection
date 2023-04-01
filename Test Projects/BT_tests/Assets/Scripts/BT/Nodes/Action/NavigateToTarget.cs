using UnityEngine;
using UnityEditor;
using UnityEngine.AI;

namespace AI.BT
{
    public class NavigateToTarget : INode
    {
        private Transform target;
        private Transform origin;

        private NavMeshAgent nav;

        float distanceThreshold;

        bool hasArrived = false;

        public NavigateToTarget(Transform target, Transform me, float distanceThreshold)
        {
            this.target = target;
            this.origin = me;

            this.nav = me.GetComponent<NavMeshAgent>();

            this.distanceThreshold = distanceThreshold;
        }

        public bool Run()
        {
            if (hasArrived) return false;

            if (Vector3.Distance(origin.position, target.position) > distanceThreshold)
            {
                nav.isStopped = false;
                nav.SetDestination(target.position);

                return true;
            }
            else
            {
                hasArrived = true;
                return false;
            }
        }

        public void OnReset()
        {
            Debug.Log("Reseted");
            hasArrived = false;
        }
    }
}