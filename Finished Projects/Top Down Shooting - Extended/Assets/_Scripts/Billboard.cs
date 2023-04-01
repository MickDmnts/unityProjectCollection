using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    [Header("Set Dynamically")]
    public Transform mainCamera;

    private void Awake()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").transform;
    }


    void LateUpdate()
    {
        transform.LookAt(transform.position - mainCamera.position);
    }
}
