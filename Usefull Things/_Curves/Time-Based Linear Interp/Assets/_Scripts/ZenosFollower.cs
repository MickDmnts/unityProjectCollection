using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZenosFollower : MonoBehaviour
{
    [Header("Set in inspector")]
    public GameObject poi; //Point of Interest
    public float u = .1f;
    public Vector3 p0, p1, p01;

    private void FixedUpdate()
    {
        p0 = this.transform.position;
        p1 = poi.transform.position;

        ///<summary> Use this code to move the sphere with the mouse
        Vector3 mousePos2D = Input.mousePosition;
        mousePos2D.z = -Camera.main.transform.position.z;
        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);
        p1 = mousePos3D;


        //Interpolate
        p01 = (1 - u) * p0 + u * p1;

        this.transform.position = p01;
    }
}
