using UnityEngine;

public class AppleTree : MonoBehaviour
{
    static public AppleTree S;

    [Header("Set in Inspector")]
    public GameObject applePrefab;
    public float speed = 1f;
    public float leftAndRightEdge = 10f;
    public float chanceToChangeDirections = 0.1f;
    public float secondsBetweenAppleDrops = 1f;

    //Apple specific\\
    public float propabilityToSwapRotation = 0.5f;


    private void Start()
    {
        Invoke("DropApple", 2f);
        S = this;
    }

    private void Update()
    {
        //Basic movement
        Vector3 pos = transform.position;
        pos.x += speed * Time.deltaTime;
        transform.position = pos;

        //Changing Directions
        if (pos.x < -leftAndRightEdge)
        {
            speed = Mathf.Abs(speed);
        }
        else if (pos.x > leftAndRightEdge)
        {
            speed = -Mathf.Abs(speed);
        }        
    }

    private void FixedUpdate()
    {
        if (Random.value < chanceToChangeDirections)
        {
            speed *= -1;
        }
    }

    void DropApple()
    {
        GameObject apple = Instantiate<GameObject>(applePrefab);
        float randomizer = Random.value;

        //Gets the random value from randomizer
        if (randomizer >= propabilityToSwapRotation) //If it is greater than the swapRotationProp
        {
            apple.transform.rotation = Quaternion.Euler(0,180f,0); //set its rotation to the opposite of it
            Apple.S.isRotated = true; //Call isRotated from the Apple script to notify it that this apple has changed direction
        }
        else
        {
            Apple.S.isRotated = false;
        }

        apple.transform.position = transform.position;
        Invoke("DropApple", secondsBetweenAppleDrops);
    }
}
