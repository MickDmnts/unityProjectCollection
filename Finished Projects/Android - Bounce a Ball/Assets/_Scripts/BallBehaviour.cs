using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBehaviour : MonoBehaviour
{
    [Header("Set dynamically")]
    [SerializeField] Rigidbody2D ballRb;
    [SerializeField] bool firstTouch = false;
    [SerializeField] Vector2 randomDirection;

    [Header("Set in inspector")]
    public float ballVelocity = 1f;

    private void Awake()
    {
        AssignReferences();
    }

    private void AssignReferences()
    {
        ballRb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Input.anyKeyDown && !firstTouch)
        {
            RandomFirstBounceDirection();
            firstTouch = true;
        }
        KeepConstantVelocity();
    }

    void RandomFirstBounceDirection()
    {
        randomDirection = new Vector2(Random.Range(-1, 1), 1);

        if (CheckIfRandomDirectionIsZero())
        {
            RandomFirstBounceDirection();
        }

        ballRb.AddForce(randomDirection * ballVelocity, ForceMode2D.Impulse);
    }

    bool CheckIfRandomDirectionIsZero()
    {
        if (randomDirection.x == 0)
            return true;
        else
            return false;
       
    }

    void KeepConstantVelocity()
    {
        ballRb.velocity = ballVelocity * (ballRb.velocity.normalized);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            ScoreManager.S.IncreaseScoreAndApply();
        }
    }
}
