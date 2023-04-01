using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
    public static CameraFollow S; //Singleton

    [Header("What to follow")]
    public Transform player;

    [Header("The camera offset and easing")]
    public Vector3 cameraOffset;
    public float cameraEasing;

    [Header("State changers")]
    public bool isLooking;
    public bool canFollow;

    [Header("Player Movement Scripts")]
    [SerializeField]GameObject playerScripts;
    PlayerMovement scriptMove;
    MainJump scriptJump;
    LineRendering script_lineR;

    private void Awake()
    {
        isLooking = true;
        canFollow = false;
        S = this;

        scriptMove = playerScripts.GetComponent<PlayerMovement>();
        scriptJump = playerScripts.GetComponent<MainJump>();
        script_lineR = playerScripts.GetComponent<LineRendering>();
    }

    private void Start()
    {
        StartCoroutine(WaitTimeUntilLanding());
    }

    IEnumerator WaitTimeUntilLanding()
    {
        yield return new WaitForSecondsRealtime(2f);
        isLooking = false;
        canFollow = true;
    }

    private void FixedUpdate()
    {
        if (isLooking)
        {
            transform.position = new Vector3(-6.5f, 1.5f, -5f);
            Camera.main.transform.LookAt(player);
        }

        if (canFollow)
        {
            Vector3 desiredPos = player.position + cameraOffset;
            Vector3 smoothedPos = Vector3.Lerp(transform.position, desiredPos, cameraEasing);
            transform.position = smoothedPos;

            //Scripts
            scriptMove.enabled = true;
            scriptJump.enabled = true;
            script_lineR.enabled = true;
        }
    }
}
