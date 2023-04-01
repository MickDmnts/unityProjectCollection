using UnityEngine;

public class FunctionOverloading : MonoBehaviour
{
    void Start() {
        float number = Add(1, 2);
        print(number); //1

        print(Add(new Vector3(1, 0, 1), new Vector3(2, 3, 1))); //2

        Color one = new Color(0.5f, 1f, 0f, 1f);
        Color two = new Color(1f, 2f, 3f, 1f);
        print(Add(one, two));
    }

    float Add(float f0, float f1) {
        return f0 + f1;
    }

    Vector3 Add(Vector3 v0, Vector3 v1) {
        return v0 + v1;
    }

    Color Add(Color c0, Color c1) {
        float r, g, b, a;
        r = Mathf.Min(c0.r + c1.r, 1.0f);
        g = Mathf.Min(c0.g + c1.g, 1.0f);
        b = Mathf.Min(c0.b + c1.b, 1.0f);
        a = Mathf.Min(c0.a + c1.a, 1.0f);
        return new Color(r, g, b, a);
    }
}
