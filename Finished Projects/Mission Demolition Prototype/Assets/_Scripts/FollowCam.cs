using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    public static GameObject POI; //The point of interest the camera needs to follow - aka the projectile

    [Header("Set in Inspector")]
    public float easing = 0.05f;
    public Vector2 minXY = Vector2.zero;

    [Header("Set Dynamically")]
    public float camZ;

    private void Awake()
    {
        camZ = transform.position.z;
    }

    private void FixedUpdate()
    {
        Vector3 destination;

        if (POI == null)
        {
            destination = Vector3.zero;
        }
        else
        {
            destination = POI.transform.position;
            if (POI.tag == "Projectile")
            {
                if (POI.GetComponent<Rigidbody>().IsSleeping())
                {
                    POI = null;
                    return;
                }
            }
        }

        destination.x = Mathf.Max(minXY.x, destination.x); //sets the x of the destination to the cameras x
        destination.y = Mathf.Max(minXY.y, destination.y); //sets the y of the destination to the cameras y
        destination.z = camZ; //sets the z of the destination to the camera Z

        destination = Vector3.Lerp(transform.position, destination, easing); //0 is the cameras pos, 1 if the destination
        transform.position = destination; //Sets the cameras pos to the destination

        Camera.main.orthographicSize = destination.y + 10;
    }
}
