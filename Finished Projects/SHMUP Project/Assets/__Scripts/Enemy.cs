using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Set in Inspector: Enemy")]
    public float speed = 10f;
    public float fireRate = 0.3f;
    public float health = 10;
    public int score = 100; //Score you getfrom destroying this enemy


    private BoundsCheck bndCheck;

    private void Awake()
    {
        bndCheck = GetComponent<BoundsCheck>();
    }

    /// <summary>
    /// A property
    /// returns the position of the ship
    /// sets the position of the ship to a given value
    /// </summary>
    public Vector3 pos
    {
        get
        {
            return this.transform.position;
        }
        set
        {
            transform.position = value;
        }
    }

    /// <summary>
    /// Calls the Move() function every frame.
    /// Checks the ships position
    /// </summary>
    private void Update()
    {
        Move();

        if (bndCheck != null && bndCheck.offDown)
        {
            Destroy(this.gameObject);
        }
    }

    /// <summary>
    /// Stores the current position of the enemy ship
    /// then sets its pos to -speed*time.deltaTime (-speed is used to go downwards)
    /// then sets it back to the GO
    /// </summary>
    public virtual void Move() 
    {
        Vector3 temp = pos;
        temp.y -= speed * Time.deltaTime;
        pos = temp;
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject collGo = collision.gameObject;
        if (collGo.tag == "ProjectileHero")
        {
            Destroy(this.gameObject);
            Destroy(collGo);
        }
        else
        {
            Debug.Log("ENemy hit by non-ProjectileHero: " + collGo.gameObject.name);
        }
    }
}
