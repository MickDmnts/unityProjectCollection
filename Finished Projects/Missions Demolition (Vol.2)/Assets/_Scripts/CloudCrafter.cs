using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudCrafter : MonoBehaviour
{
    [Header("Set in inspector")]
    public int numOfCloudInstances = 40;
    public GameObject cloudPrefab;
    public Vector3 cloudMinDistance = new Vector3(-50, -5, 10);
    public Vector3 cloudMaxDistance = new Vector3(150, 100, 10);
    public float cloudMinScale = 1;
    public float cloudMaxScale = 3;
    public float cloudSpeedMultiplier = .5f;

    private GameObject[] cloudInstances;
    private GameObject cloudAnchor;
    private float scaleVal;

    private void Awake()
    {
        InitializeArrayAndAnchor();

        if (cloudAnchor != null)
        {
            CloudMaker();
        }
        else
        {
            Debug.LogError("Cloud Anchor not properly set");
        }
    }

    void InitializeArrayAndAnchor()
    {
        cloudInstances = new GameObject[numOfCloudInstances];
        cloudAnchor = GameObject.FindGameObjectWithTag("CloudAnchor");
    }

    void CloudMaker()
    {
        GameObject cloud;
        for (int i = 0; i < cloudInstances.Length; i++)
        {
            cloud = Instantiate<GameObject>(cloudPrefab);

            Vector3 cloudPos = Vector3.zero;
            cloudPos.x = Random.Range(cloudMinDistance.x, cloudMaxDistance.x);
            cloudPos.y = Random.Range(cloudMinDistance.y, cloudMaxDistance.y);

            cloud.transform.position = PositionSmallerCloudsNearGround(cloudPos);
            cloud.transform.localScale = Vector3.one * scaleVal;

            cloud.transform.SetParent(cloudAnchor.transform);

            cloudInstances[i] = cloud;
        }
    }

    Vector3 PositionSmallerCloudsNearGround(Vector3 posToModify)
    {
        float scaleU = Random.value;
        scaleVal = Mathf.Lerp(cloudMinScale, cloudMaxScale, scaleU);
        posToModify.y = Mathf.Lerp(cloudMinDistance.y, posToModify.y, scaleU);
        posToModify.z = 100 - 90 * scaleU;
        return posToModify;
    }

    private void Update()
    {
        foreach (GameObject cloud in cloudInstances)
        {
            float cloudScale = cloud.transform.localScale.x;
            Vector3 cloudPos = cloud.transform.position;
            cloudPos.x -= cloudScale * Time.deltaTime * cloudSpeedMultiplier;
            if (cloudPos.x <= cloudMinDistance.x)
            {
                cloudPos.x = cloudMaxDistance.x;
            }

            cloud.transform.position = cloudPos;

        }
    }
}
