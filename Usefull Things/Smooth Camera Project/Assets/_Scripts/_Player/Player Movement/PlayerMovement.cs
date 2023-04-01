using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Basic player movement
    [Header("Set in inspector")]
    public float speedMultiplier;

    [Header("Set Dynamically")]
    [SerializeField] Transform playerTrans;

    private void Awake()
    {
        playerTrans = GetComponent<Transform>();
    }

    private void FixedUpdate()
    {
        //Move left-right
        float h = Input.GetAxisRaw("Horizontal");
        Vector3 movement = new Vector3(h, 0f) * speedMultiplier * Time.deltaTime;
        playerTrans.Translate(movement);
    }
}
