using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxManager : MonoBehaviour
{
    [Header("Set in inspector")]
    public List<Transform> backgroundsTransforms;
    public float parallaxSmoothing = 1f;

    private float[] parallaxScales;
    private Transform mainCamera;
    private Vector3 previousFrameCamTransform;

    private void Awake()
    {
        CheckIfSmoothingBellowZero();
        CheckIfMainCameraIsNull();
    }

    private void Start()
    {
        AssignPreviousFrameCamTransform();
        AssignParallaxScales();
    }

    private void Update()
    {
        ApplyBackgroundParallaxing();
        AssignPreviousFrameCamTransform();
    }

    void CheckIfSmoothingBellowZero()
    {
        if (parallaxSmoothing <= 0)
            parallaxSmoothing = 1f;
    }

    void CheckIfMainCameraIsNull()
    {
        if (mainCamera == null)
            mainCamera = Camera.main.gameObject.transform;
    }

    public void AddBackgroundToTheList(Transform backgroundToAdd)
    {
        backgroundsTransforms.Add(backgroundToAdd);
        AssignParallaxScales();
    }

    void AssignPreviousFrameCamTransform()
    {
        previousFrameCamTransform = mainCamera.position;
    }

    void AssignParallaxScales()
    {
        parallaxScales = new float[backgroundsTransforms.Count]; //create temp floats
        for (int i = 0; i < backgroundsTransforms.Count; i++)
        {
            parallaxScales[i] = backgroundsTransforms[i].position.z * -1; //Assign each scale the BGs scale
        }
    }

    void ApplyBackgroundParallaxing()
    {
        for (int i = 0; i < backgroundsTransforms.Count; i++)
        {
            float parallax = (previousFrameCamTransform.x - mainCamera.position.x) * parallaxScales[i];
            float backgroundTargetPosX = backgroundsTransforms[i].position.x + parallax;
            Vector3 backgroundTargetPos = new Vector3(backgroundTargetPosX, backgroundsTransforms[i].position.y, backgroundsTransforms[i].position.z);

            //Time.deltaTime converts frames to seconds 
            backgroundsTransforms[i].position = Vector3.Lerp(backgroundsTransforms[i].position, backgroundTargetPos, parallaxSmoothing * Time.deltaTime);
        }
    }
}
