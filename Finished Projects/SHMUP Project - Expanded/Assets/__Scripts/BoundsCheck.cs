using UnityEngine;

/// <summary>
/// Checks whether a GO is on screen and can force it to stay on screen.
/// Works for an Orthographic camera at [0,0,0].
/// </summary>
public class BoundsCheck : MonoBehaviour
{
    [Header("Set in Inspector")]
    public float radius = 1f; //Distance we want to keep the player away from the bounds
    public bool keepOnScreen = true;

    [Header("Set Dynamically")]
    public bool isOnScreen = true;
    public float camWidth;
    public float camHeight;

    [HideInInspector]
    public bool offRight, offLeft, offUp, offDown;

    private void Awake()
    {
        camHeight = Camera.main.orthographicSize; //Gets the camera Height
        camWidth = camHeight * Camera.main.aspect; //By multiplying the cameraHeight with its aspect you get the cameraWidth
    }

    /// <summary>
    /// This is where the bounds are getting checked
    /// </summary>
    private void LateUpdate()
    {
        Vector3 pos = transform.position; //Stores the current Ship position
        isOnScreen = true;
        offRight = offDown = offLeft = offUp = false;

        //X-Axis
        if (pos.x > camWidth - radius) //If the pos is greater than (the width of the camera-its radius) then block further moving -> Right
        {
            pos.x = camWidth - radius; //set it equal to the width of the camera minus the radius 
            offRight = true;
        }

        if (pos.x < -camWidth + radius) //Same as above but with reversed numbers -> Left
        {
            pos.x = -camWidth + radius;//set it equal to the (-width of the camera + the radius)
            offLeft = true;
        }

        //Y-Axis
        if (pos.y > camHeight - radius) // -> Up
        {
            pos.y = camHeight - radius;
            offUp = true;
        }

        if (pos.y < -camHeight + radius) // -> Down
        {
            pos.y = -camHeight + radius;
            offDown = true;
        }

        isOnScreen = !(offDown || offLeft || offRight || offUp); //If one of them is true then isOnScreen is set to false
        if (keepOnScreen && !isOnScreen)
        {
            transform.position = pos;
            isOnScreen = true;
            offDown = offLeft = offRight = offUp = false;
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
