using System;
using UnityEngine;

/// <summary>
/// This script is designed to be re-usable.
/// Works ONLY for Orthographic cameras at [0f,0f,0f]
/// </summary>

public class CameraBoundsChecker :MonoBehaviour
{
    [Header("Set in Inspector")]
    public Camera cameraToSet;
    public float radius = 1.0f; //Distance we want to keep the player away from the bounds

    [Header("Set Dynamically")]
    public float camHeight; //The camera Height (must be *2 to be correct)
    public float camWidth; //The camera Width (must be *2 to be correct)

    private void Awake()
    {
        camHeight = cameraToSet.orthographicSize; //Gets the camera Height
        camWidth = camHeight * cameraToSet.aspect; //By multiplying the cameraHeight with its aspect you get the cameraWidth
    }

    private void LateUpdate()
    {
        Vector3 pos = transform.position; //Stores the current Ship position

        //X-Axis
        if (pos.x > camWidth - radius) //If the pos is greater than (the width of the camera-its radius) then block further moving -> Right
        {
            pos.x = camWidth - radius; //set it equal to the width of the camera minus the radius 
        }

        if (pos.x < -camWidth + radius) //Same as above but with reversed numbers -> Left
        {
            pos.x = -camWidth + radius;//set it equal to the (-width of the camera + the radius)
        }

        //Y-Axis
        if (pos.y > camHeight - radius) // -> Up
        {
            pos.y = camHeight - radius;
        }

        if (pos.y < -camHeight + radius) // -> Down
        {
            pos.y = -camHeight + radius;
        }

        transform.position = pos; //Set the ships position to the current modified pos variable
    }

    private void OnDrawGizmos() //Draws on the Scene View
    {
        if (!Application.isPlaying) //If NOT on playMode, don't draw gizmos
        {
            return;
        }
        Vector3 boundSize = new Vector3(camWidth * 2, camHeight * 2, 0.1f); //Multiply camWidth and camHeight * 2 and set the thickness of the bounds to 0.1f
        Gizmos.DrawWireCube(Vector3.zero, boundSize); //Draw a cube with the above numbers, on [0f,0f,0f]
    }
}