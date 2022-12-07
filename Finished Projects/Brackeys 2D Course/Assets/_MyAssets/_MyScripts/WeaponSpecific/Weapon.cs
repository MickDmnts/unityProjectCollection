
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("Main Weapon Attributes - Set in Inspector")]
    public bool isSingleFire = true;

    [Range(5,60)]
    public float fireRate = 0f;
    public float weaponDamage = 10f;
    public float weaponTrailsPerSecond = 10;
    public LayerMask whatLayersToHit;

    [Header("GameObjects - Set in Inspector")]
    public Transform weaponFiringPoint;
    public Transform weaponTrailPrefab;
    public GameObject weaponTrailAnchor;

    [Header("Variables are set Dynamically")]
    public float lastShotTime = 0;
    public float timeToSpawnEffect = 0;
    public WeaponMuzzleFlash muzzleFlashReference;

    private void Awake()
    {
        CheckIfFiringPointIsSet();
        SetMuzzleFlashScript();
        IsMuzzleFlashSet();
        SetWeaponTrailAnchor();
        IsAnchorSet();
    }

    void CheckIfFiringPointIsSet()
    {
        if (weaponFiringPoint == null)
        {
            Debug.Log("Firing Point is not set");
        }
    }

    void SetMuzzleFlashScript()
    {
        muzzleFlashReference = this.gameObject.GetComponent<WeaponMuzzleFlash>();
    }

    void IsMuzzleFlashSet()
    {
        if (muzzleFlashReference == null)
        {
            Debug.Log("Muzzle flash ref is not set in weapon script");
        }
    }

    void SetWeaponTrailAnchor()
    {
        weaponTrailAnchor = GameObject.Find("WeaponTrailAnchor");
    }

    void IsAnchorSet()
    {
        if (weaponTrailAnchor == null)
        {
            Debug.Log("Weapon trail anchor is not set");
        }
    }

    //-------------------------
    private void Update()
    {
        if (CheckIfSingleFireTrue())
        {
            if (Input.GetButtonDown("Fire1"))
            {
                ShootWeapon();
            }
        }
        else
        {
            if (Input.GetButton("Fire1") && CanShoot())
            {
                lastShotTime = SetTimeToShoot();
                ShootWeapon();
            }
        }
    }

    bool CheckIfSingleFireTrue()
    {
        if (isSingleFire == true)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void ShootWeapon()
    {
        float mouseX = GetMouseXPosition();
        float mouseY = GetMouseYPosition();
        Vector2 mousePosition = new Vector2(mouseX, mouseY);
        Vector2 firingPointPosition = new Vector2(weaponFiringPoint.position.x, weaponFiringPoint.position.y);
        RaycastHit2D hit = Physics2D.Raycast(firingPointPosition, (mousePosition - firingPointPosition)*100, 100f, whatLayersToHit);
        if (Time.time >= timeToSpawnEffect)
        {
            CreateWeaponTrailEffect();
            InstantiateMuzzleFlashAndSetParameters();
            timeToSpawnEffect = SetTimeToSpawnEffect();
        }
    }

    void CreateWeaponTrailEffect()
    {
        Transform weaponTrail = Instantiate(weaponTrailPrefab, weaponFiringPoint.position, weaponFiringPoint.rotation);
        SetWeaponTrailParent(weaponTrail);
    }

    void InstantiateMuzzleFlashAndSetParameters()
    {
        float randomMuzzleScale = GenerateRandomMuzzleFlashScale();
        Transform tempMuzzleFlashGO = muzzleFlashReference.InstantiateMuzzleFlash(randomMuzzleScale);
        SetMuzzleFlashParent(tempMuzzleFlashGO);
        Destroy(tempMuzzleFlashGO.gameObject, 0.05f);
    }

    //Helper functions
    float SetTimeToShoot()
    {
        float tempTime = Time.time + 1 / fireRate;
        return tempTime;
    }

    bool CanShoot()
    {
        if (Time.time > lastShotTime)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    float SetTimeToSpawnEffect()
    {
        float tempTimeToSpawn = Time.time + 1 / weaponTrailsPerSecond;
        return tempTimeToSpawn;
    }

    float GetMouseXPosition()
    {
        float tempX = Camera.main.ScreenToWorldPoint(Input.mousePosition).x;
        return tempX;
    }

    float GetMouseYPosition()
    {
        float tempY = Camera.main.ScreenToWorldPoint(Input.mousePosition).y;
        return tempY;
    }

    void SetWeaponTrailParent(Transform child)
    {
        child.SetParent(weaponTrailAnchor.transform);
    }

    void SetMuzzleFlashParent(Transform child)
    {
        child.SetParent(weaponFiringPoint.transform);
    }

    float GenerateRandomMuzzleFlashScale()
    {
        float tempScale = Random.Range(.6f, .9f);
        return tempScale;
    }
}
