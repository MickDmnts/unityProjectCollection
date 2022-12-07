using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class MutantController : MonoBehaviour
{
    [SerializeField] CinemachineFreeLook mycamera;
    CharacterController thisCharacterController;
    Animator thisAnimator;
    float vertical;
    float horizontal;

    [SerializeField] float characterRotationSpeed = 10;

    float angle;
    public float speed = 2;
    float jumpTimer = 0;
    public float jumpSpeed = 8;
    public float gravity = 6;
    public float jumpDuration = 0.25f;
    float gravityTemp = 0;
    float sprint = 1;

    bool isGrounded = false;

    Transform thisCamera;
    Vector3 direction;
    Vector3 movedirection;
    Quaternion rotation;

    // Start is called before the first frame update
    void Start()
    {
        thisAnimator = GetComponentInChildren<Animator>();
        thisCharacterController = GetComponent<CharacterController>();
        thisCamera = Camera.main.transform;
        mycamera = FindObjectOfType<CinemachineFreeLook>();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        // Get inputs and store
        vertical = Input.GetAxis("Vertical");
        horizontal = Input.GetAxis("Horizontal");

        // If input get Sprint
        if (Input.GetKey(KeyCode.LeftShift))
        {
            // Accelerate
            if (sprint < 2) sprint += 6 * Time.deltaTime;
        }
        else
        {
            // Decelerate
            if (sprint > 1) sprint -= 4 * Time.deltaTime;
        }


        //thisAnimator.SetFloat("Horizontal", horizontal * sprint * dash, 0.1f, Time.deltaTime);

        direction.x = horizontal;
        direction.z = vertical;

        // Turn character towards input direction plus camera rotation
        if (direction.magnitude > 0.1f)
        {
            angle = Mathf.Atan2(direction.x, direction.z);
            angle = angle * Mathf.Rad2Deg;
            angle += thisCamera.localEulerAngles.y;
            rotation.eulerAngles = new Vector3(0, angle, 0);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, characterRotationSpeed);
        }

        if (direction.magnitude > 1) direction.Normalize();

        thisAnimator.SetFloat("Speed", direction.magnitude * sprint * 0.5f, 0.1f, Time.deltaTime);

        // Create a vector3 for movement from Character forward x input magnitude x speed x sprint x dash
        movedirection = transform.forward * direction.magnitude * speed * sprint;

        // Get Jump input
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            // Initiate animation
            thisAnimator.SetTrigger("Jump");
            // Make jump timer = to jump time
            jumpTimer = jumpDuration;
        }

        // If we are above zero move up, Jump
        if (jumpTimer >= 0)
        {
            // decrease jumpTimer
            jumpTimer -= Time.deltaTime;
            movedirection *= 6f;
            // Add gradually decreasing
            movedirection += Vector3.up * jumpSpeed * jumpTimer * 10;
        }
        else
        {
            movedirection += Vector3.down * gravity;
        }

        // Move player
        if (CheckAnimationState(0, "JumpAttack") || CheckAnimationState(0, "Swipe"))
        {
            movedirection = Vector3.zero;
        }

        thisCharacterController.Move(movedirection * Time.deltaTime);
        isGrounded = thisCharacterController.isGrounded;
        thisAnimator.SetBool("isGrounded", isGrounded);

        if (Input.GetButtonDown("Fire1"))
        {
            thisAnimator.SetTrigger("Attack1");
        }

        if (Input.GetButtonDown("Fire2"))
        {
            thisAnimator.SetTrigger("Attack2");
        }

        if (Input.GetButtonDown("Fire3"))
        {
            thisAnimator.SetTrigger("Attack3");
        }
    }

    bool CheckAnimationState(int animationLayer, string animationStateName)
    {
        if (thisAnimator.GetCurrentAnimatorStateInfo(animationLayer).IsName(animationStateName))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
