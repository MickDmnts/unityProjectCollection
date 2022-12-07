using System.Collections.Generic;
using UnityEngine;

public class Neighborhood : MonoBehaviour
{
    [Header("Set Dynamically")]
    public List<Boid> neighbors; //List of all the neighbor Boids of this Boid
    private SphereCollider coll; //The Sphere Colldier attached to this Boid

    private void Start()
    {
        neighbors = new List<Boid>(); //Init the List of NeighborBoids
        coll = GetComponent<SphereCollider>(); //Get the Collider
        coll.radius = Spawner.Singleton.neighboorDist / 2; //The radius of the sphere measured in the object's local space divided by 2
    }

    private void FixedUpdate()
    {
        if (coll.radius != Spawner.Singleton.neighboorDist / 2) //If the radius hasn't change, change it
        {
            coll.radius = Spawner.Singleton.neighboorDist / 2;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Boid b = other.GetComponent<Boid>();
        if (b != null)
        {
            if (neighbors.IndexOf(b) == -1)
            {
                neighbors.Add(b); //If the Boid that just collided with isn't in the list, Add it
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Boid b = other.GetComponent<Boid>();
        if (b != null)
        {
            if (neighbors.IndexOf(b) != -1 )
            {
                neighbors.Remove(b); //If the Boid that just collided with is in the list and out of range, Remove it
            }
        }
    }

    public Vector3 avgPos
    {
        get
        {
            Vector3 avg = Vector3.zero;
            if (neighbors.Count == 0)
            {
                return avg; //Return if there are no Boids around
            }

            for (int i = 0; i < neighbors.Count; i++)
            {
                avg += neighbors[i].Pos; //Get the average position of all the Boids
            }
            avg /= neighbors.Count; //Doesn't erase the previous sum with /=
            return avg;
        }
    }

    public Vector3 avgVel
    {
        get
        {
            Vector3 avg = Vector3.zero;
            if (neighbors.Count == 0)
            {
                return avg; //If there are no Boids, return
            }

            for (int i = 0; i < neighbors.Count; i++)
            {
                avg += neighbors[i].rigid.velocity; //Get the average Velocity of all the Boids
            }
            avg /= neighbors.Count; //Doesn't erase the previous sum with /=
            return avg;
        }
    }

    public Vector3 avgClosePos //the average position of the Boids closer to this
    {
        get
        {
            Vector3 avg = Vector3.zero; //set avg to [0,0,0] by default
            Vector3 delta; //delta refers to what changed

            int nearCount = 0; //sum of all the Bodis around
            for (int i = 0; i < neighbors.Count; i++)
            {
                delta = neighbors[i].Pos - transform.position; //All the boids.pos in the list minus THIS boid's pos
                if (delta.magnitude <= Spawner.Singleton.collDist) //magnitude = length
                {
                    avg += neighbors[i].Pos;
                    nearCount++;
                }
            }
            if (nearCount == 0)
            {
                return avg; //if no boids around return [0,0,0]
            }

            avg /= nearCount;
            return avg;
        }
    }
}
