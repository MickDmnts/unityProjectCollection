using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    static public GameObject POI;

    [Header("Set in inspector")]
    public float cameraEasing = 0.05f;
    public Vector2 minXY = Vector2.zero;

    [Header("Set dynamically")]
    public float camZ;

    private void Awake()
    {
        InitializeCameraZPosition();
    }

    void InitializeCameraZPosition()
    {
        camZ = this.transform.position.z;
    }

    private void FixedUpdate()
    {
        if (POI == null)
        {
            return;
        }

        AdjustCameraOrthographicSize(FollowPOI());
    }

    Vector3 FollowPOI()
    {
        Vector3 destination = POI.transform.position;
        destination.x = Mathf.Max(minXY.x, destination.x);
        destination.y = Mathf.Max(minXY.y, destination.y);
        destination = Vector3.Lerp(transform.position, destination, cameraEasing);
        destination.z = camZ;
        transform.position = destination;
        return destination;
    }

    void AdjustCameraOrthographicSize(Vector3 poiPosition)
    {
        Camera.main.orthographicSize = poiPosition.y + 10;
    }
}
