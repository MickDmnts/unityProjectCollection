using UnityEngine;

public class Slingshot : MonoBehaviour
{
    static private Slingshot S;

    [Header("Set in Inspector")]
    public GameObject projectilePrefab;
    public float velocityMultiplier = 8f;

    [Header("Set Dynamically")]
    public GameObject launchPoint;
    public Vector3 launchPos;
    public GameObject projectile;
    public Rigidbody projectileRigidbody;
    public bool isAiming;

    static public Vector3 LAUNCH_POS
    {
        get
        {
            if (S == null)
            {
                return Vector3.zero;
            }
            return S.launchPos;
        }
    }

    private void Awake()
    {
        S = this;
        Transform launchPointTrans = transform.Find("LaunchPoint");
        launchPoint = launchPointTrans.gameObject;
        launchPoint.SetActive(false); //Deactivate the Halo ring

        launchPos = launchPointTrans.position; //Set the launch point of the Projectile to the LaunchPoint position (middle of the slingshot)
    }

    private void OnMouseEnter()
    {
        //print("SlingShot: OnMouseEnter()");
        launchPoint.SetActive(true); //Enable the Halo ring when the mouse is inside the collider
    }

    private void OnMouseExit()
    {
        //print("Slingshot: OnMouseExit()");
        launchPoint.SetActive(false); //Disable the Halo ring when the mouse exits the collider
    }

    private void OnMouseDown()
    {
        isAiming = true; //Set isAiming to true
        projectile = Instantiate(projectilePrefab) as GameObject; //Instantiate a Projectile as a GO
        projectile.transform.position = launchPos; //The position of the projectile is the middle of the slingshot
        projectileRigidbody = projectile.GetComponent<Rigidbody>();
        projectileRigidbody.isKinematic = true; //Set the rigidbody to kinematic
    }

    private void Update()
    {
        if (!isAiming)
        {
            return;
        }

        Vector3 mousePos2D = Input.mousePosition;
        mousePos2D.z = -Camera.main.transform.position.z;
        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);

        Vector3 mouseDelta = mousePos3D - launchPos;
        float maxLength = this.GetComponent<SphereCollider>().radius;
        if (mouseDelta.magnitude > maxLength)
        {
            mouseDelta.Normalize();
            mouseDelta *= maxLength;
        }

        Vector3 projPos = launchPos + mouseDelta;
        projectile.transform.position = projPos;

        if (Input.GetMouseButtonUp(0))
        {
            isAiming = false;
            projectileRigidbody.isKinematic = false;
            projectileRigidbody.velocity = -mouseDelta * velocityMultiplier;
            FollowCam.POI = projectile;
            projectile = null;
            GameController.ShotFired();
            ProjectileLine.S.poi = projectile;
        }
    }
}
