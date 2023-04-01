using UnityEngine;

public class Enemy : MonoBehaviour
{
    //These two are fields
    public float speed = 10f; // This speed is in m/s 
    public float fireRate = 0.3f;

    //All of these below are functions
    private void Update()
    {
        Move(); 
    }

    public virtual void Move()
    {
        Vector3 tempPos = Pos;
        tempPos.y -= speed * Time.deltaTime;
        Pos = tempPos;
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject other = collision.gameObject;
        switch (other.tag)
        {
            case "Hero":
                //Currently not implemented
                break;
            case "HeroLaser":
                //Destroy this enemy
                Destroy(this.gameObject);
                break;
        }
    }

    //This is a property
    public Vector3 Pos
    {
        get
        {
            return this.transform.position; 
        }
        set
        {
            this.transform.position = value; 
        }
    }
}
