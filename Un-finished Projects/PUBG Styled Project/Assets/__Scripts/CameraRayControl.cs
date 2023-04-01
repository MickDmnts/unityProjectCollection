using UnityEngine;

public class CameraRayControl : MonoBehaviour
{
    private void Update()
    {
        Ray mainRay = new Ray(transform.localPosition, Vector3.forward);
        RaycastHit hit;
        if (Physics.Raycast(mainRay,out hit,5f))
        {
            if (hit.collider.tag == "Sword")
            {
                SwordPickUp.S.displayText = true;
            }
        }
    }
}
