using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    [Header("Set in Inspector")]
    public GameObject cloudPrefab;
    public int minSphereNumber = 6;
    public int maxSphereNumber = 10;
    public Vector3 sphereOffsetScale = new Vector3(5, 2, 1);

    public Vector2 sphereScaleRangeX = new Vector2(4, 8);
    public Vector2 sphereScaleRangeY = new Vector2(3, 4);
    public Vector2 sphereScaleRangeZ = new Vector2(2, 4);

    public float scaleYMin = 2f;

    private List<GameObject> spheres; //Just for the Restart();

    private void Start()
    {
        spheres = new List<GameObject>();

        int num = Random.Range(minSphereNumber, maxSphereNumber);
        for (int i = 0; i < num; i++)
        {
            GameObject spGO = Instantiate(cloudPrefab);
            spheres.Add(spGO);
            Transform spGOTrans = spGO.transform;
            spGOTrans.SetParent(transform);

            //Randomly assign a position
            Vector3 offset = Random.insideUnitSphere;
            offset.x *= sphereOffsetScale.x;
            offset.y *= sphereOffsetScale.y;
            offset.z *= sphereOffsetScale.z;
            spGOTrans.localPosition = offset; //Is co ordinated to the center of the Cloud parent

            //Randomly assign a scale
            Vector3 scale = Vector3.one;
            scale.x = Random.Range(sphereScaleRangeX.x, sphereScaleRangeX.y); //X holds the minimum value and the Y holds the maximum value
            scale.y = Random.Range(sphereScaleRangeY.x, sphereScaleRangeY.y); //X holds the minimum value and the Y holds the maximum value
            scale.z = Random.Range(sphereScaleRangeZ.x, sphereScaleRangeZ.y); //X holds the minimum value and the Y holds the maximum value

            //Adjust y scale by x distance from the Clouds core
            scale.y *= 1 - (Mathf.Abs(offset.x) / sphereOffsetScale.x); //Bigger X, smaller Y
            scale.y = Mathf.Max(scale.y, scaleYMin);

            spGOTrans.localScale = scale;
        }
    }

    public void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.Space))
        {
            Restart();
        }*/
    }

    private void Restart()
    {
        /*foreach (GameObject sphere in spheres)
        {
            Destroy(sphere.gameObject);
        }

        Start();*/
    }
}
