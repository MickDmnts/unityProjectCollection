using UnityEngine;

public class MainJump : MonoBehaviour
{
    //Main jump script
    [Header("Set in Inspector")]
    [Range(1, 10)]
    public float jumpForce;
    [Range(0f,1f)]
    public float distance = 1f; //Player scale 1f

    [Header("Set Dynamically")]
    [SerializeField] Rigidbody playerRb;
    [SerializeField] bool canJump;
    [SerializeField] bool jumpRequest;
    [SerializeField] bool isJumping;
    [SerializeField] GameObject leftDetector;
    [SerializeField] GameObject rightDetector;

    private void Awake()
    {
        playerRb = GetComponent<Rigidbody>();
        leftDetector = GameObject.Find("LeftDetector");
        rightDetector = GameObject.Find("RightDetector");
    }

    private void Start()
    {
        canJump = true;
        jumpRequest = false;
        isJumping = false;
    }

    private void Update()
    {
        //Jump
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (canJump && !isJumping)
            {
                canJump = false;
                jumpRequest = true;
            }
        }

        Ray myRay = new Ray(transform.position, Vector3.down);
        Ray leftRay = new Ray(leftDetector.transform.position, Vector3.down);
        Ray rightRay = new Ray(rightDetector.transform.position, Vector3.down);
        RaycastHit hit;
        RaycastHit leftHit;
        RaycastHit rightHit;

        if (Physics.Raycast(myRay,out hit,distance) | Physics.Raycast(leftRay,out leftHit,distance) | Physics.Raycast(rightRay,out rightHit,distance))
        {
            canJump = true;
            isJumping = false;
        }
        else
        {
            canJump = false;
        }
    }

    private void FixedUpdate()
    {
        if (jumpRequest && !isJumping)
        {
            isJumping = true;
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            jumpRequest = false;
        }
    }
}
