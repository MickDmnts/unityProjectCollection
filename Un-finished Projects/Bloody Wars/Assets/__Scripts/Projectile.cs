using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Set in inspector")]
    public float zRotationPerFrame = 15f;
    public float speedMultiplier = 1f;

    //Privates\\
    private GameObject projectileGO;
    public Transform projectileAnchor;

    private void Start()
    {
        CacheGameobjectRef();
        FindProjectileAnchor();
        MakeProjectileAnchorChild();
    }

    void CacheGameobjectRef()
    {
        projectileGO = this.gameObject;
    }

    void FindProjectileAnchor()
    {
        projectileAnchor = GameObject.Find("ProjectileAnchor").transform;
    }

    void MakeProjectileAnchorChild()
    {
        this.gameObject.transform.parent = projectileAnchor;
    }

    private void FixedUpdate()
    {
        RotateProjectile();
        MoveProjectile();
    }

    void RotateProjectile()
    {
        projectileGO.transform.Rotate(0f, 0f, zRotationPerFrame);
    }

    void MoveProjectile()
    {
        projectileGO.transform.Translate(-speedMultiplier*Time.deltaTime, 0f, 0f, Space.World);
    }
}
