using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>Handles the main game action</summary>
public class GameController : MonoBehaviour 
{
	public static GameController S; //Singleton

	[Header("Set in Inspector")]
	public float startDelay = 3f;
	public GameObject playerPrefab;
	public Vector3 playerSpawn;
    //Castle Positions Array\\
    public GameObject[] castlePositions;

    [Header("Set Dynamically")]
	public GameObject player; //The actual player after the instantiation

    //Private Vars\\
    [Header("Private Variables: Do not modify")]
	[SerializeField]int randomIndex;

	//MAIN SCRIPT\\
	void Awake()
	{
		S = this;
		randomIndex = Random.Range(0,castlePositions.Length); //Select a random index between 0-3 which repressent the castle positions 
		PositionActivator.S.ReAssignCurrentPositionOfThePlayer(randomIndex); //Tell each Position what position the player currently is heading.
		Invoke("FirstSpawn",startDelay); //Wait # of secs and then call FirstSpawn() to spawn the player
	}

	void FixedUpdate()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit();
		}
	}

	void FirstSpawn() //Used ONLY in the first spawn.. for now ;) 
	{
		player = Instantiate<GameObject>(playerPrefab) as GameObject;
        player.transform.position = playerSpawn;

		//From here on everything's handled from the Player class
		Player.S.TimeStart = Time.time;
		Player.S.p0 = this.transform.position;
		Player.S.p1 = castlePositions[randomIndex].transform.position;
		Player.S.spawnMove = true;
    }
}
