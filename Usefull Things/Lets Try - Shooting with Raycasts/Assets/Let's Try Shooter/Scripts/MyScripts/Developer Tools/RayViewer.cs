using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayViewer : MonoBehaviour
{
    public float weaponRange = 50f;
    private Camera fpsCam;

    private void Start()
    {
        fpsCam = GetComponentInParent<Camera>();
    }

    private void Update()
    {
        Vector3 rayOrigin = fpsCam.ViewportToWorldPoint(new Vector3(.5f, .5f));
        Debug.DrawRay(rayOrigin, fpsCam.transform.forward * weaponRange, Color.green);
    }
}
