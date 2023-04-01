using UnityEngine;
using UnityEngine.AI;

public class Agent : MonoBehaviour
{
    [Header("Set in inspector")]
    [SerializeField] Transform target;

    //NavMeshAgent component
    NavMeshAgent meshAgent;

    private void Awake()
    {
        //Cache the component
        meshAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        //Navigate to the target
        meshAgent.SetDestination(target.position);
    }
}
