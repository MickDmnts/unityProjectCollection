using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;

    [Header("Set Dynamically")]
    public Rigidbody2D rb;
    public Camera mainCamera;


    Vector2 movement;
    Vector2 mousePos;

    private void Awake()
    {
        AssignCameraAndRigidBodyRefs();
    }       
    
    void AssignCameraAndRigidBodyRefs()
    {
        rb = this.gameObject.GetComponent<Rigidbody2D>();
        mainCamera = GameObject.Find("_MainCamera").GetComponent<Camera>();
    }

    private void Start()
    {
        if (rb == null)
        {
            Debug.Log("PlayerMovement: Ln25 -- rb is not set");
        }

        if (mainCamera == null)
        {
            Debug.Log("PlayerMovement: Ln30 -- mainCamera is not set");
        }
    }

    private void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
    }

    private void FixedUpdate()
    {
        if(rb != null)
        {
            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);

            Vector2 lookDir = mousePos - rb.position;
            float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
            rb.rotation = angle;
        }
        
    }
}
