using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] GameObject objectToFollow;

    private void Start()
    {
        objectToFollow = GameObject.FindGameObjectWithTag("Player");
    }

    private void FixedUpdate()
    {
        if (!objectToFollow)
            return;

        FollowObject();
    }

    private void FollowObject()
    {
        Camera.main.transform.position = objectToFollow.transform.position + new Vector3(0f, 0f, -10f);
    }
}
