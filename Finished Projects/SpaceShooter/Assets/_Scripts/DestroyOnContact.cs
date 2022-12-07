using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnContact : MonoBehaviour 
{
	public GameObject explosion;
	public GameObject shipExplosion;
    public int scoreValue;
    private GameController gameController;

    void Start()
    {
        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        if (gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<GameController>();
        }
        else
        {
            Debug.Log("Cannot find the 'Game Controller' script.");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Bounds")
        {
            return;
        }

        Instantiate(explosion, transform.position, transform.rotation);
        if (other.tag == "Player")
        {
            Instantiate(shipExplosion, other.transform.position, other.transform.rotation);
            other.GetComponent<AudioSource>().Play();
            gameController.NewScore(-scoreValue);
            gameController.GameOver();
        }
        gameController.NewScore(scoreValue);
        Destroy(other.gameObject);
        Destroy(gameObject);
    }
}
