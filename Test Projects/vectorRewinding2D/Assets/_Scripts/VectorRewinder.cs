using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectorRewinder : MonoBehaviour
{
    public static VectorRewinder S;

    private void Awake()
    {
        S = this;
    }

    public void StartRewind(GameObject goToMove, List<Vector2> externalCords)
    {
        _InternalStartRewind(goToMove, externalCords);
    }

    void _InternalStartRewind(GameObject goToMove, List<Vector2> cordsToMoveTo)
    {
        StartCoroutine(Rewinding(goToMove, cordsToMoveTo));
    }

    IEnumerator Rewinding(GameObject goToMove, List<Vector2> listOfVectors)
    {
        for (int i = listOfVectors.Count - 1; i >= 0; i--)
        {
            try
            {
                goToMove.transform.position = listOfVectors[i];
            }
            catch (System.NullReferenceException)
            {
                Debug.Log("Gameobject is null");
                throw;
            }
            yield return new WaitForSeconds(.1f);
        }

        Debug.Log("Rewind Complete");
    }
}
