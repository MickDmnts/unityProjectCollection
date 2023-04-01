using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayRenderer : MonoBehaviour
{
    //Privates\\
    private Camera mainCam;

    private void Start()
    {
        mainCam = Camera.main;
    }

    private void Update()
    {
        RaycastHit hit;
        Ray camRay = mainCam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(camRay,out hit, 100f))
        {
            Debug.DrawRay(mainCam.transform.position, hit.point, Color.red);
            //Debug.Log(hit.collider.name);
        }
    }
}
