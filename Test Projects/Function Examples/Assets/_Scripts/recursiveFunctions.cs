using UnityEngine;

public class recursiveFunctions : MonoBehaviour
{
    void Start() {
        print(Fac(5));
        print(Fac(0));
        print(Fac(-1));
    }

    int Fac(int n) {
        if (n < 0)
        {
            return 0;
        }
        if (n == 0)
        {
            return 1;
        }
        int result = n * Fac(n - 1);
        return result;
    }
}
