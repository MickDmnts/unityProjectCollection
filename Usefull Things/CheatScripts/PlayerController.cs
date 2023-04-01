using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController1 : MonoBehaviour
{
    CharacterController charCtrl;
    Transform mainCam;

    [SerializeField] float characterHeight = 1.7f;
    //Keyboard Input and Movement

    float inputVer = 0;
    float inputHor = 0;

    Vector3 direction;
    Vector3 movement;

    //Mouse Input
    
    float mouseInputX = 0;
    float mouseInputY = 0;
    float mouseXRotate = 0;
    [SerializeField] float mouseSensitivity = 500f;


    [Header("Gravity")]

    [SerializeField] float gravity = 10;


    [Header("Move Stats")]

    [SerializeField] float playerSpeed = 10f;
    [SerializeField] float speedIncreaseRate = 5;
    [SerializeField] float speedDecreaseRate = 5;
    [SerializeField] float sprint = 1;
    [SerializeField] float maxSprint = 4;

    [Header("Jump Stats")]

    [SerializeField] float jumpSpeed;
    [SerializeField] float jumpDuration = 0.3f;
    [SerializeField] float jumpTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        charCtrl = GetComponent<CharacterController>();
        mainCam = Camera.main.transform;
        Cursor.lockState = CursorLockMode.Locked;

        mainCam.position = transform.position + Vector3.up * characterHeight;
        mainCam.parent = transform;
    }

    // Update is called once per frame
    void Update()
    {
        //Getting Input on every frame

        inputHor = Input.GetAxis("Horizontal");
        inputVer = Input.GetAxis("Vertical");
        mouseInputY = Input.GetAxis("Mouse Y");
        mouseInputX = Input.GetAxis("Mouse X");

        //Rotation

        transform.Rotate(0, mouseInputX * mouseSensitivity*Time.deltaTime, 0);

        //Sprint
        if (Input.GetButton("Sprint"))
        {
            if (sprint < maxSprint)
            {
                sprint += speedIncreaseRate * Time.deltaTime;
            }
        }
        else
        {
            if (sprint > 1)
            {
                sprint -= speedDecreaseRate * Time.deltaTime;
            }
        }




        //Counstruct Direction Vector
        direction = transform.forward * inputVer + transform.right * inputHor;
        direction *= playerSpeed * sprint;

        if (charCtrl.isGrounded)
        {
            if (Input.GetButtonDown("Jump"))
            {

                jumpTime += jumpDuration;
            }
            if (jumpTime > 0)
            {
                jumpTime -= Time.deltaTime;
                direction.y += jumpSpeed;
            }
            else
            {
                direction.y = -gravity;
            }
        }


        movement = direction * Time.deltaTime;  

        charCtrl.Move(movement);
    }

    private void LateUpdate()
    {
        mouseXRotate += mouseInputY * mouseSensitivity * Time.deltaTime;
        mouseXRotate = Mathf.Clamp(mouseXRotate, -75, 85);
        mainCam.localRotation = Quaternion.Euler(mouseXRotate, 0, 0);
    }
}
