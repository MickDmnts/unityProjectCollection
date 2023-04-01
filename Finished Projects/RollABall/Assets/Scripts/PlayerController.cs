using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public Text counter;
    public Text win;

    public Text FOV; //Field of View
    public Slider FOVSlider;

    public Text Volume; //Volume Control
    public Slider VolControl;

	private Rigidbody rb;
    private int count;

	void Start() //sets the rb var equal to the Player Rigidbody component
	{
		rb = GetComponent<Rigidbody>();
        count = 0;
        win.enabled = false;
        SetCountText();
	}


    void Update()
    {
        FOV.text = "Field of View: " + FOVSlider.value.ToString();
        Volume.text = "Volume: " + VolControl.value;
    }

    void FixedUpdate() //Takes the position of the player and applies a force to it so it can move (gets executed every frame)
    {
        float hor = Input.GetAxis("Horizontal");
        float ver = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(hor, 0.0f, ver);

        rb.AddForce(movement * speed);

		if (Input.GetKeyUp (KeyCode.Escape)) {
			Application.Quit ();
		}
    }

    void OnTriggerEnter(Collider other) //Everytime the ball touches a pick up, the pick up is set to inactive
    {
        if (other.gameObject.CompareTag("Pick Up"))
        {
            other.gameObject.SetActive(false);
            count++;
            SetCountText();
        }
    }

    void SetCountText()
    {
        counter.text = "Pick Ups: " + count.ToString();
        if (count == 12)
        {
            win.enabled = true;
        }
    }
}
