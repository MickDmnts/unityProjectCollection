using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour 
{
	static public EnemySpawn S; //Singleton

	[Header("Set in Inspector")]
	public Transform[] positionTransforms;
	public GameObject enemyPrefab;
	[Range(0f,5f)]
	public float enemySpawnDelay = 1f;

	[Header("Private Variables: Do not modify")]
	[SerializeField] int pickedPosition;
	//Private Vars\\
	GameObject enemyGO;
	[SerializeField] float camHeight;
	[SerializeField] Vector3 pickedPosTrans;
	[SerializeField] int lastPickedPos;
	[SerializeField] bool firstTimeSpawning;

	//Spawn is changed from the Player class to notify the EnemySpawn that the playerGO is in place and ready for attack
	[SerializeField] bool spawn;
	public bool Spawn
	{
		set
		{
			spawn = value;
		}
	}

	void Awake()
	{
		S = this;
		camHeight = Camera.main.orthographicSize + 3f;
		firstTimeSpawning = true;
		spawn = false;
	}

	void FixedUpdate()
	{
		if (spawn)
		{
            InvokeRepeating("SpawnEnemy", 1.5f, enemySpawnDelay);
			spawn = false;
		}
	}

	void SpawnEnemy()
	{
		//1. We need to randomize the spawn point (positionsTransforms) and Spawn one time to set a start point and 
		//THEN assign lastPickedPos to the first picked position

		//2. Randomize the spawn point again and check to see if it's the same as lastPickedPos
		//if YES: keep randomizing it until it's different from the last time and then instantiate
		//if NO: instantiate immediately
		if (firstTimeSpawning)
		{
			Vector3 enemyPositionToGo;
			pickedPosition = Random.Range(0,positionTransforms.Length);	
			pickedPosTrans =  positionTransforms[pickedPosition].transform.position;

            enemyPositionToGo = positionTransforms[pickedPosition].transform.position;
			pickedPosTrans.y = camHeight;

			enemyGO = Instantiate(enemyPrefab) as GameObject;
            enemyGO.transform.position = pickedPosTrans;
            this.enemyGO.GetComponent<Enemy>().P1 = enemyPositionToGo; //this sets the current enemy's p1 to the picked position transform

            firstTimeSpawning = false;
			lastPickedPos = pickedPosition;
			return;
		}

		if (!firstTimeSpawning)
		{
			Vector3 enemyPositionToGo;
			pickedPosition = Random.Range(0,positionTransforms.Length);
			if (pickedPosition == lastPickedPos)
			{
				do
				{
					pickedPosition = Random.Range(0,positionTransforms.Length);
				} while (pickedPosition == lastPickedPos);

                pickedPosTrans = positionTransforms[pickedPosition].transform.position;
				
                enemyPositionToGo = positionTransforms[pickedPosition].transform.position;
                pickedPosTrans.y = camHeight;

                enemyGO = Instantiate(enemyPrefab) as GameObject;
                enemyGO.transform.position = pickedPosTrans;
                this.enemyGO.GetComponent<Enemy>().P1 = enemyPositionToGo;  //this sets the current enemy's p1 to the picked position transform

                lastPickedPos = pickedPosition;

			}
			else
			{
                pickedPosTrans = positionTransforms[pickedPosition].transform.position;
				
                enemyPositionToGo = positionTransforms[pickedPosition].transform.position;
                pickedPosTrans.y = camHeight;

                enemyGO = Instantiate(enemyPrefab) as GameObject;
                enemyGO.transform.position = pickedPosTrans; 
                this.enemyGO.GetComponent<Enemy>().P1 = enemyPositionToGo;  //this sets the current enemy's p1 to the picked position transform

                lastPickedPos = pickedPosition;

			}
		}
	}
}
