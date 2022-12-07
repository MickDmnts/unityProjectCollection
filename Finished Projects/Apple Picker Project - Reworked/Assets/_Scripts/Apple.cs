using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apple : MonoBehaviour
{
    [Header("Set in inspector.")]
    public float bottomY = -10; //Default -10f

    void Update ()
    {
        if (this.transform.position.y < bottomY)
        {
            Destroy(this.gameObject);
        }
	}
}
