using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Set dynamically")]
    public GameObject projectilePrefab;
    public Transform launchPoint;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            ShootProjectile();
        }
    }

    void ShootProjectile()
    {
        GameObject proj = Instantiate<GameObject>(projectilePrefab);
        proj.transform.position = launchPoint.position;
    }
}
