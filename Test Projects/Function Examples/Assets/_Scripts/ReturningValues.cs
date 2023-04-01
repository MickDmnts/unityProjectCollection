using UnityEngine;

public class ReturningValues : MonoBehaviour
{
    void Start() {
        int num = Add(1, 2);
        print("The sum is " + num);
    }

    int Add(int numA, int numB) {
        int sum = numA + numB;
        return sum;
    }
}
