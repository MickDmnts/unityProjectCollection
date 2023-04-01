using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnGroundTouch : MonoBehaviour
{
    [Header("Set Dynamically")]
    public Transform spawnPoint;
    public GameObject player;
    public float slowDownFactor;

    [Header("Camera Freezer, Dynamic")]
    public Camera m_mainCamera;

    [Header("Player Scripts")]
    public LineRendering script_lineR;

    private bool _respawn = false;

    private void Awake()
    {
        spawnPoint = GameObject.Find("SpawnPoint").GetComponent<Transform>();
        player = GameObject.FindGameObjectWithTag("Player");
        m_mainCamera = Camera.main;
        script_lineR = player.GetComponent<LineRendering>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            LineRendering.S.Clear(0);
            script_lineR.enabled = false;
            SlowdownTime(slowDownFactor);
            CameraFollow.S.canFollow = false;
        }
    }

    void SlowdownTime(float slowness)
    {
        Time.timeScale = slowness;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
        StartCoroutine(ResetTime(true));
    }

    IEnumerator ResetTime(bool doReset)
    {
        if (doReset)
        {
            yield return new WaitForSecondsRealtime(1.8f);
            _respawn = true;
        }
        else
        {
            yield return null;
        }
    }

    private void FixedUpdate()
    {
        if (_respawn)
        {
            player.transform.position = spawnPoint.transform.position;
            CameraFollow.S.canFollow = true;
            Time.timeScale = 1f;
            Time.fixedDeltaTime = Time.timeScale * 0.02f;
            _respawn = false;
            script_lineR.enabled = true;
            LineRendering.S.Clear(1);
        }
    }
}
