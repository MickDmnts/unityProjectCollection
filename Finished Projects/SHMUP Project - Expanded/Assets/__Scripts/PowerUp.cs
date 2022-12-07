using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour 
{
	[Header("Set in Inspector")]
	public Vector2 rotMinMax = new Vector2(15,90);
	public Vector2 driftMinMax = new Vector2(.25f,2);
	public float lifeTime = 6f;
	public float fadeTime = 4f;

	[Header("Set Dynamically")]
	public WeaponType type;
	public GameObject cube; //Ref to the Cube child
	public TextMesh letter; //Ref to the TextMesh
	public Vector3 rotPerSecond; //Euler rotation speed
	public float birthTime;

	//Private Vars\\
	private Rigidbody rigid;
	private BoundsCheck bounds;
	private Renderer cubeRend;

	void Awake()
	{
		//Find the cube ref
		cube = transform.Find("Cube").gameObject;

		//Find the TextMesh and other components;
		letter = GetComponent<TextMesh>();
		rigid = GetComponent<Rigidbody>();
		bounds = GetComponent<BoundsCheck>();
		cubeRend = cube.GetComponent<Renderer>();

		//Set a random Velocity
		Vector3 vel = Random.onUnitSphere; //Get random XYZ Velocity
		vel.z = 0; //Set the Z to 0 to flatten it
		vel.Normalize(); //Normalizing a Vector3 makes it length 1m;
		vel *= Random.Range(driftMinMax.x,driftMinMax.y);
		rigid.velocity = vel;

		//Set the rotation of this gameObject to R[0,0,0];
		transform.rotation = Quaternion.identity; //Quaternion.identity is equal to no rotation;

		//Set up the rotPerSecond for the Cube
		rotPerSecond = new Vector3(Random.Range(rotMinMax.x,rotMinMax.y), //x
			Random.Range(rotMinMax.x,rotMinMax.y), //y
			Random.Range(rotMinMax.x,rotMinMax.y)); //z
		
		birthTime = Time.time;
	}

	void Update()
	{
		cube.transform.rotation = Quaternion.Euler(rotPerSecond * Time.time); //Manualy rotate the child cube - time_based

		//Fade out the powerUp over time
		float u = (Time.time - (birthTime + lifeTime)) / fadeTime;

		//if u >= 1 then destroy the GO
		if (u >= 1)
		{
			Destroy(this.gameObject);
			return;
		}

		//Use u to determine the alpa value of the Cube and the Letter
		if (u > 0)
		{
			Color c = cubeRend.material.color;
			c.a = 1f-u;
			cubeRend.material.color = c;

			//Fade the letter too, but not as much
			c = letter.color;
			c.a = 1f - (u * 0.5f); //use half the U
			letter.color = c;
		}

		if (!bounds.isOnScreen) //If the PowerUp is outside the play area, Destroy() it;
		{
			Destroy(gameObject);
		}
	}

	public void SetTypeOfPowerUp(WeaponType wt)
	{
		//Grab the Weapon Definition from Main
		WeaponDefinition definition = Main.GetWeaponDefinition(wt);

		//Set the color of the Cube child
		cubeRend.material.color = definition.color;
		letter.text = definition.letter; //Set the letter that must be shown
		type = wt; //Actually set the type;
	}

	public void AbsorbedBy(GameObject target)
	{
		//This function is called from the Hero class when the powerUp is collected;
		Destroy(this.gameObject);
	}
}
