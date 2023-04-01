using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    none = 0, //no weapons
    blaster = 1, //Simple blaster
    spread = 2, //5-shot type weapon
    phaser = 3,
    missile = 4,
    laser = 5,
    shield = 6, // Raises shieldLevel
}

/// <summary>
/// The WeaponDefinition class allows you to set the properties of a specific weapon
/// in the inspector. The Main class has an array of WeaponDefinition that makes this possible
/// </summary>
[System.Serializable]
public class WeaponDefinition
{
    [Header("Main visible properties")]
    //Main visible properties
    public WeaponType type = WeaponType.none;
    public string letter; //Letter to show on the power-up
    public Color color = Color.white; //Collor of Collar & power-up
    public GameObject projectilePrefab;
    public Color projectileColor = Color.white;
    public Color particleColor = Color.white; //the color of the emmited particles

    [Header("Main properties of each Weapon")]
    //Main properties of each weapon
    public float damageOnHit = 0; //Damage of weapon
    public float continuousDamage = 0; //continuous damage delt
    public float delayBetweenShots = 0; //delay between each shot
    public float velocity = 20; //Speed of each -bullet-
}

/// <summary>
/// MAIN SCRIPT
/// </summary>
public class _Weapon : MonoBehaviour
{
    static public Transform PROJECTILE_ANCHOR;

    [Header("Set Dynamically")]
    [SerializeField]
    private WeaponType _wType = WeaponType.none;
    public WeaponDefinition def;
    public GameObject collar;
    public float lastShotTime; //Time last shot was fired

    //Private Vars\\
    private Renderer collarRend;

    private void Start()
    {
        collar = transform.Find("Collar").gameObject;
        collarRend = collar.GetComponent<Renderer>();

        //calls SetType for the default WeaponType
        SetType(_wType);

        //Dynamically create an Anchor for all projectiles
        if (PROJECTILE_ANCHOR == null)
        {
            GameObject go = new GameObject("_ProjectileAnchor");
            PROJECTILE_ANCHOR = go.transform;
        }

        //Find the fireDelegate of the root GO
        GameObject rootGO = transform.root.gameObject;
        if (rootGO.GetComponent<Hero>() != null)
        {
            rootGO.GetComponent<Hero>().fireDelegate += Fire;
        }
    }

    public WeaponType type
    {
        get
        {
            return _wType;
        }
        set
        {
            SetType(value);
        }
    }

    public void SetType(WeaponType wt)
    {
        _wType = wt;
        if (type == WeaponType.none)
        {
            this.gameObject.SetActive(false);
            return;
        }
        else
        {
            this.gameObject.SetActive(true);
        }
        def = Main.GetWeaponDefinition(_wType);
        collarRend.material.color = def.color;  
        lastShotTime = 0; //You can fire immediately after _wType is set;
    }

    public void Fire()
    {
        if (!gameObject.activeInHierarchy) //If this.gameobject is inactive, return
        {
            return;
        }

        if (Time.time - lastShotTime < def.delayBetweenShots)
        {
            return;
        }

        Projectile p;
        Vector3 vel = Vector3.up * def.velocity; //Push the projectile up
        if (transform.up.y < 0)
        {
            vel.y = -vel.y;
        }

        switch (type)
        {
            case WeaponType.blaster:
                p = MakeProjectile();
                p.rigid.velocity = vel;
                break;
            case WeaponType.spread:
                p = MakeProjectile(); //Make middle projectile;
                p.rigid.velocity = vel;

                p = MakeProjectile(); //Make right projectile;
                p.transform.rotation = Quaternion.AngleAxis(10, Vector3.back);
                p.rigid.velocity = p.transform.rotation * vel;

                p = MakeProjectile(); //Make middle-right projectile
                p.transform.rotation = Quaternion.AngleAxis(5,Vector3.back);
                p.rigid.velocity = p.transform.rotation * vel;

                p = MakeProjectile(); //Make left projectile;
                p.transform.rotation = Quaternion.AngleAxis(-10, Vector3.back);
                p.rigid.velocity = p.transform.rotation * vel;

                p = MakeProjectile(); //Make middle-left projectile
                p.transform.rotation = Quaternion.AngleAxis(-5,Vector3.back);
                p.rigid.velocity = p.transform.rotation * vel;
                break;
        }
    }

    public Projectile MakeProjectile()
    {
        GameObject go = Instantiate<GameObject>(def.projectilePrefab);
        if (transform.parent.gameObject.tag == "Hero")
        {
            go.tag = "ProjectileHero";
            go.layer = LayerMask.NameToLayer("ProjectileHero");
        }
        else
        {
            go.tag = "ProjectileEnemy";
            go.layer = LayerMask.NameToLayer("ProjectileEnemy");
        }

        go.transform.position = collar.transform.position;
        go.transform.SetParent(PROJECTILE_ANCHOR, true);
        Projectile p = go.GetComponent<Projectile>();
        p.wType = type;
        lastShotTime = Time.time; //Controls the fire-rate

        //Colorize the particles behind the projectile
        float colR = def.particleColor.r;
        float colG = def.particleColor.g;
        float colB = def.particleColor.b;
        float colA = def.particleColor.a;
        var mainColor = p.particlesSystem.main;
        mainColor.startColor = new Color(colR,colG,colB,colA);

        //Return the projectile and shoot
        return p;
    }
}
