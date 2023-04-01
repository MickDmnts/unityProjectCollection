using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionHandler : MonoBehaviour
{
    [Header("Set Dynamically")]
    public Animator explosionAnimator;

    void Awake()
    {
        AssignAnimatorReference();
    }

    void Update()
    {
        CheckIfAnimatorNull();

        if(this.explosionAnimator.GetBool("DestroyExplosion") == true)
        {
            Destroy(this.gameObject);
        }
    }

    void AssignAnimatorReference()
    {
        explosionAnimator = this.GetComponent<Animator>();
    }

    void CheckIfAnimatorNull()
    {
        if (this.explosionAnimator == null)
        {
            Debug.LogError("explosionAnimator is not set");
            return;
        }
    }

    public void SetDestroy()
    {
        this.explosionAnimator.SetBool("DestroyExplosion", true);
    }
}
