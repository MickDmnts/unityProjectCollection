using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour 
{
	[Header("Set in Inspector")]
	[Range(1f,20f)]
	public float enemyMoveSpeed = 1f;
	public int enemyHealth = 5;
	public float showDamageDuration = 0.1f;

	[Header("Set Dynamically")]
	public Color[] originalColors;
	public Material[] materials;
	public bool showingDamage = false;
	public float damageDoneTime;

	//Private Vars\\
	[Header("Private Variables: Do not modify")]
	[SerializeField]bool canMove = false;
	[SerializeField]float timeStart;
	Vector3 p0;
	Vector3 p1;
	public Vector3 P1
	{
		set
		{
			p1 = value;
		}
	}

	void Awake()
	{
		materials = Utils.GetAllMaterials(this.gameObject);
		originalColors = new Color[materials.Length];
		for (int i = 0; i < materials.Length; i++)
		{
			originalColors[i] = materials[i].color;
		}
	}

	void Start()
	{
		if (enemyMoveSpeed <= 0)
		{
			Debug.Log("Enemy speed is 0");
			return;
		}
		if(canMove != true)
		{
            p0 = this.transform.position;
			timeStart = Time.time;
			canMove = true;
		}
	}

	void Update()
	{

		if (canMove)
		{
			float u = (Time.time - timeStart) / enemyMoveSpeed;
			if (u >= 1)
			{
				u = 1;
				canMove = false;
			}

			Vector3 p01 = (1 - u) * p0 + u * p1;

			this.transform.position = p01;
		}
		
		if (showingDamage && Time.time > damageDoneTime)
		{
			UnShowDamage();
		}
	}

	void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.tag == "Projectile")
		{
			if (enemyHealth <= 0)
			{
				Destroy(other.gameObject);
            	Destroy(this.gameObject);
				return;
			}

			Destroy(other.gameObject);
			ShowDamage();
			enemyHealth -= 1;
		}
	}


	//==========================Custom Methods=============================\\
	void ShowDamage()
	{
		foreach (Material m in materials)
		{
			m.color = Color.red;
		}
		showingDamage = true;
		damageDoneTime = Time.time + showDamageDuration;
	}

	void UnShowDamage()
	{
		for (int i = 0; i < materials.Length; i++)
		{
			materials[i].color = originalColors[i];
		}
		showingDamage = false;
	}
}
