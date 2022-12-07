using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public BoundsCheck bndsCheck;

    private void Awake()
    {
        bndsCheck = GetComponent<BoundsCheck>();
    }

    private void Update()
    {
        if (bndsCheck.offUp)
        {
            Destroy(this.gameObject);
        }
    }
}
