using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalReached : MonoBehaviour
{
    //static field, accesible by code anywhere
    static public bool goalReached = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Projectile")
        {
            goalReached = true;
            Material mat = GetComponent<Renderer>().material;
            Color c = mat.color;
            c.a = 0.90f;
            mat.color = c;
        }
    }
}
