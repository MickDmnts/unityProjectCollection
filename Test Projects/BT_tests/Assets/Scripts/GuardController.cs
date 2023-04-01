using UnityEngine.AI;
using UnityEngine;

public class GuardController : MonoBehaviour {

    private NavMeshAgent nav;

    private enum State {
        IDLE,
        CHASE
    }

    private State state;

    [SerializeField]
    private float maxSenseDistance = 10;

    [SerializeField]
    private Transform target;

    public void Start() {
        nav = GetComponent<NavMeshAgent>();
        state = State.IDLE;
    }

    public void Update() {

        if (target != null) {

            // first, check if a state transition is needed...
            // (i.e., apply the state transition logic)...

            // check distance between me and target...
            float d = Vector3.Distance(transform.position, target.position);

            if (d < maxSenseDistance) {
                // todo: check for entry and exit actions...
                // if (state != State.CHASE) {
                    // todo: run exit actions of state...
                // }
                state = State.CHASE;
                // {
                    // todo: run entry actions of state...
                // }
            }
            else {
                // todo: check for entry and exit actions...
                // if (state != State.CHASE) {
                    // todo: run exit actions of state...
                // }
                state = State.IDLE;
                // {
                    // todo: run entry actions of state...
                // }
            }
        }

        // second, decide upon next action based on the current state...

        switch (state) {

            case State.IDLE:
                // todo: move the change to isStopped to the state's entry actions...
                // otherwise, this state is nop...
                maxSenseDistance = 10;
                nav.isStopped = true;
                break;

            case State.CHASE:
                nav.SetDestination(target.position);
                // todo: move the change to isStopped to the state's entry actions...
                maxSenseDistance = 20;
                nav.isStopped = false;
                break;
        }
    }

    private void OnDrawGizmos() {
        
        switch (state) {

            case State.IDLE:
                Gizmos.color = Color.green;
                break;
            case State.CHASE:
                Gizmos.color = Color.red;
                break;
        }

        Gizmos.DrawWireSphere(transform.position, maxSenseDistance);
    }
}
