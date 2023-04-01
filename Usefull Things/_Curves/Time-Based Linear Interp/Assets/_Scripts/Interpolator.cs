using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// TIME-BASED INTERPOLATION
/// </summary>
public class Interpolator : MonoBehaviour
{
    [Header("Set in inspector")]
    public Vector3 p0 = new Vector3(0, 0, 0);
    public Vector3 p1 = new Vector3(3, 4, 5);
    public float timeDuration = 1;

    [Header("Set dynamically")]
    public Vector3 p01;
    public bool moving;
    public float timeStart;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            moving = true;
            timeStart = Time.time;
            TrailRenderer tr = this.gameObject.GetComponent<TrailRenderer>();
            tr.Clear();
        }

        if (moving)
        {
            float u = (Time.time - timeStart) / timeDuration;
            if (u >= 1)
            {
                u = 1;
                moving = false;
            }

            //Stantard interpolation formula
            p01 = (1 - u) * p0 + u * p1;

            transform.position = p01;
        }
    }
}
