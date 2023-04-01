using UnityEngine;

public class optionalParams : MonoBehaviour
{
    void Start() {
        SetX(this.gameObject, 2f); //Overides the default float value
        SetX(this.gameObject); //Keeps the default value
        print("Two of them worked");
    }

    void SetX(GameObject go, float eX = 0.0f) {
        Vector3 tempPos = go.transform.position;
        tempPos.x = eX;
        go.transform.position = tempPos;
    }
}
