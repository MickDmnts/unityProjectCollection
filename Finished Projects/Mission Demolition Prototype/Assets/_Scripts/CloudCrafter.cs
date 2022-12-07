using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudCrafter : MonoBehaviour
{
    [Header("Set in inspector")]
    public int numClouds = 40;
    public GameObject cloudPrefab;

    public Vector3 cloudPosMin = new Vector3(-100, -5, 10); //Distance the clouds can travel to the left
    public Vector3 cloudPosMax = new Vector3(200, 100, 10); //Distance the clouds can spawn to the right

    public float cloudScaleMin = 1;
    public float cloudScaleMax = 3;
    public float cloudSpeedMult = 0.5f;

    private GameObject[] cloudInsts;

    private void Awake()
    {
        cloudInsts = new GameObject[numClouds];
        GameObject anchor = GameObject.Find("CloudAnchor");
        GameObject cloud;
        for (int i = 0; i < numClouds; i++)
        {
            //Instantiate the cloud
            cloud = Instantiate<GameObject>(cloudPrefab);

            //Set the clouds position
            Vector3 cPos = Vector3.zero;
            cPos.x = Random.Range(cloudPosMin.x, cloudPosMax.x);
            cPos.y = Random.Range(cloudPosMin.y, cloudPosMax.y);

            //Scale the cloud
            float scaleU = Random.value;
            float scaleVal = Mathf.Lerp(cloudScaleMin, cloudScaleMax, scaleU);

            //Smaller clouds (with smaller U) should be nearer the ground
            cPos.y = Mathf.Lerp(cloudPosMin.y, cPos.y, scaleU);

            //Smaller clouds should be further away
            cPos.z = 100 - 90 * scaleU;

            //Apply transforms
            cloud.transform.position = cPos;
            cloud.transform.localScale = Vector3.one * scaleVal;

            //Make cloud, child of the anchor
            cloud.transform.SetParent(anchor.transform);

            //Add the cloud to the cloud instances
            cloudInsts[i] = cloud;
        }
    }

    private void Update()
    {
        foreach (GameObject cloud in cloudInsts)
        {
            //Get the cloud scale and position
            float scaleValue = cloud.transform.localScale.x;
            Vector3 cloudPos = cloud.transform.position;

            //Move larger clouds faster
            cloudPos.x -= scaleValue * Time.deltaTime * cloudSpeedMult;

            //If a cloud has moved too far to the left, move it back to the far right
            if (cloudPos.x <= cloudPosMin.x)
            {
                cloudPos.x = cloudPosMax.x;
            }

            //Apply the transform
            cloud.transform.position = cloudPos;
        }
    }
}
