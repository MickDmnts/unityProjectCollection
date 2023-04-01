using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmRotationManager : MonoBehaviour
{
    private void Update()
    {
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;  //Mouse position - current player position
        difference.Normalize();

        float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg; //Rad2Dig converts from Radiants to Degrees
        transform.rotation = Quaternion.Euler(0f, 0f, rotationZ);
    }
}
