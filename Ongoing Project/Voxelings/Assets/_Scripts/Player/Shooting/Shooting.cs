using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] GameObject enemyGO;
    [SerializeField] Vector3 enemyTransform;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag=="Enemies")
        {
            enemyGO = collision.gameObject;
            enemyTransform = collision.transform.position;
        }   
    }

    private void FixedUpdate()
    {
        if (enemyTransform!=null)
        {
            Debug.DrawLine(transform.position, enemyTransform);
        }
    }
}
