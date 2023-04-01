using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstSpawn : MonoBehaviour 
{
	public static FirstSpawn S; //Singleton

	[Header("Set in Inspector")]
	public GameObject playerPrefab; //The player prefab -- dummy
	public Vector3 spawnPosition; //The spawn position of the dummy
	public Vector3 positionToGo; //The position we want the player to go
	[Tooltip("Speed of the animation")]
	public float speed; //speed of animation
    [Tooltip("When we want to stop the dummy-Player. [Default: 0.07f]")]
	public float completedDistance = 0.07f;

	[Header("Set Dynamically")]
	public GameObject player; //The actual dummy after instantiation

    //Private Vars\\
    [Header("Private Variables: Do not modify")]
    [SerializeField]bool executeMove; //When true, we start moving
	[SerializeField]float distance;
	[SerializeField]Vector3 p0;
	[SerializeField]Vector3 p1;

	//MAIN SCRIPT\\
	void Start()
	{
        player = Instantiate(playerPrefab);
        player.transform.position = spawnPosition;
        //_timeStart = Time.time;
        executeMove = true;
	}

	void Update()
	{
		if (executeMove) //Currently tne player is not instantiated as a GameObject
		{	
			//Execute the first animation
			//speed = (Time.time - _timeStart) / duration;
			distance = positionToGo.y - player.transform.position.y;
			if (distance >= -completedDistance)
			{
				completedDistance = -completedDistance;
				executeMove = false;
				Destroy(player.gameObject);
			}

			p0 = player.transform.position;
			p1 = positionToGo;

			Vector3 p01 = (1 - speed) * p0 + speed * p1;

			player.transform.position = p01;
		}
	}
}
