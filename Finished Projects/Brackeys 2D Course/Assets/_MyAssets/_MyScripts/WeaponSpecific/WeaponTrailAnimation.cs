using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponTrailAnimation : MonoBehaviour
{
    [Header("Weapon trail moving speed - Set in Inspector")]
    [Range(230f,1000f)]
    public float weaponTrailSpeed = 230f;

    private void Update()
    {
        AnimateTrail();
    }

    void AnimateTrail()
    {
        this.transform.Translate(Vector3.right * Time.deltaTime * weaponTrailSpeed);
    }
}
