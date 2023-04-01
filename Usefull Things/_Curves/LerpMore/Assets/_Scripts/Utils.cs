using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Type
{
	linear,
	easeIn,
	easeOut,
	easeInOut,
	sin,
	sinIn,
	sinOut,
}

public class Utils : MonoBehaviour 
{
//============================================ Materials Functions ===========================================\\

	//Returns a list of all Materials on this gameObject and its children
	static public Material[] GetAllMaterials(GameObject go)
	{
		Renderer[] rends = go.GetComponentsInChildren<Renderer>();

		List<Material> mats = new List<Material>();
		foreach (Renderer rend in rends)
		{
			mats.Add(rend.material);
		}

		return mats.ToArray();
	}

//=========================================== Math Functions =====================================================\\

	static public int Recursive(int n)
	{
		if ( n < 0)
		{
			return 0;
		}

		if (n == 0)
		{
			return 1;
		}

		int result = n * Recursive( n - 1 );
		return result;
	}

	static public float Ease( float u, Type eType, float eMod = 2)
	{
		float u2 = u;

		switch (eType)
		{
			case Type.linear:
				u2 = u;
				break;
			case Type.easeIn:
				u2 = Mathf.Pow(u,eMod);
				break;
			case Type.easeOut:
				u2 = 1 - Mathf.Pow( 1-u, eMod);
				break;
			case Type.easeInOut:
				if (u <= 0.5f)
				{
					u2 = 0.5f * Mathf.Pow( u*2,eMod );
				}
				else
				{
					u2 = 0.5f + 0.5f * ( 1-Mathf.Pow(1-(2*(u-0.5f)),eMod));
				}
				break;
			case Type.sin:
				u2 = u + eMod * Mathf.Sin( 2*Mathf.PI*u );
				break;
			case Type.sinIn:
				u2 = 1 - Mathf.Cos( u*Mathf.PI*0.5f);
				break;
			case Type.sinOut:
				u2 = Mathf.Sin( u*Mathf.PI*0.5f);
				break;
		}
		return u2;
	}
}