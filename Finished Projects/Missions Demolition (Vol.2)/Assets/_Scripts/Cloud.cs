using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    [Header("Set in inspector")]
    public GameObject cloudPrefab;
    public int sphereNumMin = 6;
    public int sphereNumMax = 10;
    public Vector3 sphereOffsetScale = new Vector3(5, 2, 1);
    public Vector2 sphereScaleRangeX = new Vector2(4, 8);
    public Vector2 sphereScaleRangeY = new Vector2(3, 4);
    public Vector2 sphereScaleRangeZ = new Vector2(2, 4);
    public float scaleYMin = 2f;

    private List<GameObject> spheresList;
    private Transform sphereTrans;

    private void Start()
    {
        spheresList = new List<GameObject>();

        int num = Random.Range(sphereNumMin, sphereNumMax);
        for (int i = 0; i < num; i++)
        {
            CreateSphereGO();
            sphereTrans.localScale = AdjustDistanceFromCloudCore(AssignRandomScale(), AssignRandomPos());
        }

    }

    void CreateSphereGO()
    {
        GameObject sphere = Instantiate<GameObject>(cloudPrefab);
        spheresList.Add(sphere);
        sphereTrans = sphere.transform;
        sphereTrans.SetParent(this.transform);
    }

    Vector3 AssignRandomPos()
    {
        Vector3 offset = Random.insideUnitSphere;
        offset.x *= sphereOffsetScale.x;
        offset.y *= sphereOffsetScale.y;
        offset.z *= sphereOffsetScale.z;
        sphereTrans.localPosition = offset;
        return offset;
    }

    Vector3 AssignRandomScale()
    {
        Vector3 scale = Vector3.one;
        scale.x = Random.Range(sphereScaleRangeX.x, sphereScaleRangeX.y);
        scale.y = Random.Range(sphereScaleRangeY.x, sphereScaleRangeY.y);
        scale.z = Random.Range(sphereScaleRangeZ.x, sphereScaleRangeZ.y);
        return scale;
    }

    Vector3 AdjustDistanceFromCloudCore(Vector3 scale,Vector3 offset)
    {
        scale.y *= 1 - (Mathf.Abs(offset.x) / sphereOffsetScale.x);
        scale.y = Mathf.Max(scale.y, scaleYMin);
        return scale;
    }

    private void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.Space))
        {
            Restart();
        }*/
    }

    void Restart()
    {
        foreach (GameObject sphereGO in spheresList)
        {
            Destroy(sphereGO);
        }

        Start();
    }

}
