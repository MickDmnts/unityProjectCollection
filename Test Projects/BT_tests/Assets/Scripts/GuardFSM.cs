using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

using AI.FSM;

/**
 * 
 */
public class PatrolState : IState {

    private GuardFSM fsm;

    private int currentPatrolPointIndex = 0;

    public PatrolState(GuardFSM fsm) {
        this.fsm = fsm;
    }

    public FSM GetFSM() {
        return fsm;
    }

    public string GetName() {
        return "Patrol";
    }

    public void OnEntry() {
        fsm.senseDistance = fsm.idleSenseDistance;
        if (fsm.patrolPoints.Count != 0) {
            fsm.nav.isStopped = false;
        }
    }

    public void OnExit() {
    }

    public void OnUpdate() {

        if (Vector3.Distance(fsm.guard.position, fsm.player.position) < fsm.senseDistance) {
            fsm.MakeTransition(fsm.chaseState);
        }
        else {

            if (fsm.patrolPoints.Count != 0) {

                if (Vector3.Distance(fsm.guard.position, fsm.patrolPoints[currentPatrolPointIndex].position) <= fsm.proximityThreshold) {
                    currentPatrolPointIndex++;
                    if (currentPatrolPointIndex == fsm.patrolPoints.Count) {
                        currentPatrolPointIndex = 0;
                    }
                }

                Transform currentPatrolPoint = fsm.patrolPoints[currentPatrolPointIndex];

                fsm.nav.SetDestination(currentPatrolPoint.position);
            }
        }
    }

    public void OnVisualise() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(fsm.guard.position, fsm.senseDistance);
    }
}

/**
 * 
 */
public class IdleState : IState {

    private GuardFSM fsm;

    public IdleState(GuardFSM fsm) {
        this.fsm = fsm;
    }

    public FSM GetFSM() {
        return fsm;
    }

    public string GetName() {
        return "Idle";
    }

    public void OnEntry() {
        fsm.senseDistance = fsm.idleSenseDistance;
        fsm.nav.isStopped = true;
    }

    public void OnExit() {
    }

    public void OnUpdate() {

        if (Vector3.Distance(fsm.guard.position, fsm.player.position) < fsm.senseDistance) {
            fsm.MakeTransition(fsm.chaseState);
        }
        else {
            // nop...
        }
    }

    public void OnVisualise() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(fsm.guard.position, fsm.senseDistance);
    }
}

/**
 * 
 */
public class ChaseState : IState {

    private GuardFSM fsm;

    public ChaseState(GuardFSM fsm) {
        this.fsm = fsm;
    }

    public FSM GetFSM() {
        return fsm;
    }

    public string GetName() {
        return "Chase";
    }

    public void OnEntry() {
        fsm.senseDistance = fsm.chaseSenseDistance;
        fsm.nav.isStopped = false;
    }

    public void OnExit() {
        fsm.nav.isStopped = true;
    }

    public void OnUpdate() {

        if (Vector3.Distance(fsm.guard.position, fsm.player.position) > fsm.senseDistance) {
            fsm.MakeTransition(fsm.patrolState);
        }
        else {
            fsm.nav.SetDestination(fsm.player.position);
        }
    }

    public void OnVisualise() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(fsm.guard.position, fsm.senseDistance);
    }
}

/**
 * 
 */
public class GuardFSM : FSM {

    public float senseDistance = 10;
    public float idleSenseDistance = 10;
    public float chaseSenseDistance = 20;

    public float proximityThreshold = 1;

    public List<Transform> patrolPoints;

    public Transform player;
    public Transform guard;

    /*
    public IState[] states;
    */

    public PatrolState patrolState;
    public ChaseState chaseState;

    public NavMeshAgent nav;

    public GuardFSM(float idleSenseDistance, float chaseSenseDistance, List<Transform> patrolPoints, float proximitySensor, Transform player, Transform guard) {

        this.idleSenseDistance = senseDistance;
        this.chaseSenseDistance = chaseSenseDistance;

        this.patrolPoints = patrolPoints;

        this.guard = guard;
        this.player = player;

        nav = guard.GetComponent<NavMeshAgent>();

        /*
        states = new IState[] {
            new PatrolState(this),
            new ChaseState(this)
        };
        */

        patrolState = new PatrolState(this);
        chaseState = new ChaseState(this);

        MakeTransition(patrolState);
    }
}
