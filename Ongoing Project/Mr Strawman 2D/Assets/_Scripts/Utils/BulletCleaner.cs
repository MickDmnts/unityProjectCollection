using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCleaner : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag != "Player")
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        Destroy(gameObject, 2f);
    }
}
