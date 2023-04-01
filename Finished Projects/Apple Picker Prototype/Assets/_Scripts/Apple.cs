using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apple : MonoBehaviour
{
    public static Apple S;
    public static float bottomY = -20f;

    [Header("Set in Inspector")]
    public float appleRotationSpeed = 0.1f;

    [Header("Set Dynamically")]
    public GameObject applePickerGO;

    //Private Vars\\
    private bool _isRotated;

    public bool isRotated
    {
        get 
        {
            return _isRotated;
        }
        set
        {
            _isRotated = value;
        }
    }

    private void Awake()
    {
        applePickerGO = GameObject.Find("GameController");
        if (applePickerGO == null)
        {
            applePickerGO = GameObject.FindGameObjectWithTag("GameController");
        }
        S = this;
    }

    private void Update()
    {
        if (!_isRotated)
        {
            float rZ = -(appleRotationSpeed * Time.time * 360) % 360f;
            transform.rotation = Quaternion.Euler(0,0,rZ);
        }

        if(_isRotated)
        {
            float rZ2 = (appleRotationSpeed * Time.time * 360) % 360f;
            transform.rotation = Quaternion.Euler(0,0,rZ2);
        }

        if (transform.position.y < bottomY)
        {
            Destroy(this.gameObject);
            ApplePicker applePickerScript = applePickerGO.GetComponent<ApplePicker>();
            if (applePickerScript != null)
            {
                applePickerScript.AppleDestroyed();
            }
        }
    }
}
