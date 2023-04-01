using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interp2 : MonoBehaviour 
{
	[Header("Set in inspector")]
	public Transform c0;
	public Transform c1;
	public Transform c2;
	public Transform c3;
	public float uMin = 0;
	public float uMax = 1;
	public float timeDuration = 1;
	//Check to start the L.I;
	public bool checkToStart = false;
	public bool loopMove = true;

	[Header("Set Dynamically")]
	public Vector3 p01; //pos
	public Color c01; //color
	public Vector3 s01; //scale
	public bool moving = false;
	public float timeStart;

	private Material mat, matC0, matC1, matC2, matC3;

	private Vector3 p12,p23,p012,p123, p0123;
	private Color c12,c23,c012,c123, c0123;
	private Vector3 s12,s23,s012,s123, s0123;

	void Awake()
	{
		mat = GetComponent<Renderer>().material;
		matC0 = c0.GetComponent<Renderer>().material;
		matC1 = c1.GetComponent<Renderer>().material;
		matC2 = c2.GetComponent<Renderer>().material;
		matC3 = c3.GetComponent<Renderer>().material;
	}

	void Update()
	{
		if (checkToStart)
		{
			checkToStart = false;
			moving = true;
			timeStart = Time.time;
		}

		if (moving)
		{
			float u = (Time.time - timeStart)/timeDuration;
			if (u >= 1)
			{
				u = 1;
				if (loopMove) //if reapeat is ON
				{
					timeStart = Time.time;
				}
				else
				{
					moving = false;
				}
			}

			u = (1-u)*uMin + u*uMax;

			//Standard interpolation function
			//POSITION
			p01 = (1-u)*c0.position + u*c1.position;
			p12 = (1-u)*c1.position + u*c2.position;
			p23  =(1-u)*c2.position + u*c3.position;
			p012 = (1-u)*p01 + u*p12;
			p123 = (1-u)*p12 + u*p23;
			p0123 = (1-u)*p012 + u*p123;

			//COLOR
			c01 = (1-u)*matC0.color + u*matC1.color;
			c12 = (1 - u) * matC1.color + u * matC2.color;
			c23 = (1-u)*matC2.color + u*matC3.color;
            c012 = (1 - u) * c01 + u*c12;
            c123 = (1 - u) * c12 + u * c23;
            c0123 = (1 - u) * c012 + u * c123;

			//SCALE
			s01 = (1-u)*c0.localScale + u*c1.localScale;
            s12 = (1 - u) * c1.localScale + u * c2.localScale;
			s23 = (1-u)*c2.localScale + u*c3.localScale;
            s012 = (1 - u) * s01 + u*s12;
            s123 = (1 - u) * s12 + u * s23;
			s0123 = (1-u)*s012 + u*s123;

			//Apply settings
			transform.position = p0123;
			mat.color = c0123;
			transform.localScale = s0123;
		}
	}
}
