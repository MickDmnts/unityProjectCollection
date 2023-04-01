using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private Transform tran; //Players' location
    private bool isStrafing = false;

    [Header("Weapon Controls")] //Weapon controls
    [Space]
    public Transform weaponToStrafe;

    [Range(0f,100f)]
    public float weaponStrafeSpeed;

    [Header("Player Controls")] //Player controls
    [Space]
    [Range(1f, 100f)]
    public float speed;

    [Range(1f, 1000f)]
    public float turnSpeed; 

    void Awake() //Executes once at the very first frame
    {
        tran = GetComponent<Transform>();
    }

    void FixedUpdate()
    {
        Move();
        WeaponStrafe();
    }

    void Move() //Player movement
    {
        if (isStrafing == false)
        {
            float hor = Input.GetAxis("Horizontal") * turnSpeed * Time.deltaTime; //Turn speed
            float ver = Input.GetAxis("Vertical") * speed * Time.deltaTime; //Forward movement

            tran.Translate(Vector3.forward * ver);
            tran.Rotate(Vector3.up * hor);
        }
    }

    void WeaponStrafe() //Weapon Rotation Up-Down
    {
        if (Input.GetKey(KeyCode.E)) //Up
        {
            isStrafing = true;
            weaponToStrafe.Rotate(new Vector3(-weaponStrafeSpeed, 0.0f, 0.0f));
        }
        else
            isStrafing = false;

        if (Input.GetKey(KeyCode.Q)) //Down
        {
            isStrafing = true;
            weaponToStrafe.Rotate(new Vector3(weaponStrafeSpeed, 0.0f, 0.0f));
        }
        else
            isStrafing = false;
    }

}
