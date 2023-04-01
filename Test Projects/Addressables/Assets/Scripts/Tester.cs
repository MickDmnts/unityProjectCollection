using UnityEngine;

[ExecuteInEditMode]
public class Tester : MonoBehaviour
{
    [SerializeField] GameObject prefab0;
    [SerializeField] GameObject prefab1;

    private void OnEnable()
    {
        AutoAddressSetter.SetAddressableGroup(prefab0, "testGroup", "a_050_100");
        AutoAddressSetter.SetAddressableGroup(prefab1, "testGroup2", "b_1000_1234");
    }
}
