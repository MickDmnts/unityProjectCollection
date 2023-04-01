using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour 
{
	public static Player S; //Singleton

	[Header("Set in Inspector")]
    [Range(0f, 5f)]
    public float durationSpeed = 1f;

	[Header("Set Dynamically")]
	public bool spawnMove = false;
	public bool positionMove = false;

    //Private Vars\\
    [Header("Private Variables: Do not modify")]
    [SerializeField]float u;
	[SerializeField]float _timeStart;

	//Properties\\
	public float TimeStart //The time we started moving -- Called from P.A. to tell the Player the CurrentTime he started moving... Used in interpolation
	{
		set
		{
			_timeStart = value;
		}
	}

	public Vector3 POS //The player's POS -- Used from the P.A. script to determine the p0 
	{
		get
		{
			return this.transform.position;
		}
	}

	Vector3 _p0;
	public Vector3 p0 //FROM position _p0 -- called from GC
	{
		set
		{
			_p0 = value;
		}
	}

	Vector3 _p1;
	public Vector3 p1 //TO position _p1 -- called from GC
	{
		set
		{
			_p1 = value;
		}
	}

	//MAIN SCRIPT\\
	void Awake()
	{
		S = this;
	}

	void Update()
	{
		if (spawnMove) //Called only once in the start of the game
		{
			u = (Time.time - _timeStart) / 1f;
			if (u >= 1)
			{
				u = 1;
				ActivatePositions(); //Call ActivatePositions() when the starting animation is finished so the game can start
				EnemySpawn.S.Spawn = true; //Start spawning enemies
				spawnMove = false; //notify the player that he can move..
				Shoot.S.CanShoot = true; //... and shoot
			}

			Vector3 p01 = (1 - u) * _p0 + u * _p1;
			transform.position = p01;
		}

		if (positionMove) //Called everytime we want to move the player
		{
			u = (Time.time - _timeStart) / durationSpeed;
			if (u >= 1)
			{
				u = 1;
				positionMove = false; //if true , then the player can move again..
				Shoot.S.CanShoot = true; //... and shoot
			}

			Vector3 p01 = (1 - u) * _p0 + u * _p1;
			transform.position = p01;
		}
	}

	///<summary>Called from the Update() function when the starting animation is finished.</summary>
	void ActivatePositions()
	{
		foreach (GameObject tempPos in GameController.S.castlePositions)
		{
			tempPos.GetComponent<PositionActivator>().ActivePos = true;
		}
	}
}