using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Set in Inspector: Enemy")]
    public float speed = 10f;
    public float fireRate = 0.3f;
    public float health = 10;
    public int score = 100; //Score you get from destroying this enemy
    public float showDamageDuration = 0.1f; // # of seconds to show damage
    public float powerUpDropChance = 1f;

    [Header("Set Dynamically: Enemy")]
    public Color[] originalColors;
    public Material[] materials; //All the materials of this GO and its Children
    public bool showingDamage = false;
    public float damageDoneTime; //Time to stop showing damage
    public bool notifiedOfDestruction = false; //Will be used later

    protected BoundsCheck bndCheck; 

    private void Awake()
    {
        bndCheck = GetComponent<BoundsCheck>();

        //Get materials and colors for this GO and its children, add them to their respective arrays
        materials = Utils.GetAllMaterials(this.gameObject);
        originalColors = new Color[materials.Length];
        for (int i = 0; i < materials.Length; i++)
        {
            originalColors[i] = materials[i].color;
        }
    }

    /// <summary>
    /// A property
    /// returns the position of the ship
    /// sets the position of the ship to a given value
    /// </summary>
    public Vector3 pos
    {
        get
        {
            return this.transform.position;
        }
        set
        {
            transform.position = value;
        }
    }

    /// <summary>
    /// Calls the Move() function every frame.
    /// Checks the ships position
    /// </summary>
    private void Update()
    {
        Move();

        if (showingDamage && Time.time > damageDoneTime) //Sepcified amount of time have passes, so revert back to original colors.
        {
            UnShowDamage();
        }

        if (bndCheck != null && bndCheck.offDown) //Check to see if the enemy is inside the play area, if not then Destroy() it.
        {
            Destroy(this.gameObject);
        }
    }

    private void UnShowDamage() //Set the colors of each enemy back to normal
    {
        for (int i = 0; i < materials.Length; i++)
        {
            materials[i].color = originalColors[i];
        }
        showingDamage = false;
    }

    /// <summary>
    /// Stores the current position of the enemy ship
    /// then sets its pos to -speed*time.deltaTime (-speed is used to go downwards)
    /// then sets it back to the GO
    /// </summary>
    public virtual void Move() 
    {
        Vector3 temp = pos;
        temp.y -= speed * Time.deltaTime;
        pos = temp;
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject go = collision.gameObject;
        switch (go.tag)
        {
            case "ProjectileHero":
                Projectile proj = go.GetComponent<Projectile>();
                //If this enemy is off screen
                if (bndCheck.isOnScreen == false)
                {
                    Destroy(go);
                    return;
                }
                
                //Hurt the enemy
                health -= Main.GetWeaponDefinition(proj.wType).damageOnHit;  //Gets the weapon damage from the main WEAP_DICT
                //Show the damage
                ShowDamage();

                if (health <= 0)
                {
                    //Tell the Main Singleton that this ship was destroyed
                    if (!notifiedOfDestruction)
                    {
                        Main.S.ShipDestroyed(this);
                    }
                    notifiedOfDestruction = true;
                    Destroy(this.gameObject);
                }
                Destroy(go);
                break;
            default:
                print("Hit by a non-ProjectileHero: " + go.name);
                return;
        }
    }

    private void ShowDamage() //Turn all the materials of the GO to red for a specified amount of time
    {
        foreach (Material m in materials)
        {
            m.color = Color.red;
        }
        showingDamage = true;
        damageDoneTime = Time.time + showDamageDuration;
    }
}