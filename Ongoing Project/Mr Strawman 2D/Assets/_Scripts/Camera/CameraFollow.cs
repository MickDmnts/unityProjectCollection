using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Set in Inspector")]
    public float cameraZDistance = -10f;

    [Range(0.01f, 1f)]
    public float cameraOffsetX = 1f;

    [Range(0.01f, 1f)]
    public float cameraOffsetY = 1f;

    [Range(0.00001f, 1f)]
    public float lerpTime = .1f;

    Transform objFollow;

    private void Start()
    {
        objFollow = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Vector3 mousePos;
    // Vector3 objPos;
    // Vector3 cameraPos;
    // Vector3 halfDistance;
    // Vector3 p0, p1, p01;

    Vector3 mousePos, objPos, cameraPos, halfDistance, p0, p1, p01, cameraFinalPos;
    float xDistance, finalXDistance, yDistance, finalYDistance;

    private void LateUpdate()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        objPos = objFollow.position;
        p0 = transform.position;

        xDistance = (objPos.x + mousePos.x) / 2;
        finalXDistance = Mathf.Clamp(xDistance, objPos.x - cameraOffsetX, objPos.x + cameraOffsetX);

        yDistance = (objPos.y + mousePos.y) / 2;
        finalYDistance = Mathf.Clamp(yDistance, objPos.y - cameraOffsetY, objPos.y + cameraOffsetY);

        cameraFinalPos = new Vector3(finalXDistance, finalYDistance, cameraZDistance);

        p1 = cameraFinalPos;

        p01 = (1 - lerpTime) * p0 + lerpTime * p1;

        transform.position = p01;
    }
}