using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMove : MonoBehaviour
{
    [SerializeField] Joystick joystick;
    [SerializeField] float moveSpeed=2f;

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 horizontal = new Vector3(joystick.Horizontal * moveSpeed, 0f, 0f);
        transform.position = transform.position + horizontal * Time.deltaTime;

        Vector3 vertical = new Vector3(0f, joystick.Vertical * moveSpeed, 0f);
        transform.position = transform.position + vertical * Time.deltaTime;
    }
}
