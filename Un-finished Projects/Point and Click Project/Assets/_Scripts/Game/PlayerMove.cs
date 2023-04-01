using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PlayerMove : MonoBehaviour
{
    //Publics\\
    [Header("Set in inspector")]
    public float u = 0.1f; //used as move speed

    //Privates\\
    private GameObject player;
    private Camera playerCam;

    private void Start()
    {
        player = this.gameObject;
        playerCam = Camera.main;

        if (u <= 0f)
        {
            EditorApplication.isPlaying = false;
            Debug.LogError("Player move speed is invalid.");
        }

        if (player == null)
        {
            EditorApplication.isPlaying = false;
            Debug.LogError("player GO could not be set.");
        }

        if (playerCam == null)
        {
            EditorApplication.isPlaying = false;
            Debug.LogError("Camera is not set.");
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            Ray camRay = playerCam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(camRay,out hit, 50f))
            {
                MovePlayer(hit.point);
            }
        }
    }

    void MovePlayer(Vector3 point)
    {
        Vector3 p0 = this.transform.position;
        Vector3 p1 = point;
        float u = 
    }
}
