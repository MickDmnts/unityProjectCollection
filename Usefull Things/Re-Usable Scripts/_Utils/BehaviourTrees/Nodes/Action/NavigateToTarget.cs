using UnityEngine;
using UnityEngine.AI;

namespace AI.BT {

    public class NavigateToTarget: INode {

        private Transform target;

        private Transform me;

        private NavMeshAgent nav;

        public NavigateToTarget(Transform target, Transform me) {
            this.target = target;
            this.me = me;
            this.nav = me.GetComponent<NavMeshAgent>();
        }

        public bool Run() {

            if (Vector3.Distance(me.position, target.position) > 1.0) {

                nav.SetDestination(target.position);
                return true;
            }
            else {
                return false;
            }
        }
    }
}