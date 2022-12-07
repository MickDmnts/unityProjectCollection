using UnityEngine;

public class WeaponMuzzleFlash : MonoBehaviour
{
    [Header("Set in Inspector")]
    public Transform muzzleFlashPrefab;
    public Transform firePoint;

    private void Start()
    {
        CheckIfVariablesAreSet();
    }

    void CheckIfVariablesAreSet()
    {
        if (muzzleFlashPrefab == null)
        {
            Debug.Log("Muzzle flash is not set");
        }

        if (firePoint == null)
        {
            Debug.Log("Fire point on muzzle script is not set");
        }
    }

    public Transform InstantiateMuzzleFlash(float muzzleFlashScale)
    {
        Transform weaponMuzzle = Instantiate(muzzleFlashPrefab, firePoint.position, firePoint.rotation);
        weaponMuzzle.localScale = new Vector3(muzzleFlashScale, muzzleFlashScale, muzzleFlashScale);
        return weaponMuzzle;
    }
}
