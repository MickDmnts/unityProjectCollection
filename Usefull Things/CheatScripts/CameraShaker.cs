using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Cinemachine;

public class CameraShaker : MonoBehaviour
{
    public static CameraShaker S;

    CinemachineVirtualCamera[] cameras;
    Dictionary<CinemachineVirtualCamera, CinemachineBasicMultiChannelPerlin> cmShakePairs;
    CinemachineBasicMultiChannelPerlin activeShakingCamera;

    float shakeLength;
    float totalShakeLength;
    float defaultIntensity;

    private void Awake()
    {
        S = this;
        cmShakePairs = new Dictionary<CinemachineVirtualCamera, CinemachineBasicMultiChannelPerlin>();
    }

    private void Start()
    {
        cameras = FindObjectsOfType<CinemachineVirtualCamera>();

        for (int i = 0; i < cameras.Length; i++)
        {
            cmShakePairs.Add(cameras[i], cameras[i].GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>());
        }
    }

    public void ConstantShake(int cameraIndex, float shakePower, float shakeLength)
    {
        if (cmShakePairs.TryGetValue(cameras[cameraIndex], out activeShakingCamera))
        {
            activeShakingCamera.m_AmplitudeGain = shakePower;
            defaultIntensity = activeShakingCamera.m_AmplitudeGain;

            totalShakeLength = shakeLength;
            this.shakeLength = shakeLength;
        }
        else
        {
            Debug.LogWarning("Invalid passed camera index");
        }
    }

    public void ShakeCamera(int cameraIndex, float shakePower, float shakeLength)
    {
        if (cmShakePairs.TryGetValue(cameras[cameraIndex], out activeShakingCamera))
        {

            activeShakingCamera.m_AmplitudeGain = shakePower;
            defaultIntensity = activeShakingCamera.m_AmplitudeGain;

            totalShakeLength = shakeLength;
            this.shakeLength = shakeLength;
        }
        else
        {
            Debug.LogWarning("Invalid passed camera index");
        }
    }

    private void Update()
    {
        if (shakeLength > 0)
        {
            shakeLength -= Time.deltaTime;
            DampShakeIntensity();
        }
    }

    void DampShakeIntensity()
    {
        activeShakingCamera.m_AmplitudeGain = Mathf.Lerp(defaultIntensity, 0f, (1 - (shakeLength / totalShakeLength)));
    }
}
