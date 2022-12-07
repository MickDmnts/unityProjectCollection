using UnityEngine;

public class FPSLimiter : MonoBehaviour
{
    [SerializeField] int maxFPS = 60;

    void Update()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = maxFPS;
    }
}
