using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyerOnLeave : MonoBehaviour 
{
	void OnTriggerExit(Collider other)
	{
		Destroy (other.gameObject);
	}
}
