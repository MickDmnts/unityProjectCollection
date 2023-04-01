using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour 
{
	public static Shoot S; //Singleton

	public GameObject weapon; //The firing point
	public GameObject bulletPrefab; //The projectilePrefab
	public float fireRate = 0.1f; //Fire rate for each weapon

	//Private Vars\\
	[Header("Private Variables: Do not modify")]
	[SerializeField]float lastShotTime; //Controls the shot time
	[SerializeField]bool canShoot = false;

	public bool CanShoot
	{
		set
		{
			canShoot = value;
		}
	}

	void Awake()
	{
		S = this;
		if (canShoot != false)
		{
			canShoot = false;
		}
	}

	void Start()
	{
		lastShotTime = 0;
	}

	void Update()
	{
		if (Input.GetKey(KeyCode.Space) && canShoot)
		{
			SpawnAndFire();
		}
	}

	void SpawnAndFire()
	{
		if (Time.time - lastShotTime < fireRate)
		{
			return;
		}

        GameObject bullet = Instantiate<GameObject>(bulletPrefab);
        bullet.transform.position = weapon.transform.position;
		lastShotTime = Time.time;
	}
}
