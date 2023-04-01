using UnityEngine;

public class paramsKeyword : MonoBehaviour
{
    void Start() {
        print(Add(1));
        print(Add(1,2,3));
        print(Add(1,5,2));
        print(Add(1,2));
        print(Add(1,0));
    }

    int Add(params int[] ints) { //params
        int sum = 0;
        foreach (int myInt in ints)
        {
            sum += 1;
        }
        return sum;
    }
}
