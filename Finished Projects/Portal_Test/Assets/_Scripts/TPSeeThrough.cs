using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPSeeThrough : MonoBehaviour
{
    //We want the camera to render to a texture attached to the SAME gO

    [SerializeField] private Camera portalCamera;
    [SerializeField] private RenderTexture targetRT;

    private void Start()
    {
        portalCamera = this.GetComponent<Camera>();


    }
}
