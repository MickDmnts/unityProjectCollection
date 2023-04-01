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
    public _Weapon[] weapons; //the weapons array of the ship

    [Header("Set Dynamically")]
    [SerializeField]
    private float _shieldLevel = 1;

    private GameObject lastTriggeredGO;

    //Create a new delegate type called WeaponFireDelegate;
    public delegate void WeaponFireDelegate();

    //Create a WeaponFireDelegate called fireDelegate;
    public WeaponFireDelegate fireDelegate;

    void Awake()
    {
        S = this;
    }

    private void Start()
    {   
        //Reset the weapon[] to start ship with 1 Blaster
        ClearWeapons(); //Call ClearWeapons();
        weapons[0].SetType(WeaponType.blaster); //Set the first weapon to Blaster;
    }

    private void Update()
    {
        float xAxis = Input.GetAxis("Horizontal");
        float yAxis = Input.GetAxis("Vertical");

        Vector3 pos = transform.position;
        pos.x += xAxis * speed * Time.deltaTime; //Time_based movement
        pos.y += yAxis * speed * Time.deltaTime; //Time_based movement
        transform.position = pos;

        transform.rotation = Quaternion.Euler(yAxis * pitchMult, xAxis * rollMutl, 0); //pitching and rolling of the ship

        if (Input.GetAxis("Jump") == 1 && fireDelegate != null)
        {
            fireDelegate(); //Calls the delegate function;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Transform rootT = other.gameObject.transform.root;
        GameObject go = rootT.gameObject; //These 2 lines get the root of the GO through its Transform.

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
        else if (go.tag == "PowerUp") //If the shield was triggered by a powerUp
        {
            AbsorbPowerUp(go); 
        }
        else
        {
            Debug.Log("Triggered by non-Enemy: "+go.name);
        }
    }

    public void AbsorbPowerUp(GameObject go)
    {
        PowerUp pu = go.GetComponent<PowerUp>();
        switch (pu.type)
        {
            case WeaponType.shield: //If the powerUp is a WeaponType.shield
                ShieldLevel++; //Raise it up one level
                break;

            default:
                if ((int)pu.type < (int)weapons[0].type ) //If the PowerUp is weaker than the current WeaponType.\\ The casting takes the int number of the enum and compares it
                {
                    break;
                }
                else if ((int)pu.type == (int)weapons[0].type) //if it is the same type with the first weapon, get an empty weapon slot and set it to the collected PU
                {
                    _Weapon w = GetEmptyWeaponSlot();
                    if (w != null)
                    {
                        w.SetType(pu.type);
                    }
                    else
                    {
                        Debug.Log("There is no empty weapon slot");
                    }
                }
                else
                {
                    ClearWeapons();
                    weapons[0].SetType(pu.type);
                }
                break;
        }
        pu.AbsorbedBy(this.gameObject);
    }

    public float ShieldLevel //Update the shield OR destroy the ship and restart
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

    _Weapon GetEmptyWeaponSlot() //Search for an empty weapon[] slot
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            if(weapons[i].type == WeaponType.none) //empty weapon slot
            {
                return weapons[i]; //Returs the empty slot for the weapon
            }
        }
        return null;
    }

    void ClearWeapons() //Clear all the weapon[] slots
    {
        foreach (_Weapon w in weapons)
        {
            w.SetType(WeaponType.none); //Sets the all the weapons to none
        }
    }
}
