using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastShoot : MonoBehaviour
{
    //Public Vars\\
    [Header("Set in inspector - Hover over Damage for defaults")]
    [Tooltip("Defaults: Damage = 1f;\nfireRate = .25f;\nweaponRange = 50f;\nhitForce = 100f;")]
    public int damage = 1;
    public float fireRate = .25f;
    public float weaponRange = 50f;
    public float hitForce = 100f;
    public Transform gunEnd;

    //Private Vars\\
    private Camera fpsCam;
    private WaitForSeconds laserDuration = new WaitForSeconds(0.07f); //Caching the object to the memory now, helps improve performance.
    private AudioSource laserAudio;
    private LineRenderer laserLine;
    private float nextFire;

    private void Start()
    {
        laserAudio = GetComponent<AudioSource>();
        laserLine = GetComponent<LineRenderer>();
        fpsCam = GetComponentInParent<Camera>(); //Stores a reference of the first object of <type> it encounters.
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1") && Time.time > nextFire)
        {
            StartCoroutine(ShotEffect());
            nextFire = Time.time + fireRate;

            Vector3 rayOrigin = fpsCam.ViewportToWorldPoint(new Vector3(.5f, .5f)); //Sets the origin of the line to the center of the camera.
            RaycastHit hit; //Used to store the information returned from our ray
            laserLine.SetPosition(0, gunEnd.position);

            //Two possible outcomes:
            //1.We hit an object
            //2.We shoot the sky and nothing happens
            if (Physics.Raycast(rayOrigin,fpsCam.transform.forward, out hit, weaponRange)) //Returns true if it hits something
            {
                laserLine.SetPosition(1, hit.point); //In case we shoot an object 
                ShootableBox healt = hit.collider.GetComponent<ShootableBox>();
                if (healt != null)
                {
                    healt.Damage(damage);
                }

                if (hit.rigidbody != null)
                {
                    hit.rigidbody.AddForce(-hit.normal * hitForce);
                }
            }
            else
            {
                laserLine.SetPosition(1, rayOrigin + (fpsCam.transform.forward * weaponRange)); //Set the end position to the specified range
            }
        }


        //Handle physics - Physics force appliance
    }

    private IEnumerator ShotEffect() //Shows the "laser"
    {
        laserAudio.Play();
        laserLine.enabled = true;
        yield return laserDuration;
        laserLine.enabled = false;
    }
}
