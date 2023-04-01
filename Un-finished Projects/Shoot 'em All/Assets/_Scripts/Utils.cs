using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils : MonoBehaviour
{
	static public Material[] GetAllMaterials(GameObject go)
	{
		List<Material> mats = new List<Material>();
		if (go.GetComponent<Renderer>() != null)
		{
			mats.Add(go.GetComponent<Renderer>().material);
		}
		foreach (Transform t in go.transform)
		{
			mats.AddRange(GetAllMaterials(t.gameObject));
		}
		return (mats.ToArray());
	}
}
