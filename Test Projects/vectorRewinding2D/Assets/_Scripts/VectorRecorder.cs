using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectorRecorder : MonoBehaviour
{
    [SerializeField, Range(1, 25)] int listCapacity;
    [SerializeField, Range(0, 1)] float recordInterval;

    [SerializeField] List<Vector2> cordsToMoveTo;

    private void Start()
    {
        cordsToMoveTo = new List<Vector2>();
        cordsToMoveTo.Capacity = listCapacity;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ClearVectorList();

            StartCoroutine(StartVectorRecording());
        }
    }

    IEnumerator StartVectorRecording()
    {
        Vector2 tempVector = Vector2.zero;

        Debug.Log("Started recording");

        for (int i = 0; i < cordsToMoveTo.Capacity; i++)
        {
            tempVector.Set(gameObject.transform.position.x, gameObject.transform.position.y);
            cordsToMoveTo.Add(tempVector);

            yield return new WaitForSeconds(recordInterval);
        }

        Debug.Log("Recording completed");
    }

    void ClearVectorList()
    {
        cordsToMoveTo.Clear();
    }

    public List<Vector2> GetPastCordsList()
    {
        return cordsToMoveTo;
    }
}
