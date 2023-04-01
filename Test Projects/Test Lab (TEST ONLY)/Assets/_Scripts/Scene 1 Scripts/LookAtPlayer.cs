using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    public Transform player;
    private Camera m_Camera;

    void Awake()
    {
        m_Camera = GetComponent<Camera>();
    }

    void FixedUpdate()
    {
        m_Camera.transform.LookAt(player);
    }
}
