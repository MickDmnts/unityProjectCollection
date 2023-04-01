using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour {

    [SerializeField]
    private float speed = 180;

    protected void Update() {
        transform.Rotate(Vector3.up, Time.deltaTime * speed);
    }
}
