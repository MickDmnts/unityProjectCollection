using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour 
{
	public float bulletSpeed = 20f;

    //Private Vars\\
    [Header("Private Variables: Do not modify")]
	[SerializeField]Rigidbody projRb;
	[SerializeField]float camHeight;

	void Awake()
	{
		camHeight = Camera.main.orthographicSize;
	}

	void Start()
	{
		projRb = this.GetComponent<Rigidbody>();
		Fire();
	}

	void Update()
	{
		if (this.gameObject.transform.position.y > camHeight)
		{
			Destroy(this.gameObject,1f);
		}
	}

	void Fire()
	{
		Vector3 vel = Vector3.up * bulletSpeed;
		if(transform.up.y < 0)
		{
			vel.y = -vel.y;
		}
		this.projRb.velocity = vel;
	}
}
