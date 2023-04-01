using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Dropping : MonoBehaviour
{
    public GameObject heartGO;
    public float droppingSpeed = 1f;
    public float camHeight;


    // Start is called before the first frame update
    void Start()
    {
        heartGO = gameObject;
        camHeight = Camera.main.orthographicSize;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        heartGO.transform.Translate(Vector3.down * droppingSpeed*Time.deltaTime);

        if (heartGO.transform.localPosition.y <= -camHeight)
        {
            Destroy(heartGO);
            LifeManager.S.DecreaseLives();
        }
        else if (LifeManager.S.livesLeft <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private void OnMouseDown()
    {
        LifeManager.S.IncreasePoints();
        Destroy(heartGO);
    }
}
