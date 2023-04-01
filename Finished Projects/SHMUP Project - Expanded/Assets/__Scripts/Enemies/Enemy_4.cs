using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///Part is another serializable data storage class just like WeaponDefinition
///</summary>
[System.Serializable]
public class Part
{
	//Need to be defined in the inspector pane
	public string name; //Name of the part
	public float health; //Health of this part
	public string[] protectedBy; //The parts that protect this

	//These two fields are set automatically in Start()
	//Caching them makes it easier and faster
	[HideInInspector]
	public GameObject go; //The part GO
	[HideInInspector]
	public Material mat; //Material to show the damage
}

//===================================== MAIN ===========================================\\
public class Enemy_4 : Enemy 
{
	[Header("Set in inspector: Enemy_4")]
	public Part[] parts;

	private Vector3 p0,p1;
	private float timeStart;
	private float duration = 4; //Duration of movement

	void Start()
	{
		p0 = p1 = pos;
		InitMovement();

		//Cache material and GO of each part in Parts
		Transform t;
		foreach (Part prt in parts)
		{
			t = transform.Find(prt.name);
			if (t != null)
			{
				prt.go = t.gameObject;
				prt.mat = prt.go.GetComponent<Renderer>().material;
			}
		}
	}

	void InitMovement() //Re-Initialize the position
	{
		p0 = p1;
		//Assign a new on-screen lcoation to p1
		float widMinRad = bndCheck.camWidth - bndCheck.radius; //Width
		float hgtMinRad = bndCheck.camHeight - bndCheck.radius; //Height

		p1.x = Random.Range(-widMinRad,widMinRad);
		p1.y = Random.Range(-hgtMinRad,hgtMinRad);

		//Reset the time
		timeStart = Time.time;
	}

	public override void Move()
	{
		//This completely overrides Enemy.Move() with linear interpolation
		float u  = (Time.time - timeStart) / duration;

		if (u >= 1)
		{
			InitMovement();
			u = 0;
		}

		u = 1 - Mathf.Pow(1-u,2); //Apply a small easing to the movement
		pos = (1-u) * p0+u*p1;
	}

	//These two functions find a Part in parts based on name or gameObject
	Part FindPart(string n)
	{
		foreach (Part prt in parts)
		{
			if (prt.name == n)
			{
				return prt;
			}
		}
		return null;
	}

	Part FindPart( GameObject go)
	{
		foreach (Part prt in parts)
		{
			if (prt.go == go)
			{
				return prt;
			}
		}
		return null;
	}

	//These functions return true if the part has been destroyed
	bool Destroyed(GameObject go)
	{
		return Destroyed(FindPart(go));
	}

	bool Destroyed(string n)
	{
		return Destroyed(FindPart(n));
	}

	bool Destroyed(Part prt)
	{
		if (prt == null) //If yes then the part was destroyed
		{
			return true;
		}
		return prt.health <= 0; //If prt.health <= 0 then the part was destroyed
	}

	void ShowLocalizedDamage(Material m)
	{
		m.color = Color.red;
		damageDoneTime = Time.time + showDamageDuration;
		showingDamage = true;
	}

	//The following function overrides the Enemy.OnCollisionEnter to work for each part individualy
	void OnCollisionEnter(Collision coll)
	{
		GameObject other = coll.gameObject;
		switch (other.tag)
		{
			case "ProjectileHero":
				Projectile p = other.GetComponent<Projectile>();
				if (bndCheck.isOnScreen == false)//If the enemy is offScreen don't damage it
				{
					Destroy(other);
					break;
				}

				//Hurt this enemy
				GameObject goHit = coll.contacts[0].thisCollider.gameObject;
				Part prtHit = FindPart(goHit);
				if (prtHit == null) //If prtHit wasn't found...
				{
					goHit = coll.contacts[0].otherCollider.gameObject;
					prtHit = FindPart(goHit);
				}

				//Check whether this part is still protected
				if (prtHit.protectedBy != null)
				{
					foreach (string s in prtHit.protectedBy)
					{
						//If one of the protected parts hasn't been destroyed...
						if (!Destroyed(s))
						{
							//... then don't damage this part yet
							Destroy(other);
							return; //return before damaging the enemy
						}
					}
				}

				//Get the damageAmount from the Projectile.type & Main.W_DEFS
				prtHit.health -= Main.GetWeaponDefinition(p.wType).damageOnHit;
				//Show damage on the part
				ShowLocalizedDamage(prtHit.mat);
				if (prtHit.health <= 0)
				{
					prtHit.go.SetActive(false);	//Disable the part
				}

				//Check to see if the whole ship is destroyed
				bool allDestroyed = true; //Assume it is destroyed
				foreach (Part prt in parts)
				{
					if (!Destroyed(prt)) //If there is still an active part
					{
						allDestroyed = false; //set allDestroyed to false;
						break; //Break out from the foreach loop
					}
				}

				if (allDestroyed)
				{
					Main.S.ShipDestroyed(this);
					Destroy(this.gameObject);
				}
				Destroy(other); //Destroy the projectile
				break;
		}
	}
}
