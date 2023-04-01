using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public BoundsCheck bndsCheck;
    public ParticleSystem particlesSystem; //Used from the _Weapon & PowerUp scripts to set the particle color
    
    private Renderer rend;

    [Header("Set dynamically")]
    public Rigidbody rigid;

    [SerializeField]
    private WeaponType _wType;

    /// <summary>
    /// Returns the _wType masked field.
    /// Calls the SetType() function to set the _wType and colors
    /// </summary>
    public WeaponType wType
    {
        get
        {
            return _wType;
        }
        set
        {
            SetType(value); //Calls the SetType() function
        }
    }

    private void Awake()
    {
        bndsCheck = GetComponent<BoundsCheck>();
        rend = GetComponent<Renderer>();
        rigid = GetComponent<Rigidbody>();
        particlesSystem = GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        if (bndsCheck.offUp)
        {
            Destroy(this.gameObject);
        }
    }

    /// <summary>
    /// Sets the _type private field and colors this projectile to match the colors specified
    /// </summary>
    /// <param name="eType">The WeaponType to use</param>
    public void SetType(WeaponType eType)
    {
        //Set the _type;
        _wType = eType;
        WeaponDefinition def = Main.GetWeaponDefinition(_wType);
        rend.material.color = def.projectileColor;
    }
}
