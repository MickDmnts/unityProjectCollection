using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Set in Inspector")]
    public Camera cameraToSet;
    public float radius = 1.0f; //Distance we want to keep the player away from the bounds
    public GameObject heartPrefab;
    public float spawnRate = 1f;

    [Header("Set Dynamically")]
    public float camHeight; //The camera Height (must be *2 to be correct)
    public float camWidth; //The camera Width (must be *2 to be correct)
    public float nextTimeToSpawn = 0f;

    private void Awake()
    {
        camHeight = cameraToSet.orthographicSize; //Gets the camera Height
        camWidth = camHeight * cameraToSet.aspect; //By multiplying the cameraHeight with its aspect you get the cameraWidth
    }

    private void FixedUpdate()
    {
        if (Time.time >= nextTimeToSpawn)
        {
            float aboveScreenLocation = camHeight + radius;
            float XPos = Random.Range(-camWidth, camWidth);
            Vector3 newPos = new Vector3(XPos, aboveScreenLocation);
            GameObject prefab = Instantiate(heartPrefab);
            prefab.transform.position = newPos;
            nextTimeToSpawn = Time.time + 1 / spawnRate;
        }

    }
}
