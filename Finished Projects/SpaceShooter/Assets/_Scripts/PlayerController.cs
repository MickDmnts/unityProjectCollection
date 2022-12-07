using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Boundaries
{
	public float xMin, xMax, zMin, zMax;
}

public class PlayerController : MonoBehaviour
{
	[Range(0f,12f)] //Speed of spaceship
	public float speed;
	[Range(0f,5f)] //Tilt on x -axis
	public float tiltX;

	public Boundaries boundaries; //game Bounds

	public GameObject shot; //Bolt
	public Transform shotSpawn; //Bolt
	public float fireRate; //Bolt

	private float nextFire = 0.5f; //Bolt
	private GameObject newShot; //Bolt
	private Rigidbody rb;

	void Start() //Executes at the first frame
	{
		rb = GetComponent<Rigidbody> ();
	}

	void Update() //Executes every frame
	{
		if (Input.GetButton ("Jump") && Time.time > nextFire) 
		{
			nextFire = Time.time + fireRate;
			Instantiate (shot, transform.position, transform.rotation); //Shot instantiation
            GetComponent<AudioSource>().Play();
		}
	}

	void FixedUpdate() //Executes every physics move
	{
		float hor = Input.GetAxis ("Horizontal");
		float ver = Input.GetAxis ("Vertical");
		Vector3 movement = new Vector3 (hor, 0.0f, ver);
		rb.velocity = movement * speed;

		rb.position = new Vector3 (Mathf.Clamp (rb.transform.position.x, boundaries.xMin, boundaries.xMax), 0.0f, Mathf.Clamp (rb.transform.position.z, boundaries.zMin, boundaries.zMax));
		rb.rotation = Quaternion.Euler (0.0f, 0.0f, rb.velocity.x * -tiltX);
	}
}
