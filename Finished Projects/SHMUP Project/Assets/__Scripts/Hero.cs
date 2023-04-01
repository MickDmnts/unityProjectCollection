using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    static public Hero S; //Singleton

    [Header("Set in Inspector")]
    public float speed = 30;
    public float rollMutl = -45;
    public float pitchMult = 30;
    public float gameRestartDelay = 2f;

    [Header("Projectile")]
    public GameObject projectilePrefab;
    public float projectileSpeed = 40;

    [Header("Set Dynamically")]
    [SerializeField]
    private float _shieldLevel = 1;

    private GameObject lastTriggeredGO;

    private void Awake()
    {
        if (S == null)
        {
            S = this;
        }
        else
        {
            Debug.LogError("Second Attempt to assign the S.");
        }
    }

    private void Update()
    {
        float xAxis = Input.GetAxis("Horizontal");
        float yAxis = Input.GetAxis("Vertical");

        Vector3 pos = transform.position;
        pos.x += xAxis * speed * Time.deltaTime;
        pos.y += yAxis * speed * Time.deltaTime;
        transform.position = pos;

        transform.rotation = Quaternion.Euler(yAxis * pitchMult, xAxis * rollMutl, 0);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            TempFire();
        }
    }

    private void TempFire()
    {
        GameObject projGo = Instantiate<GameObject>(projectilePrefab);
        projGo.transform.position = transform.position;
        Rigidbody rigidB = projGo.GetComponent<Rigidbody>();
        rigidB.velocity = Vector3.up * projectileSpeed;
    }

    private void OnTriggerEnter(Collider other)
    {
        Transform rootT = other.gameObject.transform.root;
        GameObject go = rootT.gameObject; //These 2 lines get the root of the GO through its Transform.
        //Debug.Log("Triggered: " + go.name);

        //Make sure its not the same GO as the last time
        if (go == lastTriggeredGO)
        {
            return;
        }
        lastTriggeredGO = go;

        //If the Hero ship collides with an enemy ship, decrease the Hero's shields and destroy the colliding enemy
        if (go.tag == "Enemy")
        {
            ShieldLevel--;
            Destroy(go);
        }
        else
        {
            Debug.Log("Triggered by non-Enemy: "+go.name);
        }
    }

    public float ShieldLevel
    {
        get
        {
            return _shieldLevel;
        }

        set
        {
            _shieldLevel = Mathf.Min(value, 4); //This line ensures that the shield level is never set above 4
            //If the shield is going to be set below 0, destroy the ship
            if (_shieldLevel < 0)
            {
                Destroy(this.gameObject);
                Main.S.DelayedRestart(gameRestartDelay);
            }
        }
    }
}
