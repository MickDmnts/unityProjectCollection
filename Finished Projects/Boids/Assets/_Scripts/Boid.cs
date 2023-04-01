using UnityEngine;

public class Boid : MonoBehaviour
{
    [Header("Set Dynamically")]
    public Rigidbody rigid;

    private Neighborhood neighborhood;

    private void Awake() //Executes First
    {
        neighborhood = GetComponent<Neighborhood>();
        rigid = GetComponent<Rigidbody>();

        Pos = Random.insideUnitSphere * Spawner.Singleton.spawnRadious; //Spawn point

        Vector3 vel = Random.onUnitSphere * Spawner.Singleton.velocity;
        rigid.velocity = vel; //Boid's velocity

        LookAhead();

        //Set a random color but not too black
        Color randColor = Color.black;
        while (randColor.r + randColor.g + randColor.b < 1.0f)
        {
            randColor = new Color(Random.value, Random.value, Random.value);
        }
        Renderer[] rends = gameObject.GetComponentsInChildren<Renderer>();
        foreach (Renderer r in rends)
        {
            r.material.color = randColor;
        }

        TrailRenderer trail = GetComponent<TrailRenderer>();
        trail.material.SetColor("_TintColor", randColor);
    }

    private void FixedUpdate()
    {
        Vector3 vel = rigid.velocity;
        Spawner spn = Spawner.Singleton;

        //COLLISION AVOIDANCE - Avoid neighbors who are too close
        Vector3 velAvoid = Vector3.zero;
        Vector3 tooClosePos = neighborhood.avgClosePos;
        if (tooClosePos != Vector3.zero)
        {
            velAvoid = Pos - tooClosePos;
            velAvoid.Normalize();
            velAvoid *= spn.velocity;
        }

        //VELOCITY MATCHING - Match velocity with other neighboors
        Vector3 velAlign = neighborhood.avgVel;
        if (velAlign != Vector3.zero)
        {
            velAlign.Normalize(); //Normalize the velocity
            velAlign *= spn.velocity; //And set it to the speed we chose
        }

        //FLOCK CENTERING - Move towards the center of the local neighbors
        Vector3 velCenter = neighborhood.avgPos;
        if (velCenter != Vector3.zero)
        {
            velCenter -= transform.position;
            velCenter.Normalize();
            velCenter *= spn.velocity;
        }

        //ATTRACTION - Move towards the attractor
        Vector3 offset = Attractor.POS - Pos; //Gets the distance from the Boid to the Attractor
        bool attracted = (offset.magnitude > spn.velocity); //If the Boids is going towards the Attractor
        Vector3 velAttract = offset.normalized * spn.velocity;


        //Apply all the velocities
        float fdt = Time.fixedDeltaTime;
        if (velAvoid != Vector3.zero)
        {
            vel = Vector3.Lerp(vel, velAvoid, spn.collAvoid * fdt);
        }
        else
        {
            if (velAlign != Vector3.zero)
            {
                vel = Vector3.Lerp(vel, velAlign, spn.VelMatching * fdt);
            }

            if (velCenter != Vector3.zero)
            {
                vel = Vector3.Lerp(vel, velCenter, spn.flockCentering * fdt);
            }

            if (velAttract != Vector3.zero)
            {
                if (attracted)
                {
                    vel = Vector3.Lerp(vel, velAttract, spn.attractPull * fdt);
                }
                else
                {
                    vel = Vector3.Lerp(vel, -velAvoid, spn.attractPush * fdt);
                }
            }
        }

        vel = vel.normalized * spn.velocity;
        rigid.velocity = vel;
        LookAhead();
    }

    void LookAhead()
    {
        transform.LookAt(Pos + rigid.velocity);
    }

    public Vector3 Pos //Shorthand for .GetComponent<Transform>().position;
    {
        get { return transform.position; }
        set { transform.position = value; }
    }
}
