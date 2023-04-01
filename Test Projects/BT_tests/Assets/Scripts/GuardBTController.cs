using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.AI;

using AI.BT;

public class GuardBTController : MonoBehaviour
{
    BehaviourTree guardBT;
    BehaviourTree patrolBT;

    [SerializeField] private Transform player;
    [SerializeField] List<Transform> waypoints;
    [SerializeField] private float idleSenseDistance;
    [SerializeField] float waypointDistanceThreshold;

    NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public void Start()
    {
        //Patrol BT
        if (waypoints.Count != 0)
        {
            List<INode> patrolNavTo = new List<INode>();

            foreach (Transform point in waypoints)
            {
                INode navToPoint = new NavigateToTarget(point, transform, waypointDistanceThreshold);
                patrolNavTo.Add(navToPoint);
            }

            INode patrolSelector = new Selector(patrolNavTo);
            patrolBT = new BehaviourTree(patrolSelector);
        }

        //Idle chase BT
        INode navigateToTarget = new NavigateToTarget(player, transform, waypointDistanceThreshold);
        INode canChase = new DecoratorCheckDistance(navigateToTarget, transform, player, idleSenseDistance);

        //For separate branch running
        List<INode> nodes = new List<INode>();
        nodes.Add(canChase);
        if (patrolBT != null) nodes.Add(patrolBT);
        INode selector = new Selector(nodes); //change in BT

        //For parallel branch running
        List<INode> runInParallel = new List<INode>();
        runInParallel.Add(selector);
        if (patrolBT != null) runInParallel.Add(patrolBT);
        INode parallel = new Parallel(runInParallel); //change in BT

        //Main tree
        guardBT = new BehaviourTree(parallel);
    }

    public void Update()
    {
        guardBT.Run();
    }

    private void OnDrawGizmos()
    {
        Handles.color = Color.green;
        Handles.DrawWireDisc(transform.position, Vector3.up, idleSenseDistance);

        Handles.color = Color.cyan;
        Handles.DrawWireDisc(transform.position, Vector3.up, waypointDistanceThreshold);

        if (agent != null)
        {
            DrawPath();
        }
    }

    void DrawPath()
    {
        NavMeshPath path = agent.path;

        for (int i = 0; i < path.corners.Length - 1; i++)
        {
            Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.cyan);
        }
    }
}
