using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartInitializations : MonoBehaviour
{
    [Header("Set in Inspector")]
    [Range(1f, 100f)]
    public float spawnFactor;

    [Header("Set Dynamically")]
    [SerializeField]
    GameObject playerGO;

    private void Awake()
    {
        playerGO = GameObject.Find("Player");
    }

    private void Start()
    {
        playerGO.transform.localPosition = playerGO.transform.localPosition + Vector3.up * spawnFactor;
    }
}
