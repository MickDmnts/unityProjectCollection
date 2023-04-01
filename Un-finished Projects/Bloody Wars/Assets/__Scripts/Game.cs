using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    [Header("Set dynamically")]
    public Transform projectileAnchor;

    private void Awake()
    {
        CreateProjectileAnchor();
    }

    void CreateProjectileAnchor()
    {
        GameObject go = new GameObject("ProjectileAnchor");
        projectileAnchor = go.transform;
    }
}
