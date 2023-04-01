using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    static public Spawner Singleton; //Singleton - static
    static public List<Boid> boids; //All the boids spawned

    [Header("Set in Inspector: Spawning")]
    public GameObject boidPrefab; //Boid Prefab
    public Transform boidAnchor; //The BoidAnchor
    public int numBoids = 100; //Number of Boids to spawn
    public float spawnRadious = 100f; //Maximum space to spawn the Boids
    public float spawnDelay = 0.1f; //Delay between each spawn

    [Header("Set in Inspector: Boids")]
    public float velocity = 30f; //The Boids speed
    public float neighboorDist = 30f; //Max distance between the Boids
    public float collDist = 4f; //Used in the Neighbohood script - length
    public float VelMatching = 0.25f;
    public float flockCentering = 0.2f;
    public float collAvoid = 2f;
    public float attractPull = 2f; //
    public float attractPush = 2f;
    public float attractPushDist = 5f;

    private void Awake()
    {
        Singleton = this; //Sets the Singleton S to this instance of the BoidSpawner
        boids = new List<Boid>(); //Init the lsit of the Boids in the scene
        InstantiateBoid();
    }

    public void InstantiateBoid()
    {
        GameObject go = Instantiate(boidPrefab); //Instantiate the boid

        Boid b = go.GetComponent<Boid>();
        b.transform.SetParent(boidAnchor); //Put the boids under the BoidAnchor obj

        boids.Add(b); //Add the Boid in the boids list

        if (boids.Count < numBoids)
        {
            Invoke("InstantiateBoid", spawnDelay); //Repeat if the limit is not reached
        }
    }
}
