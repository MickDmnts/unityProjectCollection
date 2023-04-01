using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slingshot : MonoBehaviour
{
    [Header("Set in inspector")]
    public GameObject prefabProjectile;
    public float velocityMultiplier = 8f;

    [Header("Set Dynamically")]
    public GameObject launchPoint;
    public Vector3 launchPos;
    public GameObject projectile;
    public bool aimingMode;

    private Rigidbody projectileRigidbody;

    void Awake()
    {
        InitializeLaunchPoint();
        LaunchPointToggle();
    }

    void InitializeLaunchPoint()
    {
        Transform launchPointTrans = transform.Find("LaunchPoint");
        launchPoint = launchPointTrans.gameObject;
        launchPos = launchPointTrans.position;
    }

    void LaunchPointToggle()
    {
        if (launchPoint.activeSelf)
        {
            launchPoint.SetActive(false);
        }
        else
        {
            launchPoint.SetActive(true);
        }
    }

    void OnMouseEnter()
    {
        LaunchPointToggle();
    }

    void OnMouseExit()
    {
        LaunchPointToggle();
    }

    private void OnMouseDown()
    {
        SetAimingMode(true);
        InitilizeProjectile();
    }

    void SetAimingMode(bool state)
    {
        aimingMode = state;
    }

    void InitilizeProjectile()
    {
        projectile = Instantiate(prefabProjectile) as GameObject;
        projectile.transform.position = launchPos;
        projectileRigidbody = projectile.GetComponent<Rigidbody>();
        SetProjectileKinematicState(true);
    }

    void SetProjectileKinematicState(bool state)
    {
        projectileRigidbody.isKinematic = state;
    }

    private void Update()
    {
        if (!aimingMode)
        {
            return;
        }

        Vector3 mousePos3D = Get3DWorldMousePosition();

        Vector3 mouseDelta = mousePos3D - launchPos;

        Vector3 projectilePos = launchPos + LimitProjectileDistanceFromSlingshot(mouseDelta);
        projectile.transform.position = projectilePos;

        if (Input.GetMouseButtonUp(0))
        {
            ReleaseProjectile(mouseDelta);
        }
    }

    Vector3 Get3DWorldMousePosition()
    {
        Vector3 mousePos2D = Input.mousePosition;
        mousePos2D.z = -Camera.main.transform.position.z;
        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);
        return mousePos3D;
    }

    Vector3 LimitProjectileDistanceFromSlingshot(Vector3 mouseDelta)
    {
        float sphereColliderRadius = this.GetComponent<SphereCollider>().radius;
        if (mouseDelta.magnitude > sphereColliderRadius)
        {
            mouseDelta.Normalize();
            mouseDelta *= sphereColliderRadius;
        }
        return mouseDelta;
    }

    void ReleaseProjectile(Vector3 mouseDelta)
    {
        SetAimingMode(false);
        SetProjectileKinematicState(false);
        projectileRigidbody.velocity = -mouseDelta * velocityMultiplier;
        CameraFollow.POI = projectile;
        projectile = null;
    }
}
