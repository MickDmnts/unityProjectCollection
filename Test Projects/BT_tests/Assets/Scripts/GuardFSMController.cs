using System.Collections.Generic;
using UnityEngine;

public class GuardFSMController : MonoBehaviour {
    
    private GuardFSM fsm;

    [SerializeField]
    private float idleSenseDistance;

    [SerializeField]
    private float chaseSenseDistance;

    [SerializeField]
    private Transform player;
    
    [SerializeField]
    private float proximityThreshold;

    [SerializeField]
    private List<Transform> patrolPoints;

    public void Awake() {

        fsm = new GuardFSM(idleSenseDistance, chaseSenseDistance, patrolPoints, proximityThreshold, player, transform);
    }

    public void Start() {
    }

    public void Update() {
        fsm.Update();
    }

    public void OnDrawGizmos() {
        if (fsm != null) {
            fsm.Visualise();
        }
    }
}
