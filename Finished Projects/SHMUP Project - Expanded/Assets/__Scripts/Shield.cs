using UnityEngine;

public class Shield : MonoBehaviour
{
    [Header("Set in Inspector")]
    [Range(0f,1f)]
    public float rotationPerSecond = 0.1f; //The shields rotation Speed

    [Header("Set Dynamically")]
    public int levelShown = 0;

    Material mat;

    private void Start()
    {
        mat = GetComponent<Renderer>().material;
    }

    private void Update()
    {
        int curLevel = Mathf.FloorToInt(Hero.S.ShieldLevel); //Store the shield level from the Hero script

        if (levelShown != curLevel)
        {
            levelShown = curLevel;
            mat.mainTextureOffset = new Vector2(0.2f * levelShown, 0);
        }

        //Rotate the shield a bit every frame
        float rZ = -(rotationPerSecond * Time.time * 360) % 360f;
        transform.rotation = Quaternion.Euler(0, 0, rZ);
    }
}
