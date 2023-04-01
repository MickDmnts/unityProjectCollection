using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundCubes : MonoBehaviour
{
    //Main Inits
    [Header("Main configs")]
    public GameObject cubePrefab;
    public GameObject backgroundAnchor;

    //Cubes List
    [Header("The cubes list")]
    public int numberOfCubes = 10;
    public List<GameObject> cubesList;

    //Spawn
    [Header("The spawning")]
    public Vector3 randomizedSpawn;
    public bool canSpawn;
    public int cubesSpawned = 0;

    //Min and Max spawn points
    [Header("Please set min and max spawn points for the cubes")]
    public float xMin = 100f; //100 default
    public float xMax = 200f; //200 default
    //-----------------------
    public float yMin = 5f; //5 default
    public float yMax = 35f; //25 default
    //-----------------------
    public float zMin = 45f; //45 default
    public float zMax = 90f; //90 default

    //Move Lerping
    [Header("Lerping area")]
    public bool canLerp;
    public float distance = 100f;
    public float lerpForce = 0.003f;
    public Vector3 spawnPoint;
    public int unitsSpawned = 0;

    //----------------------------------------------------------------
    private void Awake()
    {       
        randomizedSpawn = new Vector3();
        cubesList = new List<GameObject>();
    }

    private void Start()
    {
        canSpawn = true;
        canLerp = false;
    }

    private void FixedUpdate()
    {
        if (canSpawn) //If canSpawn == true the continue on with cube spawning
        {
            for (int i = 0; i < numberOfCubes; i++)
            {
                randomizedSpawn.Set(Random.Range(xMin,xMax),Random.Range(yMin,yMax),Random.Range(zMin,zMax));
                SpawnCubes();
            }
            canSpawn = false;
            SetLerp(true);
        }

        if (canLerp)
        {
            StartCoroutine(Lerping());
        }
    }

    void SetLerp(bool lerpState)
    {
        canLerp = lerpState;
    }

    /// <summary>
    /// Called from the ---> for loop in FixedUpdate() to spawn cubes in random positions
    /// </summary>
    void  SpawnCubes()
    {
        cubesSpawned = cubesSpawned + 1;
        Vector3 cubeSpawn = randomizedSpawn;
        GameObject cube = Instantiate(cubePrefab, backgroundAnchor.transform) as GameObject;
        cube.transform.localPosition = cubeSpawn;
        cube.name = "Cube #" + cubesSpawned;
        cubesList.Add(cube);
    }

    /// <summary>
    /// Runs all the time to move the cubes
    /// </summary>
    IEnumerator Lerping()
    {
        foreach (GameObject cube in cubesList)
        {
            Vector3 startPos = cube.transform.position;
            Vector3 endPos = cube.transform.position + Vector3.right * -distance;  
            //--------------------------------------------------------
            if (cube.transform.position.z  < zMax / 2f)
            {
                float newLerpForce = lerpForce / 3f;
                cube.transform.position = Vector3.Lerp(startPos, endPos, newLerpForce);
            }
            else
            {
                cube.transform.position = Vector3.Lerp(startPos, endPos, lerpForce);
            }
            //----------------------------------------------------------
            if (cube.transform.position.x <= -100f)
            {
                unitsSpawned++;
                float pos = startPos.x + spawnPoint.x;
                Vector3 backToSpawn = new Vector3(pos, startPos.y, startPos.z);
                cube.transform.position = backToSpawn;
                switch (unitsSpawned)
                {
                    case 5:
                        canSpawn = true;
                        break;
                    case 10:
                        canSpawn = true;
                        break;
                    case 15:
                        canSpawn = true;
                        break;
                }
            }
        }
        yield return null;
    }
}
