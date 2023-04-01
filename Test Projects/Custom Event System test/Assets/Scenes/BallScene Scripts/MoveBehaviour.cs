using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBehaviour : MonoBehaviour
{
    public float lerpTime = .1f;

    Vector3 p0, p1, p01;

    void Start()
    {
        p1 = new Vector3(2f, 0f, 0f);
    }

    private void FixedUpdate()
    {
        p0 = this.transform.position;

        p01 = (1-lerpTime) * p0 + lerpTime * p1;

        this.transform.position = p01;
    }
}
