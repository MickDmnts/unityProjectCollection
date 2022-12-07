using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHandler : MonoBehaviour
{
    public GameObject explosionPrefab;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject effect = Instantiate(explosionPrefab, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
