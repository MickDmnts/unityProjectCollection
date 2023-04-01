using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [Header("Set in inspector")]
    public int damage = 10;
    public float raycastRange = 100f;

    [Header("Set Dynamically")]
    public Camera mainCamera;

    private void Awake()
    {
        AssignMainCameraRef();
    }

    void AssignMainCameraRef()
    {
        mainCamera = GetComponent<Camera>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            ShootRayAndDamageIfEnemy();
        }
    }

    void ShootRayAndDamageIfEnemy()
    {
        RaycastHit hit;
        var mousePos2D = Input.mousePosition;
        mousePos2D.z = raycastRange;
        Vector3 mousePos3D = mainCamera.ScreenToWorldPoint(mousePos2D);
        if (Physics.Raycast(mainCamera.transform.position, mousePos3D, out hit))
        {
            if (hit.transform.tag == "Enemy")
            {
                hit.transform.GetComponent<Enemy>().DamageEnemyByValue(damage);
            }
        }
    }
}
